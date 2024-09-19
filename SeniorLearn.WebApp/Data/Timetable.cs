using SeniorLearn.WebApp.Data.Identity;

namespace SeniorLearn.WebApp.Data
{
    public class Timetable
    {
        public int Id { get; set; }
        //Organisation
        public int OrganisationId { get; set; }
        public virtual Organisation Organisation { get; set; } = default!;
        public virtual List<Lesson> Lessons { get; set; } = new();
      
        //Schedule Lesson
        public Lesson ScheduleLesson
            (
                Professional professional, string name, string description, DateTime start, int classDurationInMinutes, DeliveryPattern deliveryPattern, Topic topic, Lesson.DeliveryModes deliveryMode, string location = "", string url = "") 
        {
            //Todo: implement delivery mode validation
            Lesson lesson;
            switch (deliveryMode) 
            {
                case Lesson.DeliveryModes.OnPremises:
                    lesson = new LessonOnPremises(this, topic, name, description, start, deliveryPattern, classDurationInMinutes, location);
                    break;
                case Lesson.DeliveryModes.Online:
                    lesson = new LessonOnline(this, topic, deliveryPattern, name, description, start, classDurationInMinutes, url);
                    break;
                    default:
                    throw new ArgumentException("Invalid lesson delivery mode provided");
            }
            Lessons.Add(lesson);//add lesson to lessons list
            deliveryPattern.Lessons.Add(lesson);//add to deliverypattern
            return lesson;
        }
    }
}
