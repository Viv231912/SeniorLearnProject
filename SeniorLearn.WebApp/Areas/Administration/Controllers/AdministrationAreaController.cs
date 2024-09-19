using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Administration.Controllers
{
    [Area("Administration"), Authorize(Roles = "ADMINISTRATION")]
    public class AdministrationAreaController : Controller
    {
        protected readonly ApplicationDbContext _context;
        public AdministrationAreaController(ApplicationDbContext context)
        {
            _context = context; 
        }
       
    }
}
