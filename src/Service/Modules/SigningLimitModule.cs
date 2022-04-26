namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class SigningLimitModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/signing-limits", this.GetSigningLimits);
            app.MapGet("/purchasing/signing-limits/{id:int}", this.GetSigningLimitById);
            app.MapGet("/purchasing/signing-limits/application-state", this.GetApplicationState);
            app.MapPost("/purchasing/signing-limits", this.CreateSigningLimit);
            app.MapPut("/purchasing/signing-limits/{id:int}", this.UpdateSigningLimit);
            app.MapDelete("/purchasing/signing-limits/{id:int}", this.DeleteSigningLimitById);
        }

        private async Task GetSigningLimits(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            await res.Negotiate(signingLimitFacadeService.GetAll());
        }

        private async Task GetApplicationState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            await res.Negotiate(signingLimitFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetSigningLimitById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            var result = signingLimitFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task CreateSigningLimit(
            HttpRequest request,
            HttpResponse response,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService,
            SigningLimitResource resource)
        {
            var result = signingLimitFacadeService.Add(
                resource,
                request.HttpContext.GetPrivileges(),
                request.HttpContext.User.GetEmployeeNumber());

            await response.Negotiate(result);
        }

        private async Task UpdateSigningLimit(
            HttpRequest request,
            HttpResponse response,
            int id,
            SigningLimitResource resource,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            var result = signingLimitFacadeService.Update(
                id,
                resource,
                request.HttpContext.GetPrivileges(),
                request.HttpContext.User.GetEmployeeNumber());

            await response.Negotiate(result);
        }

        private async Task DeleteSigningLimitById(
            HttpRequest req,
            HttpResponse res,
            int id,
            IFacadeResourceService<SigningLimit, int, SigningLimitResource, SigningLimitResource> signingLimitFacadeService)
        {
            var result = signingLimitFacadeService.DeleteOrObsolete(
                id,
                req.HttpContext.GetPrivileges(),
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }
    }
}
