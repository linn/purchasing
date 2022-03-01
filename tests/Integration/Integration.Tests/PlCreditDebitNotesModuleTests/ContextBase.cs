namespace Linn.Purchasing.Integration.Tests.PlCreditDebitNotesModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager MockTransactionManager { get; private set; }

        protected IRepository<PlCreditDebitNote, int> MockPlCreditDebitNoteRepository { get; private set; }

        protected IPlCreditDebitNoteService MockDomainService { get; private set; }

        protected IFacadeResourceFilterService<PlCreditDebitNote, int, PlCreditDebitNoteResource,
            PlCreditDebitNoteResource, PlCreditDebitNoteResource> FacadeService
        {
            get; private set;
        }

        [SetUp]
        public void EstablishContext()
        {
            this.MockPlCreditDebitNoteRepository = Substitute.For<IRepository<PlCreditDebitNote, int>>();
            this.MockTransactionManager = Substitute.For<ITransactionManager>();
            this.MockDomainService = Substitute.For<IPlCreditDebitNoteService>();
            this.FacadeService = new PlCreditDebitNoteFacadeService(
                this.MockPlCreditDebitNoteRepository,
                this.MockTransactionManager,
                new PlCreditDebitNoteResourceBuilder(),
                this.MockDomainService);

            this.Client = TestClient.With<PlCreditDebitNotesModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
