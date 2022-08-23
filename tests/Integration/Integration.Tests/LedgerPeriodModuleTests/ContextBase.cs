namespace Linn.Purchasing.Integration.Tests.LedgerPeriodModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
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

        protected ITransactionManager TransactionManager { get; set; }

        protected IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource> LedgerPeriodFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();

            this.LedgerPeriodFacadeService = Substitute.For<IFacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource>>();

            this.Client = TestClient.With<LedgerPeriodModule>(
                services =>
                {
                    services.AddSingleton(this.TransactionManager);
                    services.AddSingleton(this.LedgerPeriodFacadeService);
                    services.AddHandlers();
                },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
