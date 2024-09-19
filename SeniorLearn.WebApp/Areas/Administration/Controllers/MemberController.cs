using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PasswordGenerator;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Extensions;
using SeniorLearn.WebApp.Services.Member;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    public class MemberController : AdministrationAreaController
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public MemberController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper) : base(context)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        //Get All Members
        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                    .Include(m => m.User)
                    .OrderBy(m => m.LastName)
                    .ThenBy(m =>m.FirstName)
                    .ToArrayAsync();
                    

            return View(members);
        }

        [HttpGet]
        //Get Member Registration form
        public IActionResult Register() 
        {
           
            return View();
        }


        [HttpPost]
        //Post Member Registration form
        public async Task<IActionResult> Register(Models.Member.Register m) 
        {
            //check if provide email is already exists
            var existingUser = await _userManager.FindByEmailAsync(m.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A member with the provided email is already exists.");
     
            }

            if (ModelState.IsValid) 
            {
                //Create new user 
                User newUser = new User { UserName = m.Email, Email = m.Email };
                var password = new Password().Next();

                password = "s123!@#";

                var result = await _userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoleType.RoleTypes.STANDARD.ToString());

                    var organisation = await _context.Organisations.FirstAsync();

                    if (organisation == null)
                    {
                        return NotFound();
                    }
                   
                    var member = new Data.Member
                    {
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Email = m.Email,
                        User = newUser,
                        RenewalDate = CalculateRenewalDate(),
                        OutstandingFees = 0.0m
                       
                    };
         
                    organisation!.Members.Add(member);
                    newUser.Member = member;
                    member.UpdateStandardRole(true, notes: "Initial Registration");
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
      
            }
             return View(m);
        }

        [HttpGet]
        //Manage member
        public async Task<IActionResult> Manage(int id)
        {
            var member = await _context.FindMemberAsync(id);

            if (member == null) 
            {
                return NotFound();  
            }
            //map models manage to member
            var model = _mapper.Map<Models.Member.Manage>(member);
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Manage(Models.Member.Manage member) 
        {
            var existingMember = await _context.FindMemberAsync(member.Id); 
              
            if (existingMember == null) 
            {
                return NotFound();  
            }
            if (ModelState.IsValid)
            {
                existingMember.FirstName = member.FirstName;
                existingMember.LastName = member.LastName;
                existingMember.DateOfBirth = member.DateOfBirth;
                existingMember.RenewalDate = member.RenewalDate;
                existingMember.OutstandingFees = member.OutstandingFees;
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction("Manage", new { Id = member.Id});

        }


        public async Task<IActionResult> RoleUpdates(int id) 
        {

            var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) 
            {
                return NotFound();  
            }

            var roleUpdateHistory = _context.RoleUpdates.Where(u => u.Id == member.Id).ToList();
            return View(roleUpdateHistory);
        } 
        //Post
        //Get

        //Update Standard MemberRole
        [HttpPost]
        public async Task<IActionResult> UpdateStandardRole(int id, int active)
        {
            //find the member
            var member = await _context.FindMemberAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.UpdateStandardRole(active.ToBool(), $"Role: {(active.ToBool() ? "A" : "Dea")}ctivated");
            await _context.SaveChangesAsync();
            return RedirectToAction("Manage", new { id });
        }


        //Update Professional MemberRole
        [HttpPost]
        public async Task<IActionResult> UpdateProfessionalRole(int id, int active, int renewal)
        {
            //find the member
            var member = await _context.FindMemberAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            member.UpdateProfessionalRole("",active.ToBool(), $"Role: {(active.ToBool() ? "A" : "Dea")}ctivated");

            // if the standard member role is active and renewal date is greater than zero
            if (active.ToBool() && renewal > 0) 
            {
                var now = DateTime.Now;
                var proposed = now;
                if (renewal == 3)
                {
                    var nextRenewalDate = new DateTime(now.Year, now.Month, 1);

                    if (now.Day > 1)
                    {
                        nextRenewalDate = nextRenewalDate.AddMonths(1);
                    }

                    proposed = nextRenewalDate.AddMonths(3);
                }

                if (renewal == 12) 
                {
                    var nextRenewalDate = new DateTime(now.Year, now.Month, 1);
                    if (now.Day > 1) 
                    {
                        nextRenewalDate = nextRenewalDate.AddMonths(1);
                    }
                    proposed = nextRenewalDate.AddMonths(12);
                }

                member.RenewalDate = proposed > member.RenewalDate ? proposed : member.RenewalDate;

                var roleName = UserRoleType.RoleTypes.PROFESSIONAL.ToString();
                var user = await _userManager.FindByIdAsync(member?.UserId!);
                if (!(await _userManager.IsInRoleAsync(user!, roleName))) 
                {
                    await _userManager.AddToRoleAsync(user!, roleName);  
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Manage", new { id });
        }



        //Update Honorary MemberRole
        public async Task<IActionResult> GrantHonoraryRole(int id)
        {
            var member = await _context.FindMemberAsync(id);
            if (member == null) 
            {
                return NotFound();
            }

            bool isActive = true;
            member.UpdateHonoraryRole(ref isActive, $"Role: Honorary (Granted)");
            await _context.SaveChangesAsync();
            return RedirectToAction("Manage", new { id });
        }


        public DateTime CalculateRenewalDate()
        {
            var registrationDate = DateTime.Now;
            var renewalDate = registrationDate.AddMonths(1);
            renewalDate = new DateTime(renewalDate.Year, renewalDate.Month, 1);
            return renewalDate;
        }

        
        
    }
}
