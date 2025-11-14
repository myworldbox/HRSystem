using HRSystem.Application.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Application.Authorization.Handlers
{
    public class CanAccessContractFormHandler : AuthorizationHandler<CanAccessContractFormRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext ctx, CanAccessContractFormRequirement req)
        {
            if (ctx.User.HasClaim("AccessContractForm", "true")) ctx.Succeed(req);
            return Task.CompletedTask;
        }
    }
}
