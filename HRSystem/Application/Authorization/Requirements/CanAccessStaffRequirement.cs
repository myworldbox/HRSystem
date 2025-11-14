using Microsoft.AspNetCore.Authorization;

namespace HRSystem.Application.Authorization.Requirements
{
    public class CanAccessStaffRequirement : IAuthorizationRequirement
    {
        public string StaffNo { get; }
        public CanAccessStaffRequirement(string staffNo) => StaffNo = staffNo;
    }
}
