namespace Linn.Purchasing.Service.Modules
{
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PurchaseOrderReqModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/purchase-orders/reqs", this.SearchReqs);
            app.MapGet("/purchasing/purchase-orders/reqs/states", this.GetReqStates);
            app.MapGet("/purchasing/purchase-orders/reqs/print", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/reqs/{id:int}/print", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/reqs/create", this.GetApp);
            app.MapGet("/purchasing/purchase-orders/reqs/application-state", this.GetReqApplicationState);
            app.MapGet("/purchasing/purchase-orders/reqs/{id:int}", this.GetReq);
            app.MapPut("/purchasing/purchase-orders/reqs/{id:int}", this.UpdateReq);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/cancel", this.CancelReq);
            app.MapPost("/purchasing/purchase-orders/reqs/email", this.EmailReq);
            app.MapPost("/purchasing/purchase-orders/reqs/email-for-authorisation", this.EmailForReqAuthorisation);
            app.MapPost("/purchasing/purchase-orders/reqs/email-for-finance", this.EmailForReqFinanceCheck);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/authorise", this.AuthoriseReq);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/finance-authorise", this.FinanceAuthoriseReq);
            app.MapPost("/purchasing/purchase-orders/reqs/{id:int}/turn-into-order", this.TurnIntoMiniOrder);
            app.MapPost("/purchasing/purchase-orders/reqs/check-signing-limit-covers-po-auth", this.CheckIfSigningLimitCoversOrder);
            app.MapPost("/purchasing/purchase-orders/reqs", this.CreateReq);
        }

        private async Task AuthoriseReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Authorise(id, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task CheckIfSigningLimitCoversOrder(
            HttpRequest req,
            HttpResponse res,
            int reqNumber,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.CheckIfSigningLimitCoversOrder(reqNumber, req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task FinanceAuthoriseReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.FinanceAuthorise(id, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task TurnIntoMiniOrder(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.CreateMiniOrderFromReq(id, req.HttpContext.GetPrivileges(), req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }

        private async Task CancelReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.DeleteOrObsolete(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task EmailReq(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService,
            int reqNumber)
        {
            var toEmailAddress = req.Query.As<string>("toEmailAddress");

            using var ms = new MemoryStream();

            await req.Body.CopyToAsync(ms);
            var result = purchaseOrderReqFacadeService.SendEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmailAddress,
                reqNumber,
                ms);

            await res.Negotiate(result);
        }

        private async Task EmailForReqAuthorisation(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService,
            int reqNumber,
            int toEmployeeId)
        {
            var result = purchaseOrderReqFacadeService.SendAuthorisationRequestEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmployeeId,
                reqNumber);

            await res.Negotiate(result);
        }

        private async Task EmailForReqFinanceCheck(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService,
            int reqNumber,
            int toEmployeeId)
        {
            var result = purchaseOrderReqFacadeService.SendFinanceCheckRequestEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                toEmployeeId,
                reqNumber);

            await res.Negotiate(result);
        }

        private async Task CreateReq(
            HttpRequest req,
            HttpResponse res,
            PurchaseOrderReqResource resource,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Add(resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.GetById(id, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetReqApplicationState(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            await res.Negotiate(purchaseOrderReqFacadeService.GetApplicationState(req.HttpContext.GetPrivileges()));
        }

        private async Task GetReqStates(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceService<PurchaseOrderReqState, string, PurchaseOrderReqStateResource, PurchaseOrderReqStateResource> purchaseOrderReqStateService)
        {
            var result = purchaseOrderReqStateService.GetAll();

            await res.Negotiate(result);
        }

        private async Task SearchReqs(
            HttpRequest req,
            HttpResponse res,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService,
            string reqNumber,
            string part,
            string supplier)
        {
            var result = purchaseOrderReqFacadeService.FilterBy(
                new PurchaseOrderReqSearchResource
                    {
                        ReqNumber = reqNumber, Part = part, Supplier = supplier
                    },
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task UpdateReq(
            HttpRequest req,
            HttpResponse res,
            int id,
            PurchaseOrderReqResource resource,
            IPurchaseOrderReqFacadeService purchaseOrderReqFacadeService)
        {
            var result = purchaseOrderReqFacadeService.Update(id, resource, req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
