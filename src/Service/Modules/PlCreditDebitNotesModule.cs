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
            this.Get("/purchasing/open-debit-notes", this.GetOpenDebitNotes);
            this.Get("/purchasing/pl-credit-debit-notes", this.SearchNotes);
            this.Get("/purchasing/pl-credit-debit-notes/{id}", this.GetNote);
            this.Put("/purchasing/pl-credit-debit-notes/{id}", this.UpdateDebitNote);
        }

        private async Task GetOpenDebitNotes(HttpRequest req, HttpResponse res)
        {
            var results = this.service.FilterBy(new PlCreditDebitNoteResource
                                                    {
                                                        DateClosed = null,
                                                        NoteType = "D"
                                                    });
            
            await res.Negotiate(results);
        }

        private async Task SearchNotes(HttpRequest req, HttpResponse res)
        {
            var search = req.Query.As<string>("searchTerm");

            var results = this.service.Search(search);

            await res.Negotiate(results);
        }

        private async Task UpdateDebitNote(HttpRequest req, HttpResponse res)
        {
            var resource = await req.Bind<PlCreditDebitNoteResource>();
            resource.Who = req.HttpContext.User.GetEmployeeNumber();
            var result = this.service.Update(
                req.RouteValues.As<int>("id"),
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetNote(HttpRequest req, HttpResponse res)
        {
            var result = this.service.GetById(
                req.RouteValues.As<int>("id"));

            await res.Negotiate(result);
        }
    }
}
