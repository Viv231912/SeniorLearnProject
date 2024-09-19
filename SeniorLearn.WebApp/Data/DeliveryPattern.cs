using Microsoft.Build.Framework;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Services.Lesson;
using SeniorLearn.WebApp.Services.Member;

namespace SeniorLearn.WebApp.Data
{
    public abstract class DeliveryPattern
    {
        protected DeliveryPattern(){}

        protected DeliveryPattern(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode)
        {
            Name = name;
            StartOn = startOn;
            IsCourse = isCourse;
            DeliveryMode = deliveryMode;
        }

        //enum DeliveryPattern type
        public enum DeliveryPatternType { NonRepeating = 0,  Daily = 1, Weekly = 2}
        public abstract DeliveryPatternType PatternType { get; }
        //GenerateLessons 
        public abstract void GenerateLessons(Timetable timetable, Topic topic, string name, string description, int durationInMinutes);

        public int Id { get; set; }

        protected bool Initialized { get; private set; } = false;
        public string Name { get; set; } = string.Empty;
        public DateTime StartOn { get; set; }
        //Professional
        public int ProfessionalId {  get; set; }
        public Professional Professional { get; set; } = default!;
        public List<Lesson> Lessons { get; set; } = new();
        public Lesson.DeliveryModes DeliveryMode { get; set; }
        //public virtual IReadOnlyList<UserRoleType> Roles => User?.Roles!;
      
        public bool IsCourse { get; set; }


        //public bool IsInActiveRole(UserRoleType.RoleTypes roleTypes) => Roles.Any(r => r.RoleType == roleTypes && r.Active);

        //public bool IsActiveProfessionalMember => IsInActiveRole(UserRoleType.RoleTypes.PROFESSIONAL);
        //public bool IsActiveStandardMember => IsInActiveRole(UserRoleType.RoleTypes.STANDARD);




        //Todo: Enrol Member
        public virtual List<Enrolment> EnrolMember(Member member, int lessonId)
        {
            ValidateMember(member);

            var result = new List<Enrolment>();

            if (!IsCourse)
            {
                var lesson = GetLessonById(lessonId);
                ValidateLesson(lesson, member);

                if (!lesson.Enrolments.Any(e => e.MemberId == member.Id))
                {
                    var enrolment = new Enrolment(member, lesson, Enrolment.Statuses.Active);
                    result.Add(enrolment);
                    lesson.Enrolments.Add(enrolment);
                }
            }
            else 
            {
                foreach (var lesson in Lessons) 
                {
                    ValidateLesson(lesson, member);
                    if (!lesson.Enrolments.Any(e => e.MemberId == member.Id)) 
                    {
                        var enrolment = new Enrolment(member, lesson, Enrolment.Statuses.Active);
                        result.Add(enrolment);
                        lesson.Enrolments.Add(enrolment);
                    }
                }
            }
            return result;  

           
        }
        //Member validation

        private void ValidateMember(Member member)
        {
            if (!member.IsActiveStandardMember)
            {
                throw new ApplicationException("Only a Member with a current active role of 'Standard Member' can enrol in a lesson.");
            }
                //Todo: need to verify this will work, otherwise need to change to ProfessionalRole
            if (member.IsActiveProfessionalMember)
            {
                throw new ApplicationException("A Professional cannot enrol in their own lesson (or related Professional Entity is not loaded)");
            }

        }

        //Get the lesson id that member would like to enrol
        private Lesson GetLessonById(int lessonId) => Lessons.First(l => l.Id == lessonId);


        private void ValidateLesson(Lesson lesson, Member member) 
        {
            //Todo: check if the lesson status is scheduled
            if (lesson.StatusType != Lesson.Statuses.Scheduled) 
            {
                throw new ApplicationException("Enrolements can only be created for scheduled lessons.");
            }
            //check if the member have enrolled
            if (member.Enrolments.Any(e => e.Lesson == lesson)) 
            {
                throw new ApplicationException("A member can only enrol in a lesson once, existing enrolment detected. ");
            }
        }



    }

    public class NonRepeating : DeliveryPattern
    {
        /// <summary>
        /// To be able to access outside the class, constructor require to be created
        /// </summary>
        protected NonRepeating() {}
        public NonRepeating(string name, DateTime startsOn, bool isCourse, Lesson.DeliveryModes deliveryMode) : base(name, startsOn, isCourse, deliveryMode)
        {
            
        }
        public override DeliveryPatternType PatternType => DeliveryPatternType.NonRepeating;

        //To generate lesson, scheduleLesson has to be added in the timetable
        public override void GenerateLessons(Timetable timetable, Topic topic, string name, string description, int durationInMinutes)
        {
            timetable.ScheduleLesson(Professional, name, description, StartOn, durationInMinutes, this, topic, DeliveryMode);
            
        }
    }

    //Repeating lesson logic
    //repeating lesson maybe a course or same content delivery repeatly for certain period of time

    ///<summary>
    ///Creating repeating lesson as abstruct therefore the daily, weekly repeat can inherit from it 
    ///</summary>

    public abstract class Repeating : DeliveryPattern
    {

        protected Repeating() {}
        //start date and end date for repeating class
        //IsCourse how many lessons, end by number of lessons
        // professional, name, description,
        protected Repeating(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode, int endStrategyId) : base (name, startOn, isCourse, deliveryMode)
        {
            EndStrategyId = endStrategyId;
        }


