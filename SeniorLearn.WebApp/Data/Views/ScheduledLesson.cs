namespace SeniorLearn.WebApp.Data.Views
{
    public class ScheduledLesson
    {
        public int Id { get; set; }
        public string Title  { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int ClassDurationInMinutes { get; set; }
        public int TimetableId { get; set; }
        public int DeliveryPatternId { get; set; }
        public int TopicId { get; set; }
        public string Topic { get; set; } = string.Empty ;
        public int StatusId { get; set; }
        public string Professor { get; set; } = string.Empty;   

        public int ProfessionalId { get; set; }
        public bool IsCourse { get; set; }

        public bool IsEditable { get; set; } = false;
    }
}
