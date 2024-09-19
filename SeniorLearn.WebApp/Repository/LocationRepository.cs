using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Repository
{
    public class LocationRepository
    {
        private readonly ApplicationDbContext _context;
        public LocationRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

//        public void AddLocation(string location) 
//        {
//            var locationAddress = new Lesson.DeliveryModes { LessonOnPremise = location };
//            _context.Lessons.Add(locationAddress);
//            _context.SaveChanges(); 
//1        }
    }
}
