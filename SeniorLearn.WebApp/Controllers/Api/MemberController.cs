using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Controllers.Api
{
   
    public class MemberController : ApiControllerBase
    {
        public MemberController(ApplicationDbContext context) : base(context){}
        
        //Member Enrolments
        [HttpGet, Route("enrolments"), Authorize(Roles = "STANDARD")]
        public async Task<IActionResult> Enrolments([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var member = await _context.FindMemberAsync(User);

            if (member == null)
            {
                return NotFound();
            }

            var enrolments = await _context.Enrolments
                .Where(e => e.MemberId == member.Id)
                .Where(e => e.Lesson.Start.AddMinutes(e.Lesson.ClassDurationInMinutes) > start && e.Lesson.Start < end)
                .Select(e =>
                new
                {
                    e.Id,
                    e.MemberId,
                    e.LessonId,
                    e.Status,
                    e.Lesson.Start,
                    e.Lesson.ClassDurationInMinutes

                }).ToListAsync();

            return Ok(enrolments);
        }
    }
}
