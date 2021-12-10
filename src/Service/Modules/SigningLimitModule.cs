namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Http;

    public class SigningLimitModule : CarterModule
    {
        private readonly IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService;

        public SigningLimitModule(IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            this.signingLimitFacadeService = signingLimitFacadeService;
            this.Get("/purchasing/signing-limits", this.GetSigningLimits);
            this.Get("/purchasing/signing-limits/{id:int}", this.GetSigningLimitById);
            this.Get("/purchasing/signing-limits/application-state", this.GetApplicationState);
            this.Post("/purchasing/signing-limits", this.CreateSigningLimit);
            this.Put("/purchasing/signing-limits/{id:int}", this.UpdateSigningLimit);
            this.Delete("/purchasing/signing-limits/{id:int}", this.DeleteSigningLimitById);
        }

        private async Task GetSigningLimits(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(this.signingLimitFacadeService.GetAll());
        }

        private async Task GetApplicationState(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(this.signingLimitFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetSigningLimitById(HttpRequest req, HttpResponse res)
        {
            var signingLimitId = req.RouteValues.As<int>("id");

            var result = this.signingLimitFacadeService.GetById(signingLimitId, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateSigningLimit(HttpRequest request, HttpResponse response)
        {
            var resource = await request.Bind<SigningLimitResource>();
            var result = this.signingLimitFacadeService.Add(resource);

            await response.Negotiate(result);
        }

        private async Task UpdateSigningLimit(HttpRequest request, HttpResponse response)
        {
            var id = request.RouteValues.As<int>("id");
            var resource = await request.Bind<SigningLimitResource>();
            var result = this.signingLimitFacadeService.Update(id, resource);

            await response.Negotiate(result);
        }

        private async Task DeleteSigningLimitById(HttpRequest req, HttpResponse res)
        {
            var signingLimitId = req.RouteValues.As<int>("id");

            var result = this.signingLimitFacadeService.DeleteOrObsolete(signingLimitId, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
