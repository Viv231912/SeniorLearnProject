using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SeniorLearn.WebApp.Data;
using SeniorLearn.WebApp.Data.Identity;
using System.Data;

namespace SeniorLearn.WebApp.Services.Member
{
    public class MemberService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        public MemberService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<bool> IsActiveRole(string userId, UserRoleType.RoleTypes roleTypes) 
        {
            var member = await _context.Members
                .Include(m => m.User)
                    .ThenInclude(u => u.Roles)
                        .FirstOrDefaultAsync(m => m.UserId == userId);    

            if (member == null || member.User == null)
            {
                return false;
            }

            if (member.User.Roles == null || !member.User.Roles.Any()) 
            {
                return false;
            }

            return member.User.Roles.Any(r => r.RoleId == roleTypes.ToString() && r.Active);

        }

        public Task<bool> IsActiveStandardMember(string userId)
        {
            return IsActiveRole(userId, UserRoleType.RoleTypes.STANDARD);
        }

        public Task<bool> IsActiveProfessionalMember(string userId)
        {
            return IsActiveRole(userId, UserRoleType.RoleTypes.PROFESSIONAL);
        }


       
       
    }
}
