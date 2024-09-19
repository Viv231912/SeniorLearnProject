using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Models;
using System.Diagnostics;
using SeniorLearn.WebApp.Services.Member;


namespace SeniorLearn.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly MemberService _memberService;
        private readonly RoleService _roleService;
        public HomeController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, MemberService memberService, RoleService roleService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _memberService = memberService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //User redirect
        public async Task<IActionResult> RedirectUser() 
        {
            var user = await _userManager.GetUserAsync(User);

            if (await _userManager.IsInRoleAsync(user!, "ADMINISTRATION"))
            {
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

            if (await _userManager.IsInRoleAsync(user!, "STANDARD"))
            
            {
                var isActiveMember = await _roleService.IsActiveStandardMember(user!);

                if (isActiveMember) 
                {
                    return RedirectToAction("Index", "Home", new { area = "Member" });
                }

            }
            if (await _userManager.IsInRoleAsync(user!, UserRoleType.RoleTypes.OTHER.ToString())) 
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
          
             return BadRequest();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
