namespace Linn.Purchasing.Service.Modules
{
    using System.IO;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PlCreditDebitNotesModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchasing/pl-credit-debit-notes", this.CreateNote);
            app.MapGet("/purchasing/open-debit-notes", this.GetOpenDebitNotes);
            app.MapGet("/purchasing/pl-credit-debit-notes", this.SearchNotes);
            app.MapGet("/purchasing/pl-credit-debit-notes/{id}", this.GetNote);
            app.MapPut("/purchasing/pl-credit-debit-notes/{id}", this.UpdateDebitNote);
            app.MapPost("/purchasing/pl-credit-debit-notes/email", this.EmailDebitNote);
            app.MapGet("/purchasing/pl-credit-debit-notes/application-state", this.GetState);
        }

        private async Task GetState(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)

        {
            var result = service.GetApplicationState(req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task GetOpenDebitNotes(
            HttpRequest req,
            HttpResponse res,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)
        {
            var results = service.FilterBy(new PlCreditDebitNoteResource
                                               {
                                                   DateClosed = null,
                                                   NoteType = "D"
                                               });
            
            await res.Negotiate(results);
        }

        private async Task SearchNotes(
            HttpRequest req,
            HttpResponse res,
            string searchTerm,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)
        {
            var results = service.Search(searchTerm);

            await res.Negotiate(results);
        }

        private async Task UpdateDebitNote(
            HttpRequest req, 
            HttpResponse res,
            int id,
            PlCreditDebitNoteResource resource,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)
        {
            resource.Who = req.HttpContext.User.GetEmployeeNumber();
            var result = service.Update(
                id,
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }

        private async Task EmailDebitNote(
            HttpRequest req, 
            HttpResponse res,
            int id,
            IPlCreditDebitNoteEmailService emailService)
        {
            using var ms = new MemoryStream();
            
            await req.Body.CopyToAsync(ms);
            var result = emailService.SendEmail(
                req.HttpContext.User.GetEmployeeNumber(),
                id,
                ms);

            await res.Negotiate(result);
        }

        private async Task GetNote(
            HttpRequest req, 
            HttpResponse res,
            int id,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)

        {
            var result = service.GetById(id);

            await res.Negotiate(result);
        }

        private async Task CreateNote(
            HttpRequest req,
            HttpResponse res,
            PlCreditDebitNoteResource resource,
            IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource, PlCreditDebitNoteResource, PlCreditDebitNoteResource> service)
        {
            resource.Who ??= req.HttpContext.User.GetEmployeeNumber();

            var result = service.Add(
                resource,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(result);
        }
    }
}
