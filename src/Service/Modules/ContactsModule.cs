namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    using Microsoft.AspNetCore.Http;

    public class ContactsModule : CarterModule
    {
        private readonly IFacadeResourceService<Contact, int, ContactResource, ContactResource> contactService;

        public ContactsModule(IFacadeResourceService<Contact, int, ContactResource, ContactResource> contactService)
        {
            this.contactService = contactService;
            this.Get("/purchasing/suppliers/contacts", this.SearchContacts);
        }

        private async Task SearchContacts(HttpRequest req, HttpResponse res)
        {
            var search = req.Query.As<string>("searchTerm");
            var result = this.contactService.Search(search);

            await res.Negotiate(result);
        }
    }
}
