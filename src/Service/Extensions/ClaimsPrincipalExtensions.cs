namespace Linn.Purchasing.Service.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Http;

    public static class ClaimsPrincipalExtensions
    {
        public static IEnumerable<string> GetPrivileges(this HttpContext context)
        {
            return context.User?.Claims?.Count() == 0 
                       ? new List<string>() 
                       : context.User?.Claims?.Where(b => b.Type == "privilege").Select(a => a.Value);
        }

        public static string GetEmployeeUrl(this ClaimsPrincipal principal)
        {
            return principal?.Claims
                .FirstOrDefault(claim => claim.Type.Equals("employee", StringComparison.InvariantCultureIgnoreCase))
                ?.Value;
        }

        public static int GetEmployeeNumber(this ClaimsPrincipal principal)
        {
            int.TryParse(principal.GetEmployeeUrl()?.Split('/').Last(), out var employee);

            return employee;
        }

        public static bool HasClaim(this ClaimsPrincipal principal, string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return principal.Identities.Any(identity => identity.HasClaim(type));
        }
    }
}
