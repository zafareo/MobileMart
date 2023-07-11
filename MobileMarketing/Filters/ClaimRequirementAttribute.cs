using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MobileMarketing.Filters
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }

        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Roles { get; set; }
        public string? Permissions { get; set; }
    }   
}
