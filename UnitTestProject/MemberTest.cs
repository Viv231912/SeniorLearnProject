using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SeniorLearn.WebApp.Areas.Administration.Controllers;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Priority;
using Identity = SeniorLearn.WebApp.Data.Identity;
using AdminstrationVMs = SeniorLearn.WebApp.Areas.Administration.Models;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace UnitTestProject
{
    [CollectionDefinition(nameof(MemberTestCollection))]
    public class MemberTestCollection : ICollectionFixture<TestDatabaseFixture>
    {
        public static string TestMemberEmail = "member@test.emal";
    }

    [Collection(nameof(MemberTestCollection))]
    public class MemberTests : ICollectionFixture<TestDatabaseFixture> 
    {
        #region Constructor & Properties
       public MemberTests(TestDatabaseFixture fixture) 
        {
            Fixture = fixture;
            Context = fixture.CreateContext();
            UserManager = new UserManager<User>(null!, null!, new PasswordHasher<Identity.User>(), null!, null!, null!, null!, null!, null!); 
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

        #endregion

        #region Tests
        [Fact, Priority(0)]
        public async Task ShouldRegisterMemberWithActiveStandardRole()
        {
            var controller = new MemberController(Context,UserManager, Mapper);
            var model = new AdminstrationVMs.Member.Register
            {
                FirstName = "Test",
                LastName = "Citizen",
                DateOfBirth = new DateOnly(1940, 2, 8),
                Email = MemberTestCollection.TestMemberEmail
            };

            await controller.Register(model);
            var member = await GetMember();

        }
        [Fact, Priority(1)]
        public async Task ShouldDeactivateStandardRole()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var member = await GetMember();
            Assert.True(member.IsActiveStandardMember, "Member should be active prior to test");
            await controller.UpdateStandardRole(member.Id, 0);
            member = await GetMember();
            Assert.False(member.IsActiveStandardMember);

        }
        [Fact, Priority(2)]
        public async Task ShouldActivateStandardRole()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var member = await GetMember();
            Assert.False(member.IsActiveStandardMember, "Member should NOT be active prior to test");
            await controller.UpdateStandardRole(member.Id, 1);
            member = await GetMember();
            Assert.True(member.IsActiveStandardMember);

        }


        [Fact, Priority(3)]
        public async Task ShouldActivateProfessionalRole()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var member = await GetMember();
            Assert.False(member.IsActiveProfessionalMember, "Member should NOT be active prior to test");
            await controller.UpdateProfessionalRole(member.Id, 1, 0);
            member = await GetMember();
            Assert.True(member.IsActiveProfessionalMember);

        }

        [Fact, Priority(4)]
        public async Task ShouldDeactivateProfessionalRole()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var member = await GetMember();
            Assert.True(member.IsActiveProfessionalMember, "Member should be active prior to test");
            await controller.UpdateProfessionalRole(member.Id, 0, 0);
            member = await GetMember();
            Assert.False(member.IsActiveProfessionalMember);

        }

        [Fact, Priority(5)]
        public async Task ShouldActivateHonoraryRole()
        {
            var controller = new MemberController(Context, UserManager, Mapper);
            var member = await GetMember();
            Assert.False(member.IsActiveHonoraryMember, "Member should NOT be active prior to test");
            await controller.GrantHonoraryRole(member.Id);
            member = await GetMember();
            Assert.True(member.IsActiveHonoraryMember);

        }

        #endregion

        #region Helpers
        public async Task<Member> GetMember()
            => await Context.Members
                            .Include(m => m.User)
                                .ThenInclude(u => u.Roles)
                                     .ThenInclude(ur => ur.RoleType)
                        .FirstAsync(u => u.Email == MemberTestCollection.TestMemberEmail);
        #endregion


    }
}
