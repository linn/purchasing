namespace Linn.Purchasing.Service.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    public static class ClaimsIdentityExtensions
    {
        public static bool HasClaim(this ClaimsIdentity identity, string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return identity.Claims.Any(claim =>
                string.Equals(claim.Type, type, StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrEmpty(claim.Value));
        }
    }
}
