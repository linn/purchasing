﻿namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class ApplicationModule : CarterModule
    {
        public ApplicationModule()
        {
            this.Get("/", this.Redirect);
            this.Get("/template", this.GetApp);
            this.Get("/template/signin-oidc-client", this.GetApp);
            this.Get("/template/signin-oidc-silent", this.GetSilentRenew);
        }

        private Task Redirect(HttpRequest req, HttpResponse res)
        {
            res.Redirect("/template");
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
