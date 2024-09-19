using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace SeniorLearn.WebApp.Data.Identity
{
    public class UserRoleType
    {
        public enum RoleTypes { STANDARD, PROFESSIONAL, HONORARY, OTHER };

        public int Id { get; set; }
        public string UserId { get; set; } = default!;
        public User User { get; set; } = default!;
        public string RoleId { get; set; } = default!;
        public virtual RoleTypes RoleType => RoleTypes.OTHER;
        public virtual int Order => 0;
        public bool Active { get; set; }
        public List<RoleUpdate> Updates { get; set; } = new();
    }

    public class Standard : UserRoleType
    {
        public DateTime RegistrationDate { get; set; }
        public override RoleTypes RoleType => RoleTypes.STANDARD;
        public override int Order => 1;
    }

    public class Professional : UserRoleType 
    {
        public string Discipline { get; set; } = "";
        
        public override RoleTypes RoleType => RoleTypes.PROFESSIONAL;
        public override int Order => 2;

 
        public List<DeliveryPattern> DeliveryPatterns { get; set; } = new();

        public DeliveryPattern AddDeliveryPattern(
            string name,
            DateTime startsOn,
            DeliveryPattern.DeliveryPatternType patternType,
            bool isCourse = false,
            Lesson.DeliveryModes deliveryMode = Lesson.DeliveryModes.OnPremises,
            Repeating.EndStrategies endStrategy = default,
            int occurrence = 0,
            DateTime endsOn = default,
            IList<DayOfWeek> days = default!)
        {
            DeliveryPattern pattern;

            switch (patternType)
            {
                case DeliveryPattern.DeliveryPatternType.NonRepeating:
                    pattern = new NonRepeating(name, startsOn, isCourse, deliveryMode);
                    break;
                case DeliveryPattern.DeliveryPatternType.Daily:
                    pattern = new Daily(name, startsOn, isCourse, deliveryMode, endStrategy, occurrence, endsOn);
                    break;
                case DeliveryPattern.DeliveryPatternType.Weekly:
                    pattern = new Weekly(name, startsOn, isCourse, deliveryMode, endStrategy, occurrence, endsOn, days);
                    break;
                default: throw new NotImplementedException("No Delivery Pattern Type Found");
            }
            pattern.IsCourse = isCourse;
            pattern.Name = name;
            pattern.Professional = this;
            DeliveryPatterns.Add(pattern);
            return pattern;
        }

    }

    public class Honorary : UserRoleType 
    {
        public string Notes { get; set; } = "";
        public override RoleTypes RoleType => RoleTypes.HONORARY;
        public override int Order => 3;
    }
}
