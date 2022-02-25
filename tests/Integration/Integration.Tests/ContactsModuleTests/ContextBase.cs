namespace Linn.Purchasing.Integration.Tests.ContactsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Persistence.LinnApps.Repositories;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<Contact, int, ContactResource, ContactResource> ContactService { get; private set; }

        protected IRepository<Contact, int> MockContactRepository { get; private set; }

        private ITransactionManager MockTransactionManager { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockContactRepository = Substitute.For<IRepository<Contact, int>>();
            this.MockTransactionManager = Substitute.For<ITransactionManager>();
            this.ContactService = new ContactsService(
                this.MockContactRepository,
                this.MockTransactionManager,
                new ContactResourceBuilder());
            this.Client = TestClient.With<ContactsModule>(
                services =>
                    {
                        services.AddSingleton(this.ContactService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
