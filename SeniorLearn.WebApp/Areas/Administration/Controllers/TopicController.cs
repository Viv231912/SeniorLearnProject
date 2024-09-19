using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    public class TopicController : AdministrationAreaController
    {
        public TopicController(ApplicationDbContext context) : base(context) { }


        //Get Topics 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.ToListAsync());
        }

        //Get Single Topic details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
            {
                return NotFound();  
            }
            var topic = await _context.Topics.FirstOrDefaultAsync(x => x.Id == id); 
            if (topic == null) 
            {
                return NotFound();
            }
            return View(topic); 
        }

        //Create topic, retrieve initial topic create page
        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        //Post topic, bind topic id, name, description, and organisation details with the new topic
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Description, Organisation")] Topic topic) 
        {

            var organisation = await _context.Organisations.FirstOrDefaultAsync();

            topic.Organisation = organisation!;

            //Check if ModelState is valid
            if (ModelState.IsValid) 
            {
                //organisation.Timetable.Organisation.Topics = topic.OrganisationId;
                _context.Topics.Add(topic);
                await _context.SaveChangesAsync();  
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        //Edit 
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            {
                return NotFound();  
            }
            var topic = await _context.Topics.FindAsync(id);

            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Description")] Topic topic)
        {
            if (id != topic.Id)
            {
                return NotFound("Topic not found");
            }

            if (ModelState.IsValid)
            {
                _context.Update(topic);
                await _context.SaveChangesAsync();  
                return RedirectToAction(nameof(Index));
            }

            return View(topic);

        }

        //Delete
        public async Task<IActionResult> DeleteTopic(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            if (topic == null) 
            {
                return NotFound("Topic Not Found");
            }

            //if there is scheduled lessons link to the Topic, therefore it can't be remove
            var hasScheduledLessons = _context.Lessons
                .Any(Lesson => Lesson.TopicId == topic.Id && 
                        _context.Enrolments.Any(enrolment => enrolment.LessonId == Lesson.Id));

            if (hasScheduledLessons) 
            {
                ModelState.AddModelError("", "This topic has lessons with existing enrolments and cannot be deleted.");
                return View(topic);
            }

            if (ModelState.IsValid) 
            {
                _context.Remove(topic); 
                await _context.SaveChangesAsync();  
                return RedirectToAction(nameof(Index)); 
            }
            return View(topic); 
        }
    }
}
