namespace Linn.Purchasing.Integration.Tests.PurchaseOrderDeliveryModuleTests
{
    using System.Net.Http;

    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager MockTransactionManager { get; private set; }

        protected IPurchaseOrderDeliveryFacadeService FacadeService { get; private set; }

        protected IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey> MockRepository { get; private set; }

        protected IPurchaseOrderDeliveryService MockDomainService { get; private set; }

        protected ILog MockLog { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockTransactionManager = Substitute.For<ITransactionManager>();
            this.MockRepository = Substitute.For<IRepository<PurchaseOrderDelivery, PurchaseOrderDeliveryKey>>();
            this.MockDomainService = Substitute.For<IPurchaseOrderDeliveryService>();
            this.MockLog = Substitute.For<ILog>();

            this.FacadeService = new PurchaseOrderDeliveryFacadeService(
                this.MockRepository,
                new PurchaseOrderDeliveryResourceBuilder(),
                this.MockDomainService,
                this.MockTransactionManager);

            this.Client = TestClient.With<PurchaseOrderDeliveryModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.MockLog);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
