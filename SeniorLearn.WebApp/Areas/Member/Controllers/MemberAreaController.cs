using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Member.Controllers
{
    [Area("Member"), Authorize(Roles = "STANDARD")]
    public class MemberAreaController : Controller
    {
        protected readonly ApplicationDbContext _context;


        public MemberAreaController(ApplicationDbContext context)
        {
            _context = context;
        }
       
    }
}
