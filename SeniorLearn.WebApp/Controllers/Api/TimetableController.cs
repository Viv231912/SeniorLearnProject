using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Controllers.Api
{

    [Authorize(Roles = "STANDARD")]
    public class TimetableController : ApiControllerBase
    {
        public TimetableController(ApplicationDbContext context) : base(context) { }

        [HttpPost, Route("lessons/{lessonId:int}/enrol")]
        public async Task<IActionResult> Enrol(int lessonId)
        {
            //find member
            var member = await _context.FindMemberAsync(User);
            if (member == null)
            {
                return NotFound();
            }
            //load enrolments belongs to the member
            await _context.Entry(member).Collection(m => m.Enrolments).LoadAsync();
            //find lesson
            var lesson = await _context
                .Lessons
                .Include(l => l.DeliveryPattern)
                    .ThenInclude(dp => dp.Professional)
                 .Include(l => l.DeliveryPattern)
                     .ThenInclude(dp => dp.Lessons)
                .Include(l => l.Enrolments)
                .Include(l => l.Timetable.Organisation)
                .FirstAsync(l => l.Id == lessonId);


            var enrolments = lesson.Enrol(member);
            await _context.SaveChangesAsync();

            var result = enrolments.Select(e => new
            {
                e.Id,
                e.Lesson.Name,
                DeliveryPattern = e.Lesson.DeliveryPattern.Name,
                e.Status,
                e.Lesson.StatusType
            }).ToArray();

            return Ok(result);
        }


        //Update lessons by professional
        [HttpPut, Route("lessons/{id:int}/start"), Authorize(Roles = "PROFESSIONAL")]
        public async Task<IActionResult> UpdateLessonStartTime([FromRoute] int id, [FromBody] DateTime start)
        {
            var member = await _context.FindMemberAsync(User);
            if (!member.IsActiveProfessionalMember)
            {
                return BadRequest();
            }

            //find lesson
            var lesson = await _context.Lessons
                .Include(l => l.Enrolments).Include(l => l.DeliveryPattern)
                .FirstAsync(l => l.Id == id);

            lesson.Start = start;
            int result = await _context.SaveChangesAsync();
            return Ok(new { Result = result });
        }



        [HttpGet, Route("lessons")]
        public async Task<IActionResult> Lessons(DateTime start, DateTime end)
        {
            try
            {
                var member = await _context.FindMemberAsync(User);
                var now = DateTime.Now;
                var query = _context.ScheduledLessons.Where(l => l.End > start && l.Start < end);

                if (member.IsActiveProfessionalMember)
                {
                    query = query.Where(l => l.StatusId == (int)Lesson.Statuses.Scheduled || l.ProfessionalId == member.ProfessionalRole.Id);
                }
                else
                {
                    query = query.Where(l => l.StatusId == (int)Lesson.Statuses.Scheduled);
                }

                //var enrolments = await _context.Enrolments
                //    .Where(e => e.MemberId == member.Id)
                //    .Select(e => e.LessonId)
                //    .ToListAsync();

                var result = (await query.ToArrayAsync()).Select(l =>
                {
                    l.IsEditable = l.ProfessionalId == (member?.ProfessionalRole?.Id ?? -1)
                    && member!.IsActiveProfessionalMember
                    && l.StatusId == (int)Lesson.Statuses.Draft
                    && l.Start > DateTime.Now;
                    return l;

                }).ToArray();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "An error occurred while fetching lessons");
            }





            //if (User.IsInRole("PROFESSIONAL") && member!.IsActiveProfessionalMember)
            //{
            //    var result = lessons.Select(l => 
            //    {
            //        if (l.StatusId == (int)Lesson.Statuses.Draft && l.Start > now) 
            //        {
            //            l.IsEditable = true;
            //        }
            //        return l;
            //    }).ToArray();
            //    return Ok(result); 
            //}

            //return Ok(lessons);


        }

        [HttpGet, Route("lessons/{lessonId:int}")]
        public async Task<IActionResult> GetLesson(int lessonId)
        {
            try
            {

                var lesson = await _context.Lessons
            .Include(l => l.DeliveryPattern)
                .ThenInclude(dp => dp.Professional)
            .Include(l => l.DeliveryPattern)
                .ThenInclude(dp => dp.Lessons)
            .Include(l => l.Enrolments)
            .Include(l => l.Timetable.Organisation)
            .FirstOrDefaultAsync(l => l.Id == lessonId);

                return Ok(lesson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching lesson by ID {lessonId}: {ex.Message}");
                return StatusCode(500, "An error occurred while fetching the lesson");
            }
        }

        [HttpDelete, Route("lessons/{lessonId:int}"), Authorize(Roles = "PROFESSIONAL")]
        public async Task<IActionResult> DeleteLesson(int lessonId)
        {

            var member = await _context.FindMemberAsync(User);
            if (member.IsActiveProfessionalMember)
            {
                //find lesson
                var lesson = await _context.Lessons
                    .FirstOrDefaultAsync(l => l.Id == lessonId);
                //check for null
                if (lesson == null)
                {
                    return NotFound();
                }

                if (lesson.StatusId != (int)Lesson.Statuses.Draft)
                {
                    return BadRequest("Only lesson in Draft status can be deleted.");
                }
                //Delete lesson 
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Lesson Deleted Successfully" });
            }
            else 
            {
                return BadRequest("You are not authorised to delete lesson.");
            }

           
            
        }
    }
}
