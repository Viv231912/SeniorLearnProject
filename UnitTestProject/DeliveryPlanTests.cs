using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Areas.Administration.Controllers;
using SeniorLearn.WebApp.Areas.Member.Controllers;
using SeniorLearn.WebApp.Areas.Member.Models.DeliveryPattern;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using System.Security.Claims;
using Xunit.Priority;
using AdminstrationVMs = SeniorLearn.WebApp.Areas.Administration.Models;
using Identity = SeniorLearn.WebApp.Data.Identity;

namespace UnitTestProject
{
    [CollectionDefinition(nameof(MemberTestCollection))]
    public class DeliveryPlanTestCollection : ICollectionFixture<TestDatabaseFixture> 
    {
        public static string TestMemberEmail = "Test@Citizen.email";
    }


    [Collection(nameof(DeliveryPlanTestCollection)), TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class DeliveryPlanTests : IClassFixture<TestDatabaseFixture> 
    {
        #region Constructor & Properties
        public DeliveryPlanTests(TestDatabaseFixture fixture)
        {
            Fixture = fixture;
            Context = fixture.CreateContext();
            UserManager = new UserManager<User>(null!, null!,new PasswordHasher<User>(),null!,null!,null!, null!, null!, null!);

            var configuration = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<SeniorLearn.WebApp.Mapper.AuotMapperProfile>();
            });
            Mapper = new Mapper(configuration); 
            
        }

        public TestDatabaseFixture Fixture { get; } 
        public ApplicationDbContext Context { get; }    
        public UserManager<User> UserManager { get; } 
        public Mapper Mapper { get; set; }   

        [Fact, Priority(0)]
        public async Task ShouldScheduleOnPremisesLessons()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var mdoel = new AdminstrationVMs.Member.Register
            {
                FirstName = "Professional",
                LastName = "Test",
                DateOfBirth = new DateOnly(1940, 2, 8),
                Email = "professional@test.email"
            };

            await controller.Register(mdoel);
            var member = await GetMember();
            await controller.UpdateProfessionalRole(member.Id, 1, 1);

            var deliveryPatternController = new DeliveryPatternController(Context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity([new Claim
                        (ClaimTypes.Name, "professional@test.email")], "TestAuthentication"))
                    }
                }
            };

            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, 1, 9 ,0, 0).AddMonths(1);

            var m = new Create
            {
                DeliveryModeId = (int)Lesson.DeliveryModes.OnPremises,
                EndStrategyId = 1,
                Initialize = true,
                StartOn = start,
                Occurences = 8,
                IsCourse = true,
                Location = "Senior Learn Main Office (Leture room AZ.3)",
                PatternType = 1, //repeating - daily
                Template = new Create.Lesson
                {
                    Name = "Test Lesson",
                    Description = "Test Lesson",
                    ClassDurationInMinutes = 60,
                    TopicId = 1,
                }
            };
            await deliveryPatternController.Create(m);

            var firstLesson = member.ProfessionalRole.DeliveryPatterns.FirstOrDefault()?.Lessons.FirstOrDefault();
            Assert.NotNull(firstLesson);
            Assert.Equal(start, firstLesson.Start);
            Assert.Equal(Lesson.DeliveryModes.OnPremises, firstLesson.DeliveryMode);
        }

        #endregion

        #region Helpers
        public async Task<Member> GetMember()
            => await Context.Members
                .Include(m => m.Organisation.Timetable)
                    .ThenInclude(t => t.Lessons)
                .Include(m => m.User)
                    .ThenInclude(u => u.Roles)
                        .ThenInclude(ur => ur.RoleType)
                .FirstAsync(u => u.Email == "professional@test.email");

        #endregion
    }
}