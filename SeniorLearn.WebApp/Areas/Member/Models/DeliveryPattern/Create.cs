using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using static SeniorLearn.WebApp.Data.DeliveryPattern;
using static SeniorLearn.WebApp.Data.Lesson;
using static SeniorLearn.WebApp.Data.Repeating;

namespace SeniorLearn.WebApp.Areas.Member.Models.DeliveryPattern
{
    public class Create
    {
        public Create()
        {}


        //Method for creating a lesson
        public Create(ApplicationDbContext context, int organisationId, string lessonName = "", string lessonDescription = "", int durationInMinutes = 60 )
        {
            InitTemplate(context, organisationId, lessonName, lessonDescription, durationInMinutes).Wait();
      
        }


        public string Name { get; set; } = string.Empty;
        public DateTime StartOn { get; set; }
        public bool IsCourse { get; set; }
        
        public int EndStrategyId { get; set; }
        public int PatternType { get; set; }
       

        public int DeliveryModeId { get; set; }
        
        //List of delivery mode
        public SelectList DeliveryModes => GetDeliveryModes();

        private SelectList GetDeliveryModes()
        {
            var deliveryMode = Enum.GetValues<DeliveryModes>()
                .Select(dm => new { Value = (int)dm, Text = dm.ToString() });

            return new SelectList(deliveryMode, "Value", "Text");
        }

        
        public string? Url { get; set; } = "";
        
        public string? Location { get; set; } = "";

        [Range(0, 16)]
        public int Occurences { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndOn { get; set; } = DateTime.MinValue;
        public bool Sunday { get; set; }
        public bool Friday { get; set; }
        public bool Thursday { get; set; }
        public bool Wednesday { get; set; }
        public bool Tuesday { get; set; }
        public bool Monday { get; set; }
        public bool Saturday { get; set; }

        public DeliveryPatternType DeliveryPattern => (DeliveryPatternType)PatternType;

        public EndStrategies EndStrategy => (EndStrategies)EndStrategyId;


        public List<DayOfWeek> Days 
        {
            get 
            {
                var days = new List<DayOfWeek>();
                if (Sunday) 
                {
                    days.Add(DayOfWeek.Sunday);
                }
                if (Monday) 
                {
                    days.Add(DayOfWeek.Monday);
                }
                if (Tuesday)
                {
                    days.Add(DayOfWeek.Tuesday);
                }
                if (Wednesday) 
                {
                    days.Add (DayOfWeek.Wednesday);
                }
                if (Thursday)
                {
                    days.Add(DayOfWeek.Thursday);
                }
                if (Friday) 
                {
                    days.Add(DayOfWeek.Friday); 
                }
                if (Saturday) 
                {
                    days.Add(DayOfWeek.Saturday);   
                }
                return days;
            }
        }

        public bool Initialize {  get; set; }

        public Lesson? Template { get; set; }

        //Create a instance of Lesson
        public class Lesson
        {
            public string Name { get; set; } = "";
            public string Description { get; set; } = "";
            public int ClassDurationInMinutes { get; set; } = 0;
            public int TopicId { get; set; }
            public SelectList? Topics { get; set; } = default!;
        }

        public async Task InitTemplate(ApplicationDbContext context, int organisationId, string lessonName = "", string lessonDescription = "", int durationInMinutes = 60)
        {
            Template = new Lesson
            {
                Name = lessonName,
                Description = lessonDescription,
                ClassDurationInMinutes = durationInMinutes,
                Topics = await GetTopicList(context, organisationId)
           

            };
        }

        //Todo: Rebuild topics when new page loaded
        public async Task RebuildTopics(ApplicationDbContext context, int organisationId)
        {
            Template!.Topics = await GetTopicList(context, organisationId);
        }


        //Get Select List of topics
        private async Task<SelectList> GetTopicList(ApplicationDbContext context, int organisationId)
        {
            var topicList = new SelectList(await context.Topics
               .Where(t => t.OrganisationId == organisationId)
                   .ToArrayAsync(),
                   nameof(Topic.Id), nameof(Topic.Name));
            return topicList;
        }

       
    }
}
