namespace Linn.Purchasing.Service.Host.Negotiators
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Carter;

    using HandlebarsDotNet;

    using Linn.Common.Configuration;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class HtmlNegotiator : IResponseNegotiator
    {
        private readonly IViewLoader viewLoader;

        public HtmlNegotiator(IViewLoader viewLoader)
        {
            this.viewLoader = viewLoader;
        }

        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return accept.MediaType.Equals("text/html");
        }

        public async Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            var viewName = model is ViewResponse viewResponse
                ? viewResponse.ViewName
                : "Index.html";

            var view = this.viewLoader.Load(viewName);

            var template = Handlebars.Compile(view);

            var jsonAppSettings = JsonConvert.SerializeObject(
                new ApplicationSettings
                {
                    AppRoot = ConfigurationManager.Configuration["APP_ROOT"],
                    ProxyRoot = ConfigurationManager.Configuration["PROXY_ROOT"],
                    CognitoHost = ConfigurationManager.Configuration["COGNITO_HOST"],
                    CognitoClientId = ConfigurationManager.Configuration["COGNITO_CLIENT_ID"],
                    CognitoDomainPrefix = ConfigurationManager.Configuration["COGNITO_DOMAIN_PREFIX"],
                    EntraLogoutUri = ConfigurationManager.Configuration["ENTRA_LOGOUT_URI"]
                },
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            var viewModel = new ViewModel
            {
                AppSettings = jsonAppSettings,
                BuildNumber = ConfigurationManager.Configuration["BUILD_NUMBER"]
            };

            res.ContentType = "text/html";
            res.StatusCode = (int)HttpStatusCode.OK;

            await res.WriteAsync(template(viewModel), cancellationToken);
        }
    }
}
