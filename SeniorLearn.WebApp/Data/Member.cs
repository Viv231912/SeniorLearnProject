using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Services.Member;

namespace SeniorLearn.WebApp.Data
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; }= default!;    
        public DateOnly DateOfBirth { get; set; }
        public DateTime RenewalDate { get; set; }
        public decimal OutstandingFees { get; set; }

        public virtual Organisation Organisation { get; set; } = default!;
        public int OrganisationId { get; set; }
        public virtual List<Enrolment> Enrolments { get; set; } = new();
        public virtual List<Payment> Payments { get; set; } = new();
        public virtual User User { get; set; } = default!;
        public string? UserId { get; set; }

        public virtual IReadOnlyList<UserRoleType> Roles => User?.Roles!;

        public Professional ProfessionalRole => Roles.OfType<Professional>().FirstOrDefault()!;

        public bool IsInActiveRole(UserRoleType.RoleTypes roleType) => User.IsInActiveRole(roleType);

        public bool IsActiveStandardMember => IsInActiveRole(UserRoleType.RoleTypes.STANDARD);
        public bool IsActiveProfessionalMember => IsInActiveRole(UserRoleType.RoleTypes.PROFESSIONAL);
        public bool IsActiveHonoraryMember => IsInActiveRole(UserRoleType.RoleTypes.HONORARY);



        public UserRoleType UpdateStandardRole(bool active = true, string notes = "") => User.UpdateStandardRole(active, notes);

        public UserRoleType UpdateProfessionalRole(string discipline, bool active = true, string notes = "") => User.UpdateProfessionalRole(discipline, active, notes);


        public UserRoleType UpdateHonoraryRole(ref bool active, string notes = "") => User.UpdateHonoraryRole(ref active, notes);
        


    }
}
