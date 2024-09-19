using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Data.Views;

namespace SeniorLearn.WebApp.Data.Configuration
{
    public class EntityMapper
    {
        public EntityMapper(ModelBuilder mb)
        {
            mb.HasDefaultSchema("org");

            mb.Entity<Organisation>(o => 
            {
                o.HasOne(o => o.Timetable)
                .WithOne(t => t.Organisation)
                .HasForeignKey<Timetable>(t => t.OrganisationId)
                .OnDelete(DeleteBehavior.Restrict); 

                o.HasMany(o => o.Topics)
                    .WithOne(t => t.Organisation)
                    .HasForeignKey(t => t.OrganisationId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });


            mb.Entity<User>(u =>
            {
                u.HasOne(u => u.Member)
                .WithOne(m => m.User)
                .IsRequired(required: false)
                .OnDelete(DeleteBehavior.Restrict);

                u.HasMany(u => u.Roles)
                    .WithOne()
                    .HasPrincipalKey(u => u.Id)
                    .OnDelete(DeleteBehavior.Restrict);
                    
            });

            mb.Entity<UserRoleType>(ur =>
            {

                ur.HasKey(ur => ur.Id);
                ur.UseTptMappingStrategy();

                ur.HasOne(ur => ur.User)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(ur => ur.UserId);

               ur.HasMany(ur => ur.Updates);

                mb.Entity<Standard>();

                mb.Entity<Professional>(p => { 
                    p.HasMany(p => p.DeliveryPatterns)
                    .WithOne(dp => dp.Professional)
                    .HasForeignKey(l => l.ProfessionalId)
                    .OnDelete(DeleteBehavior.Restrict); 
                });

                mb.Entity<Honorary>();
      
            });

            

            mb.Entity<Member>(m =>
            {
                m.Property(p => p.OutstandingFees).HasColumnType("decimal(5, 2)");
                m.Ignore(m => m.Roles);

                m.HasMany(m => m.Enrolments)
                .WithOne(e =>e.Member)
                .OnDelete(DeleteBehavior.Restrict);

                m.HasMany(m => m.Payments)
                .WithOne(e =>e.Member)  
                .OnDelete(DeleteBehavior.Restrict); 
               
            });

            mb.Entity<Payment>(p => 
            {
                p.UseTpcMappingStrategy();
                p.Property(p => p.Amount).HasColumnType("decimal(5, 2)");
                mb.Entity<Cash>().ToTable("PaymentsCash", schema: "finance");
                mb.Entity<Cheque>().ToTable("PaymentsCheque", schema: "finance");
                mb.Entity<CreditCard>().ToTable("PaymentsCreditCard", schema: "finance");
                mb.Entity<ElectronicFundTransfer>().ToTable("PaymentsElectronicFundTransfer", schema: "finance");

            });

            mb.Entity<Timetable>(t => 
            {
                t.ToTable("Timetables", schema: "timetable");
                t.HasMany(t => t.Lessons)
                .WithOne(l => l.Timetable)
                .OnDelete(DeleteBehavior.Restrict);

            });

            mb.Entity<DeliveryPattern>(dp => 
            {
                dp.ToTable("DeliveryPatterns", schema: "timetable");
             
                dp.UseTphMappingStrategy();

                dp.HasOne(dp=> dp.Professional)
                .WithMany(p => p.DeliveryPatterns)
                .HasForeignKey(dp => dp.ProfessionalId)
                .OnDelete(DeleteBehavior.Restrict);

                dp.Property(dp => dp.DeliveryMode)
                .HasColumnName("DeliveryMode")
                .HasConversion<int>();

                mb.Entity<NonRepeating>();
                mb.Entity<Daily>();
                mb.Entity<Weekly>();
            });

            mb.Entity<Enrolment>(e =>
            {
                e.ToTable("Enrolments", schema: "timetable");
                e.Property(e => e.Status)
                .HasColumnName("Status")
                .HasConversion<int>();
            });

            //Topic
            mb.Entity<Topic>(t =>
            {
                t.ToTable("Topics", schema: "timetable");
                t.HasMany(t => t.Lessons)
                .WithOne(t => t.Topic)
                .OnDelete(DeleteBehavior.Restrict);   
            });

            //Lesson
            mb.Entity<Lesson>(l => 
            {

                l.Property(l => l.StatusId).IsRequired().HasDefaultValue(1);
                l.ToTable("Lessons", schema: "timetable", t =>
                {
                    t.HasCheckConstraint("CK_Lessons_StatusId", "StatusId >= 1 AND StatusId <= 5");

                });
                l.HasIndex(l => l.Start);
                //l.Ignore(l => l.IsEditable);

                l.UseTphMappingStrategy();
                mb.Entity<LessonOnline>();
                mb.Entity<LessonOnPremises>();
            });

            mb.Entity<ScheduledLesson>(
                sl => {
                    sl.HasNoKey();
                    sl.Ignore(sl => sl.IsEditable);
                    sl.ToView("ViewScheduledLessons", schema: "timetable");
                    
                });
            ;




        }
    }
}
