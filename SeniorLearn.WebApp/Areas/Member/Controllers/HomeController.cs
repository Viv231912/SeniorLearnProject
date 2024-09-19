using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Services.Member;

namespace SeniorLearn.WebApp.Areas.Member.Controllers
{
    public class HomeController : MemberAreaController
    {
        private readonly MemberService _memberService;
        public HomeController(ApplicationDbContext context, MemberService memberService) : base(context)
        {
            _memberService = memberService; 
        }


        public async Task<IActionResult> Index()
        {
            //Find member by using claimPrinciple 
            var member = await _context.FindMemberAsync(User); 
            //Find enrolments details by using member id and also include deliverypattern which belongs to a lesson.
            var enrolments = await _context.Enrolments.Where(e=> e.MemberId == member.Id)
                .Include(e => e.Lesson)
                .ThenInclude(l => l.DeliveryPattern)
                .OrderBy(e => e.Lesson.Start).ToListAsync();

            member.Enrolments = enrolments;

            return View(member);
        }
    }
}
