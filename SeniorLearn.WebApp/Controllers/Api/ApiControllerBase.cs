using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ApplicationDbContext _context;   
        public ApiControllerBase(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
