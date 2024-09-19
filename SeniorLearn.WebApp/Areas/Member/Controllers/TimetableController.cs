using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;

namespace SeniorLearn.WebApp.Areas.Member.Controllers
{
    public class TimetableController : MemberAreaController
    {
        public TimetableController(ApplicationDbContext context) : base(context)
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Enrol()
        {
                //find member by claim principle
                var member = await _context.FindMemberAsync(User);

                var lessons = await _context.Lessons
                                .ToListAsync();
                var scheduledLessons = lessons.Where(l => (int)l.StatusId == (int)Lesson.Statuses.Scheduled).ToList();
                if (!scheduledLessons.Any())
                {
                    ViewData["ErrorMessage"] = "There are no lessons available for enrolment";
                }
                var m = new Models.Timetable.Calendar
                {
                    MemberId = member.Id,
                    ProfessionalId = member.Roles.Any(r => r.RoleType == UserRoleType.RoleTypes.PROFESSIONAL) ? member.ProfessionalRole.Id : 0,
                    Lessons = scheduledLessons
                  
                };
                return View(m);
            
           
        }
            

        public async Task<IActionResult> Enrol(int lessonId, int memberId)
        {
            if (lessonId == 0 || memberId == 0)
            {
                return BadRequest();
            }
            var enrolmentExists = await _context.Enrolments
                 .AnyAsync(e => e.LessonId == lessonId && e.MemberId == memberId);

            if (enrolmentExists)
            {
                return BadRequest("You had already enrolled in this lesson");
            }

            var member = await _context.Members.FindAsync(memberId);
            var lesson = await _context.Lessons.FindAsync(lessonId);

            var enrolment = new Enrolment(member!, lesson!);

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();  

            return RedirectToAction("Index", new { id = enrolment.Id});   
        }



       
     
    }
}
