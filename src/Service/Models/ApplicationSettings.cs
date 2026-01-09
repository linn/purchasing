namespace Linn.Purchasing.Service.Models
{
    using Linn.Common.Configuration;

    public class ApplicationSettings
    {
        public string CognitoHost { get; set; }

        public string AppRoot { get; set; }

        public string ProxyRoot { get; set; }

        public string CognitoClientId { get; set; }

        public string CognitoDomainPrefix { get; set; }

        public string EntraLogoutUri { get; set; }

        public static ApplicationSettings Get()
        {
            return new ApplicationSettings
            {
                CognitoHost = ConfigurationManager.Configuration["COGNITO_HOST"],
                AppRoot = ConfigurationManager.Configuration["APP_ROOT"],
                ProxyRoot = ConfigurationManager.Configuration["PROXY_ROOT"],
                CognitoClientId = ConfigurationManager.Configuration["COGNITO_CLIENT_ID"],
                CognitoDomainPrefix = ConfigurationManager.Configuration["COGNITO_DOMAIN_PREFIX"],
                EntraLogoutUri = ConfigurationManager.Configuration["ENTRA_LOGOUT_URI"]
            };
        }
    }
}
