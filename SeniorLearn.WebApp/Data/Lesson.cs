using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using SeniorLearn.WebApp.Data.Identity;
using System.Data;
using static SeniorLearn.WebApp.Data.Lesson.Status;

namespace SeniorLearn.WebApp.Data
{
    public abstract class Lesson
    {
        
     
        protected Lesson(int statusId, DateTime start) 
        {
            _start = start;
            StatusId = statusId;
            _status = InitStatus();
        }

        public Lesson(Timetable timetable, Topic topic, string name, string description, DateTime start, DeliveryPattern deliveryPattern, int classDurationInMinutes) : base()
        {
            if (!deliveryPattern.Professional.Active)
            {
                throw new ArgumentException("Member must have an active professional role to schedule lesson");
            }

            Timetable = timetable;
            Topic = topic;
            Name = name;
            Description = description;
            Start = start;
            DeliveryPattern = deliveryPattern;
            ClassDurationInMinutes = classDurationInMinutes;
            InitStatus();

        }


        public int Id { get; set; }
        public string? Name { get; set; }
        public string Description { get; set; } = string.Empty;
        protected DateTime _start;


        //Todo: Change start time
        public DateTime Start
        {
            //get start time
            get => _start;
            // check if initial status is in draft (1), otherwise return error
            set
            {
                //validation
                if (InitStatus().Type != Statuses.Draft)
                {
                    throw new LessonStatusExecption("Only lesson's in the status of Draft can have their start time changed");
                }

                _start = value;//assign start time to current value
            }
        }



        public int ClassDurationInMinutes { get; set; }
        public DateTime Finish => Start.AddMinutes(ClassDurationInMinutes);
        //Timetable
        public int TimetableId { get; set; }
        public virtual Timetable Timetable { get; set; } = default!;
        //DeliveryPattern
        public int DeliveryPatternId { get; set; }
        public DeliveryPattern DeliveryPattern { get; set; } = default!;

        //Delivery Mode
        public enum DeliveryModes { OnPremises = 1, Online = 2 }
        public abstract DeliveryModes DeliveryMode { get; protected set; }

        //Topic
        public int TopicId { get; set; }
        public Topic Topic { get; set; } = default!;

        //public bool IsEditable { get; set; } = false;
        public virtual List<Enrolment> Enrolments { get; set; } = new();
        //List of enrolement with enrolled member and id
        public List<Enrolment> Enrol(Member member) => DeliveryPattern.EnrolMember(member, Id);


        
        #region Status (State Machine Pattern)
        //Status
        public enum Statuses { Draft = 1, Scheduled = 2, Closed = 3, Completed = 4, Cancelled = 5 }
        public int StatusId { get; private set; }
        private Status _status = default!;
       
        

        //Todo: Update status
        public void UpdateStatus(Statuses statuses)
        {
            switch (statuses)
            {
                case Statuses.Scheduled: Open(); break;
                case Statuses.Closed: Closed(); break;
                case Statuses.Completed: Completed(); break;
                case Statuses.Cancelled: Cancel(); break;
            }
        }

        //Status Transition
        private Action<Status> transition()
        {
            return new Action<Status>(state =>
            {
                StatusId = (int)state.Type;
                _status = state;
            });
        }

        public void Open() { _status.Transition(transition(), Statuses.Scheduled); }
        public void Closed() { _status.Transition(transition(), Statuses.Closed); }
        public void Completed() { _status.Transition(transition(), Statuses.Completed); }
        public void Cancel() { _status.Transition(transition(), Statuses.Cancelled); }

        //Status Transitions
        private static readonly Dictionary<Statuses, Statuses[]> statusTransitions = new()
        {
            { Statuses.Draft, new[] { Statuses.Scheduled, Statuses.Cancelled } },
            { Statuses.Scheduled, new[] { Statuses.Closed, Statuses.Completed, Statuses.Cancelled }},
            { Statuses.Closed, new[] { Statuses.Completed, Statuses.Cancelled} }
        };

        public Statuses[] ValidTransitions 
        {
            get => statusTransitions.TryGetValue(StatusType, out var transitions) ? transitions : Array.Empty<Statuses>();
        }



        public Statuses StatusType => InitStatus().Type;
        private Status InitStatus()
        {
            if (_status == null)
            {
                
                _status = Create(this, StatusId == 0 ? (int)Statuses.Draft : StatusId);
               
            }
            
            return _status;

        }

        //create lesson status

        public abstract class Status 
        {
            protected Lesson _lesson;

            protected Status(Lesson lesson)
            {
                _lesson = lesson;
            }
            
            public static Status Create(Lesson lesson, int statusId)
            {

            //Todo: Implement switch statement
            switch (statusId)
            {
                case (int)Statuses.Draft:
                    return new Draft(lesson);
                case (int)Statuses.Scheduled:
                    return new Scheduled(lesson);
                case (int)Statuses.Closed:
                    return new Closed(lesson);
                case (int)Statuses.Completed:
                    return new Completed(lesson);
                case (int)Statuses.Cancelled:
                    return new Cancelled(lesson);
            }
            throw new ArgumentOutOfRangeException(nameof(statusId), "Invalid lesson status Id");
        }