        //Initialize Repeating class with additional properties for inheritance and validation
        protected Repeating(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode, EndStrategies endStrategy, int occurrence = 0, DateTime? endOn = null!) : this(name, startOn, isCourse, deliveryMode,(int)endStrategy) 
        {
            //Validation
            if (occurrence < 1 && !endOn.HasValue) 
            {
                throw new ArgumentException("Repeating Patterns require either the number of occurences to be greater than zero or an 'End On' date must be provided");
            }
            Occurrences = occurrence;
            EndOn = endOn.GetValueOrDefault();
        }


        //repeating pattern can be structure as per lessons and days, if it's course
        public enum EndStrategies { Occurrence = 1, Date = 2 }
        public int EndStrategyId { get; set; }
        public EndStrategies EndStrategy => (EndStrategies)EndStrategyId;
        public int Occurrences { get; set; }    
        public DateTime EndOn { get; set; } = DateTime.MinValue;
    }

    public class Daily : Repeating
    {
        public Daily(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode, EndStrategies endStrategy, int occurrence = 0, DateTime? endOn = default) : base(name, startOn, isCourse, deliveryMode, endStrategy, occurrence, endOn)
        {
        }

        protected Daily() {}
      
       

        public override DeliveryPatternType PatternType => DeliveryPatternType.Daily;

        public override void GenerateLessons(Timetable timetable, Topic topic, string name, string description, int durationInMinutes)
        {
            //Create index for StartOn 
            var index = StartOn;
            //if EndStrategies Date is selected
            if (EndStrategy == EndStrategies.Date)
            {
                // set lesson count initial state to 1
                int lessonCount = 1;
                while (index <= EndOn)
                {
                    //if the above condition is met, shhedule lesson
                    timetable.ScheduleLesson(Professional, $"{(IsCourse ? lessonCount++ : "")} {name}", description, index, durationInMinutes, this, topic, DeliveryMode);
                    index = index.AddDays(1);
                }
            }
            else if (EndStrategy == EndStrategies.Occurrence)
            {
                for(int i = 0; i < Occurrences; i++) 
                {
                    //Schedule lesson by Occurrence
                    timetable.ScheduleLesson(Professional, $"{(IsCourse ? i + 1 : "")} {name}", description, index, durationInMinutes, this, topic, DeliveryMode);
                    index = index.AddDays(1);   
                }
            }
        }
    }

    public class Weekly : Repeating 
    {
        protected Weekly() { }
        
        public Weekly(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode, EndStrategies endStrategy, int occurrence = 0, DateTime? endOn = default) : base(name, startOn, isCourse, deliveryMode, endStrategy, occurrence, endOn)
        {
            
        }

        //Add additional properties
        public Weekly(string name, DateTime startOn, bool isCourse, Lesson.DeliveryModes deliveryMode, EndStrategies endStrategy, int occurrence = 0, DateTime? endOn = default, IList<DayOfWeek> days = default!) : base(name, startOn, isCourse, deliveryMode, endStrategy, occurrence, endOn)
        {
            SetDays(days);
        }

        public override DeliveryPatternType PatternType => DeliveryPatternType.Weekly;

        public override void GenerateLessons(Timetable timetable, Topic topic, string name, string description, int durationInMinutes)
        {
            var index = StartOn;
            int count = 0, lessonCount = 1;

            while ((EndStrategy == EndStrategies.Date && index <= EndOn) || (EndStrategy == EndStrategies.Occurrence && count < Occurrences))
            {
                if (DoesDayMatch(index.DayOfWeek))
                {
                    timetable.ScheduleLesson(Professional, $"{(IsCourse ? lessonCount++ : "")} {name}", description, index, durationInMinutes, this, topic, DeliveryMode);
                }
                index = index.AddDays(1);
                if (index.DayOfWeek == DayOfWeek.Sunday)
                {
                    count++;
                }
            }
        }

        public void SetDays(IList<DayOfWeek> days)
        {
            //Initialize days of the week to false
            Sunday = false; Monday = false; Tuesday = false; Wednesday = false; Thursday = false; Friday = false; Saturday = false;

            foreach (var d in days) 
            {
                switch (d)
                {
                    case DayOfWeek.Sunday: Sunday = true; break;
                    case DayOfWeek.Monday: Monday = true; break;
                    case DayOfWeek.Tuesday: Tuesday = true; break;
                    case DayOfWeek.Wednesday: Wednesday = true; break;
                    case DayOfWeek.Thursday: Thursday = true; break;
                    case DayOfWeek.Friday: Friday = true; break;
                    case DayOfWeek.Saturday: Saturday = true; break;
                }
            }
        }

        private bool DoesDayMatch(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday: return Sunday;
                case DayOfWeek.Monday: return Monday;
                case DayOfWeek.Tuesday: return Tuesday;
                case DayOfWeek.Wednesday: return Wednesday;
                case DayOfWeek.Thursday: return Thursday;
                case DayOfWeek.Friday: return Friday;
                case DayOfWeek.Saturday: return Saturday;
                default: return false;
            }
        }

        



        //Declare the days of the week as bool value
        public bool Sunday { get; set; }    
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }   
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }    
        public bool Saturday { get; set; }
    }



}
