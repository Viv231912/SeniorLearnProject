using SeniorLearn.WebApp.Data.Identity;
using SeniorLearn.WebApp.Services.Member;

namespace SeniorLearn.WebApp.Areas.Administration.Models.Member
{
    public class Manage : Register
    {
        public int Id { get; set; }
        public DateTime RenewalDate { get; set; }
        public decimal OutstandingFees { get; set; }
        public List<UserRoleType> Roles { get; set; } = new();
        
        public string? Discipline { get; set; } 
        public User? User { get; set; } 
        public bool IsActiveStandardMember { get; set; }

        public bool IsActiveProfessionalMember { get; set; }    
        public bool IsActiveHonoraryMember { get; set; }
    }
}
