namespace Linn.Purchasing.Service.Host
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class MultiAuthHandler : AuthenticationHandler<MultiAuthOptions>
    {
        private readonly JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

        public MultiAuthHandler(
            IOptionsMonitor<MultiAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var cognitoIssuer = this.Options.CognitoIssuer;
            var cognitoScheme = this.Options.CognitoScheme;
            var legacyScheme = this.Options.LegacyScheme;

            if (!this.Request.Headers.TryGetValue("Authorization", out var hdr))
            {
                return await this.Context.AuthenticateAsync(legacyScheme);
            }

            var header = hdr.ToString();
            if (!header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return await this.Context.AuthenticateAsync(legacyScheme);
            }

            var token = header.Substring("Bearer ".Length).Trim();

            JwtSecurityToken jwt;
            try
            {
                jwt = this.jwtHandler.ReadJwtToken(token);
            }
            catch
            {
                return await this.Context.AuthenticateAsync(legacyScheme);
            }

            // decide which scheme to forward to based on the issuer
            if (jwt.Issuer != null && jwt.Issuer.Equals(cognitoIssuer, StringComparison.OrdinalIgnoreCase))
            {
                return await this.Context.AuthenticateAsync(cognitoScheme);
            }

            return await this.Context.AuthenticateAsync(legacyScheme);
        }
    }
}
