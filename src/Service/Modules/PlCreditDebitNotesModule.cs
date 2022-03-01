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

    public class PlCreditDebitNotesModule : CarterModule
    {
        private readonly IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service;

        public PlCreditDebitNotesModule(
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)
        {
            this.service = service;
            this.Get("/purchasing/pl-credit-debit-notes", this.GetDebitNotes);
            this.Put("/purchasing/pl-credit-debit-notes/{id}", this.UpdateDebitNote);
        }

        private async Task GetDebitNotes(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PlCreditDebitNoteResource>();
            var results = this.service.FilterBy(new PlCreditDebitNoteResource
                                                    {
                                                        SupplierName = resource?.SupplierName
                                                    });
          

            await res.Negotiate(results);
        }

        private async Task UpdateDebitNote(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PlCreditDebitNoteResource>();
            var result = this.service.Update(
                req.RouteValues.As<int>("id"),
                resource,
                req.HttpContext.GetPrivileges(),
                req.HttpContext.User.GetEmployeeNumber());

            await res.Negotiate(result);
        }
    }
}
