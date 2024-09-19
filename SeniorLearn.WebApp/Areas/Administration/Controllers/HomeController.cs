using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    public class HomeController : AdministrationAreaController
    {
      
        public HomeController(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<IActionResult> Index()
        {
            var members = await _context.Members
                  .Include(m => m.User)
                  .OrderBy(m => m.Id)
                  //.OrderBy(m => m.LastName)
                  //.ThenBy(m => m.FirstName)
                  .ToArrayAsync();


            return View(members);
           
        }
    }
}
