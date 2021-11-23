namespace Linn.Purchasing.Service.Models
{
    using Linn.Common.Configuration;

    public class ApplicationSettings
    {
        public string AuthorityUri { get; set; }

        public string AppRoot { get; set; }

        public string ProxyRoot { get; set; }

        public static ApplicationSettings Get()
        {
            return new ApplicationSettings
                       {
                           AuthorityUri = ConfigurationManager.Configuration["AUTHORITY_URI"],
                           AppRoot = ConfigurationManager.Configuration["APP_ROOT"],
                           ProxyRoot = ConfigurationManager.Configuration["PROXY_ROOT"]
                       };
        }
    }
}
