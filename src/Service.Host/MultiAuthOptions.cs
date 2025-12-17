namespace Linn.Purchasing.Service.Host
{
    using Microsoft.AspNetCore.Authentication;

    public class MultiAuthOptions : AuthenticationSchemeOptions
    {
        public string CognitoIssuer { get; set; }

        public string CognitoScheme { get; set; }

        public string LegacyScheme { get; set; }
    }
}
