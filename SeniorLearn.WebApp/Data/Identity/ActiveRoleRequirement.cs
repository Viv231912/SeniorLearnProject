using Microsoft.AspNetCore.Authorization;

namespace SeniorLearn.WebApp.Data.Identity
{
    //public class ActiveRoleRequirement : IAuthorizationRequirement{}
    //    public class ActiveRoleHandler : AuthorizationHandler<ActiveRoleRequirement>
    //    {
    //    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveRoleRequirement requirement)
    //    {
    //        var userRoles = context.User.Claims.Where(c => c.Type == "role").Select(s => s.Value).ToList();
    //        var isActive = context.User.Claims.Any(c => c.Type == "Active" && c.Value
    //        == "Active");
    //    }

    //}
}
