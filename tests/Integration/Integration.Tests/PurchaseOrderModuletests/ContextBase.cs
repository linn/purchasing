namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource> FacadeService { get; private set; }

        protected ILog Log { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.FacadeService = Substitute.For<IFacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>>();
            this.Log = Substitute.For<ILog>();

            this.Client = TestClient.With<PurchaseOrderModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.Log);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
