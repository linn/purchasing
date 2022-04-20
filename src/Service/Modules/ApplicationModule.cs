namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ApplicationModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/", this.Redirect);
            app.MapGet("/purchasing", this.GetApp);
            app.MapGet("/purchasing/signin-oidc-client", this.GetApp);
            app.MapGet("/purchasing/signin-oidc-silent", this.GetSilentRenew);
        }

        private Task Redirect(HttpRequest req, HttpResponse res)
        {
            res.Redirect("/purchasing");
            return Task.CompletedTask;
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetSilentRenew(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "SilentRenew.html" });
        }
    }
}
