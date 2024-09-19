using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data.Configuration;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Data.Views;
using System.Security.Claims;

namespace SeniorLearn.WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
                       
        }
        public DbSet<Organisation> Organisations { get; set; }  
        public DbSet<Member> Members { get; set; }  
        public DbSet<UserRoleType> UserRolesTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RoleUpdate> RoleUpdates { get; set; }  
       public DbSet<Timetable> Timetables { get; set; } 
        public DbSet<DeliveryPattern> DeliveryPatterns { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ScheduledLesson> ScheduledLessons { get; set; }    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            new EntityMapper(builder);
            new DataSeeder(builder);
            //SeedTopics(builder);
        }
        private void SeedTopics(ModelBuilder mb)
        {
            mb.Entity<Topic>().HasData(
               new Topic { Id = 1, OrganisationId = 1, Name = "Philosophy", Description = "Philosophy is the study of fundamental questions related to existence, knowledge, values, reason, and language through critical, analytical, and systematic approaches." },
               new Topic { Id = 2, OrganisationId = 1, Name = "Mathematics", Description = "Mathematics involves the study of quantity, structure, space, and change, forming the foundation for understanding abstract concepts and physical phenomena." },
               new Topic { Id = 3, OrganisationId = 1, Name = "History", Description = "History examines past events to understand human societies, analyzing documents and artifacts to interpret societal development and change." },
               new Topic { Id = 4, OrganisationId = 1, Name = "Biology", Description = "Biology is the science of life, studying living organisms, their structure, function, growth, origin, evolution, and distribution." },
               new Topic { Id = 5, OrganisationId = 1, Name = "Computer Science", Description = "Computer Science focuses on the theory, design, and application of computer systems, covering areas such as algorithms, computation, and data processing." },
               new Topic { Id = 6, OrganisationId = 1, Name = "Physics", Description = "Physics studies matter, energy, and the fundamental forces of the universe, seeking to understand the laws governing physical phenomena." },
               new Topic { Id = 7, OrganisationId = 1, Name = "Chemistry", Description = "Chemistry investigates the properties and behavior of matter, exploring how substances interact with energy and undergo changes." },
               new Topic { Id = 8, OrganisationId = 1, Name = "Economics", Description = "Economics analyzes how individuals and societies allocate resources to satisfy needs and wants, examining the consequences of those decisions." },
               new Topic { Id = 9, OrganisationId = 1, Name = "English Literature", Description = "English Literature explores written works in the English language, emphasizing literary analysis and interpretation across genres." },
               new Topic { Id = 10, OrganisationId = 1, Name = "Environmental Science", Description = "Environmental Science studies the interactions between the environment's physical, chemical, and biological components." },
               new Topic { Id = 11, OrganisationId = 1, Name = "Art History", Description = "Art History examines the development of visual arts across cultures and periods, including painting, sculpture, and architecture." },
               new Topic { Id = 12, OrganisationId = 1, Name = "Political Science", Description = "Political Science studies government systems, political behavior, and the theoretical and practical aspects of politics." },
               new Topic { Id = 13, OrganisationId = 1, Name = "Psychology", Description = "Psychology is the scientific study of the mind and behavior, exploring how humans perceive, think, feel, and act." },
               new Topic { Id = 14, OrganisationId = 1, Name = "Sociology", Description = "Sociology investigates human social behavior, including the development, structure, and functioning of human society." },
               new Topic { Id = 15, OrganisationId = 1, Name = "Music Theory", Description = "Music Theory analyzes the elements of music, including harmony, melody, rhythm, and form, to understand musical composition." },
               new Topic { Id = 16, OrganisationId = 1, Name = "Philosophy of Science", Description = "Philosophy of Science examines the foundations, methods, and implications of science, questioning the nature of scientific knowledge." },
               new Topic { Id = 17, OrganisationId = 1, Name = "World Religions", Description = "World Religions explores various global belief systems, their teachings, practices, and impacts on societies." },
               new Topic { Id = 18, OrganisationId = 1, Name = "Anthropology", Description = "Anthropology studies humans, their ancestors, and related primates, focusing on cultural, social, and physical aspects." },
               new Topic { Id = 19, OrganisationId = 1, Name = "Linguistics", Description = "Linguistics studies language structure, development, and variation, including syntax, semantics, phonetics, and phonology." },
               new Topic { Id = 20, OrganisationId = 1, Name = "Creative Writing", Description = "Creative Writing focuses on the art of writing fiction and poetry, emphasizing creativity, narrative techniques, and expression." }
           );

        }



        public async Task<Member?> FindMemberAsync(int id)
        {
            return await Members
                .Include(m => m.User)
                    .ThenInclude(u => u.Roles)
                        .ThenInclude(r => r.Updates)
                .Include(m => m.Organisation)
                        .ThenInclude(m => m.Timetable)
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        //Find member using claim principal
        public async Task<Member> FindMemberAsync(ClaimsPrincipal principal)
        {
            return await Members
                .Include(m => m.User)
                    .ThenInclude(u => u.Roles)
                        .ThenInclude(r => r.Updates)
                .Include(m => m.Organisation)
                    .ThenInclude(m => m.Timetable)
                        .FirstAsync(m => m.User.UserName == principal.Identity!.Name);
        }

    }
}
