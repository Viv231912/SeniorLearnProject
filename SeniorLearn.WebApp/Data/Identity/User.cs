using Microsoft.AspNetCore.Identity;

namespace SeniorLearn.WebApp.Data.Identity
{
    public class User: IdentityUser
    {
        public virtual Member? Member { get; set; }

        public virtual List<UserRoleType> Roles { get; set; } = new();

        public bool IsInActiveRole(UserRoleType.RoleTypes roleType) => Roles.Any(r => r.RoleType == roleType && r.Active);
       

        public UserRoleType UpdateStandardRole(bool active = true, string notes = "")
        {
            if (Member == null) 
            {
                throw new ApplicationException("No Associated Member Object, User is either not a member or associated entity is not loaded");
            }
            var role = Roles.OfType<Standard>().FirstOrDefault();
            if (role == null)
            {
                role = new Standard
                {
                    Active = active,
                    RegistrationDate = DateTime.Now,
                    RoleId = UserRoleType.RoleTypes.STANDARD.ToString()
                };
                Roles.Add(role);
            }
            else 
            {
                role.Active = active;   
            }
            var update = new RoleUpdate
            {
                Active = role.Active,   
                Timestamp = DateTime.Now,    
                RenewalDate = Member.RenewalDate,
                Notes = $"{role.RoleType}: {notes.Trim().ToUpper()}"
            };
            role.Updates.Add(update);   
            return role;

        }

        public UserRoleType UpdateProfessionalRole(string discipline, bool active = true, string notes = "")
        {
            //check if member exist
            if (Member == null)
            {
                throw new ApplicationException("No Associated Member Object, User is either not a member or associated entity is not loaded");
            }

            //assign the professional to role
            var role = Roles.OfType<Professional>().FirstOrDefault();

            //if the role doesn't exist, create a new Professional
            if (role == null)
            {
                role = new Professional
                {
                    Active = active,
                    Discipline  = discipline,
                    RoleId = UserRoleType.RoleTypes.PROFESSIONAL.ToString()
                };
                Roles.Add(role);
            }
            else
            {
                role.Active = active;
                role.Discipline = discipline;   
            }

            //track the update
            var update = new RoleUpdate
            {
                Active = role.Active,
                Timestamp = DateTime.Now,
                RenewalDate = Member.RenewalDate,
                Notes = $"{role.RoleType} : {notes.Trim().ToUpper()}"
            };
            role.Updates.Add(update);
            return role;

        }

        //Update Honorary Role

        public UserRoleType UpdateHonoraryRole(ref bool active, string notes = "")
        {
            if (Member == null) 
            {
                throw new ApplicationException("No Associate Member Object, User is either not a member or associated entity is not loaded");
            }
            //find role
            var role = Roles.OfType<Honorary>().FirstOrDefault() ?? new Honorary
            {
                Active = active,
                RoleId = UserRoleType.RoleTypes.HONORARY.ToString(),
                Notes = ""
            };

            if (Roles.Contains(role))
            {
                Roles.Add(role);
            }
            else 
            {
                role.Active = active;
            }

            role.Updates.Add(new RoleUpdate 
            {
                Active = role.Active,
                Timestamp = DateTime.Now,   
                RenewalDate= Member.RenewalDate,
                Notes = $"{role.RoleType}: {notes.Trim().ToUpper()}"
            });
            active = role.Active;

            return role;

            //var role = Roles.OfType<Honorary>().FirstOrDefault();
            ////check if role exist
            //if (role == null)
            //{
            //    role = new Honorary
            //    {
            //        Active = active,
            //        RoleId = UserRoleType.RoleTypes.HONORARY.ToString(),
            //        Notes = ""
            //    };

            //    Roles.Add(role);
            //}
            //else 
            //{
            //    role.Active = active;   
            //}

            //var update = new RoleUpdate
            //{
            //    Active = role.Active,
            //    Timestamp = DateTime.Now,
            //    RenewalDate = Member.RenewalDate,
            //    Notes = $"{role.RoleType}: {notes.Trim().ToUpper()}"
            //};
            //role.Updates.Add(update);

            //active = role.Active;   

            //return role;
        }
    }
}
