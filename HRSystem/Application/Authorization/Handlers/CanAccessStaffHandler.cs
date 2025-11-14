using HRSystem.Application.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Application.Authorization.Handlers
{
    public class CanAccessStaffHandler : AuthorizationHandler<CanAccessStaffRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext ctx, CanAccessStaffRequirement req)
        {
            if (ctx.User.HasClaim("AccessStaff", req.StaffNo)) ctx.Succeed(req);
            return Task.CompletedTask;
        }
    }
}