            public string Name { get; set; } = "";

            public abstract Statuses Type { get; }
            //May need transition 
            public abstract void Transition(Action<Status> action, Statuses status);


            //Lesson in Draft
            public class Draft : Status
            {
           
                public Draft(Lesson lesson) : base(lesson) { }

                public override Statuses Type => Statuses.Draft;
                //Logic for the closing, completing and cancelling lesson
                //To be able to perform any of the above action, first the lesson has to be shceduled and open
                public override void Transition(Action<Status> action, Statuses status)
                {
                    if (status == Statuses.Scheduled)
                    {
                        action(new Scheduled(_lesson!));
                    }
                    else if (status == Statuses.Closed)
                    {
                        throw new Exception("Lesson must be scheduled before it can be closed.");
                    }
                    else if (status == Statuses.Completed)
                    {
                        throw new Exception("Lesson must be scheduled before it can be completed.");

                    }
                    else if (status == Statuses.Cancelled)
                    {
                        throw new Exception("Lesson must be scheduled before it can be cancelled.");
                    }

                }
            }

            //Scheduled Lesson
            public class Scheduled : Status
            {
               
                public Scheduled(Lesson lesson) : base(lesson)
                {
                }
                public override Statuses Type => Statuses.Scheduled;

                public override void Transition(Action<Status> action, Statuses status)
                {
                    if (status == Statuses.Closed)
                    {
                        action(new Closed(_lesson!));
                    }
                    else if (status == Statuses.Completed)
                    {
                        action(new Completed(_lesson!));
                    }
                    else if (status == Statuses.Cancelled)
                    {
                        action(new Cancelled(_lesson!));
                    }
                }
            }

            public class Closed : Status
            {
                
                public Closed(Lesson lesson) : base(lesson)
                {
                }
                public override Statuses Type => Statuses.Closed;

                public override void Transition(Action<Status> action, Statuses status)
                {
                    if (status == Statuses.Scheduled)
                    {
                        throw new Exception("A closed lesson cannot be changed to scheduled");
                    }
                    else if (status == Statuses.Completed)
                    {
                        action(new Completed(_lesson!));
                    }
                    else if (status == Statuses.Completed)
                    {
                        action(new Cancelled(_lesson!));
                    }
                }
            }

            //Completed Lesson
            public class Completed : Status
            {
               

                public Completed(Lesson lesson) : base(lesson) { }

                public override Statuses Type => Statuses.Completed;

                //Logic for completed lesson
                //When lessons are completed they can not have status change to OPEN, CLOSED,or CANCELLED
                public override void Transition(Action<Status> action, Statuses status)
                {
                    if (status == Statuses.Scheduled)
                    {
                        throw new Exception("A completed lesson can not have it's status changed to scheduled(Open).");
                    }
                    else if (status == Statuses.Closed)
                    {
                        throw new Exception("A completed lesson can not have it's status changed to Closed.");
                    }
                    else if (status == Statuses.Cancelled)
                    {
                        throw new Exception("A completed lesson can not have it's status changed to Cancelled.");
                    }
                }
            }

            //Cancelled Lesson
            public class Cancelled : Status
            {
                
                public Cancelled(Lesson lesson) : base(lesson)
                {

                }

                public override Statuses Type => Statuses.Cancelled;

                //No logic required for cancelled lesson
                public override void Transition(Action<Status> action, Statuses status) { }
            }

        }

        #endregion

    }


    public class LessonOnline : Lesson 
    {
        protected LessonOnline(int statusId, DateTime start, string url) : base(statusId, start)
        {
            Url = url;
        }

        public LessonOnline(Timetable timetable, Topic topic, DeliveryPattern deliveryPattern, string name, string description, DateTime start,  int classDurationInMinutes, string url): base (timetable, topic, name, description, start, deliveryPattern, classDurationInMinutes)
        {
            Url = url;
        }
        public string Url { get; set; }
        public override DeliveryModes DeliveryMode { get ; protected set; } = DeliveryModes.Online;

    }

    public class LessonOnPremises : Lesson
    {
        protected LessonOnPremises(int statusId, DateTime start, string location) : base(statusId, start)
        {
            Location = location;    
        }

        public LessonOnPremises(Timetable timetable, Topic topic, string name, string description, DateTime start, DeliveryPattern deliveryPattern, int classDurationInMinutes, string location) : base(timetable, topic, name, description, start, deliveryPattern, classDurationInMinutes)
        {
            Location = location;
        }
        public string Location { get; set; }
        public override DeliveryModes DeliveryMode { get; protected set ; } = DeliveryModes.OnPremises;
    }


    //Lesson status exception
    public class LessonStatusExecption : ApplicationException 
    {
        public LessonStatusExecption(string message) : base(message){}
    }
    
 } 