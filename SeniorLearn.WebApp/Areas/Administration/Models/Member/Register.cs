using SeniorLearn.WebApp.Data.Identity;
using System.ComponentModel;

namespace SeniorLearn.WebApp.Areas.Administration.Models.Member
{
    public class Register
    {
        
        [DisplayName("Given Name")]
        public string FirstName { get; set; } = default!;
        [DisplayName("Surname")]
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        [DisplayName("Date Of Birth")]
        public DateOnly DateOfBirth { get; set; }

        
    }
}
