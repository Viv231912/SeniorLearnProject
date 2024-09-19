using SeniorLearn.WebApp.Data;

namespace SeniorLearn.WebApp.Areas.Member.Models.DeliveryPattern
{
    public class UpdateLesson
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public int ClassDurationInMinutes { get; set; }
        public int TimeTableId { get; set; }    
        public int TopicId { get; set; }
        public int StatusId { get; set; }
        public Lesson.Statuses Status => (Lesson.Statuses)StatusId;
        public Topic Topic { get; set; } = default!;
    }
}
