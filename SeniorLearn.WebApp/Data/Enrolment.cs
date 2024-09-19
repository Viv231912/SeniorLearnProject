namespace SeniorLearn.WebApp.Data
{
    public class Enrolment
    {

        private Enrolment()
        {
            
        }
        //Set the parameters for the Enrolment
        public Enrolment(Member member, Lesson lesson, Statuses status = Statuses.Active)
        {
            Member = member;
            Lesson = lesson;
            Status = status;
        }


        //Enrolment status
        public enum Statuses { Active = 1, Participated = 2, Withdrawn = 3, NoShow = 4}
        public Statuses Status { get; set; }    
        public int Id { get; set; }
        //Lesson
        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = default!;
        //Member
        public int MemberId { get; set; }
        public virtual Member Member { get; set; } = default!;


    }
}
