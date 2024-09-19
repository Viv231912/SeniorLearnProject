using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Member.Models.Timetable
{
    public class Calendar
    {
        public int MemberId { get; set; }
        public int ProfessionalId { get; set; } = 0;

        public List<Lesson> Lessons { get; set; } = new();
    }
}
