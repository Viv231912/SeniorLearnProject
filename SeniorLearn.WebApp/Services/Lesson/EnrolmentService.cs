using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Services.Lesson
{
    
    public class EnrolmentService
    {
        private readonly ApplicationDbContext _context;

        public EnrolmentService(ApplicationDbContext context)
        {
            _context = context;
        }

       
    }
}
