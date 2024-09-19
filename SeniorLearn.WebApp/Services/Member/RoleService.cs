using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace SeniorLearn.WebApp.Services.Member
{
    public class RoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RoleService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;

        }

        public DbSet<UserRoleType> UserRoles { get => _context.Set<UserRoleType>(); }



        public virtual async Task<bool> IsInAdminRoleAsync(User user)
        {
            return await _userManager.IsInRoleAsync(user, "ADMINISTRATION");
        }


        private async Task<List<UserRoleType>> GetUserRoles(User user)
        {
            return await _context.UserRolesTypes
                .Include(ur => ur.User)
                .Where(ur => ur.User.Id == user.Id)
                .ToListAsync();
        }

        public async Task<bool> UserIsInActiveRole(User user, UserRoleType.RoleTypes roleType) 
        {
            var roles = await GetUserRoles(user);
            return roles.Any(r => r.RoleType == roleType && r.Active);
        }

        public async Task<bool> IsActiveStandardMember(User user)
        {
            return await UserIsInActiveRole(user, UserRoleType.RoleTypes.STANDARD);
        }


        //public async Task<bool> IsActiveProfessionalMember(User user) 
        //{
        //    return await UserIsInActiveRole(user, UserRoleType.RoleTypes.PROFESSIONAL);
        //}

        public async Task<bool> IsActiveProfessionalMember(ClaimsPrincipal claimsPrincipal) 
        {
            var userName = claimsPrincipal.Identity?.Name;
            var userRole = await _context.UserRolesTypes
                .Include(ur => ur.User)
                .FirstOrDefaultAsync(ur => ur.User.UserName == userName);
            return await UserIsInActiveRole(userRole!.User, UserRoleType.RoleTypes.PROFESSIONAL);
        }


        



        //public async Task<bool> HasActiveRoleAsync(User user)
        //{
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    foreach (var roleName in userRoles) 
        //    {
        //        var role = await _roleManager.FindByNameAsync(roleName);
        //        if (role != null && role.) 
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}


    }
}
