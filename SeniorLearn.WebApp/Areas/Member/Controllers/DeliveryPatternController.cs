using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;

namespace SeniorLearn.WebApp.Areas.Member.Controllers
{
    public class DeliveryPatternController : MemberAreaController
    {
        public DeliveryPatternController(ApplicationDbContext context) : base(context){}


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //find member by claimprinciple 
            var member = await _context.FindMemberAsync(User);
            //find delivery pattern which belong to the login Professional User
            var deliveryPattern = await _context.DeliveryPatterns
                .Include(p => p.Lessons)
                .Where(p => p.ProfessionalId == member.ProfessionalRole.Id)
                .ToListAsync();

            return View(deliveryPattern);
        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            //Find member using claim principal
            var member = await _context.FindMemberAsync(User);
            //Create a new delivery pattern
            var m = new Models.DeliveryPattern.Create(_context, member!.OrganisationId)
            {
                // Initial set to none
                PatternType = 0, // 0 = None, 1 = Daily, 2 = Weekly
                EndStrategyId = 1, //1 = Occurence , 2 = Date
                Occurences = 1,
                StartOn = DateTime.Now,
                EndOn = DateTime.Now.AddDays(14),
                Initialize = true
            };

            return View(m);

        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Token validation
        public async Task<IActionResult> Create(Models.DeliveryPattern.Create m) 
        {
            var member = await _context.FindMemberAsync(User);
            //check modelState is valid
            if (ModelState.IsValid) 
            {
                //find user role
                var role = member!.Roles.OfType<Professional>().First();

                //delivery mode id
                var mode = (Lesson.DeliveryModes)m.DeliveryModeId;

                //assign properties to deliverypattern local variable
                var deliveryPattern = role.AddDeliveryPattern(m.Name, m.StartOn, m.DeliveryPattern, m.IsCourse, mode, m.EndStrategy, m.Occurences, m.EndOn, m.Days);

                //if Initialize selected
                if (m.Initialize) 
                {
                    //set the temple to current template
                    var template = m.Template;
                    var topic = await _context.Topics.FirstAsync(t => t.Id == template!.TopicId);
                    //Generate new lesson on current deliveryPattern
                    deliveryPattern.GenerateLessons(member.Organisation.Timetable, topic, template!.Name, template.Description, template.ClassDurationInMinutes);
                    //check if delivery pattern is a course
                    if (deliveryPattern.IsCourse) 
                    {
                        foreach (var l in deliveryPattern.Lessons)
                        {
                            l.Open();//set each lesson status to open
                        }
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //Rebuild the topics
            await m.RebuildTopics(_context, member!.OrganisationId);
            //Return new view
            return View(m);
        }


        //Todo: Get Lessons
        [HttpGet]
        public async Task<IActionResult> Lessons(int id)
        {
            var pattern = await _context.DeliveryPatterns
                .Include(dp => dp.Lessons).ThenInclude(l=>l.Topic)
                .Include(dp => dp.Lessons).ThenInclude(l => l.Enrolments)
                .FirstAsync(t => t.Id == id);
            return View(pattern);   
        }

        //Open Lessons
        [HttpPost]
        public async Task<IActionResult> OpenLesson(int id) 
        {
            //find lesson by id
            var lesson = await _context.Lessons.FirstAsync(t => t.Id == id);
            //set lesson to open
            lesson.Open();
            await _context.SaveChangesAsync();
            return RedirectToAction("Lessons", new { id = lesson.DeliveryPatternId });

        }
        //CloseLesson
        [HttpPost]
        public async Task<IActionResult> CloseLesson(int id)
        {
            //find lesson by id
            var lesson = await _context.Lessons.FirstAsync(l => l.Id == id);
            lesson.Closed();
            await _context.SaveChangesAsync();
            return RedirectToAction("Lessons", new { id = lesson.DeliveryPatternId });
        }


        //UpdateLesson
        [HttpPost]
        public async Task<IActionResult> UpdateLesson(Models.DeliveryPattern.UpdateLesson m)
        {
            var lesson = await _context.Lessons
                            .Include(l => l.Timetable)
                            .Include(l => l.DeliveryPattern)
                            .FirstAsync(l => l.Id == m.Id);

            lesson.UpdateStatus(m.Status);

            return View(lesson);
        }

    }
}
