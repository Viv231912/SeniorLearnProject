using Microsoft.AspNetCore.Mvc;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    public class TaskController : AdministrationAreaController
    {

        public TaskController(ApplicationDbContext context) : base(context)  { }

        public IActionResult Index()
        {
            return View();
        }
    }
}
