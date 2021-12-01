namespace Linn.Purchasing.Integration.Tests.PurchaseOrderReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }
        
        protected IPurchaseOrderReportFacadeService FacadeService { get; private set; }

        protected ILog Log { get; private set; }
        

        [SetUp]
        public void EstablishContext()
        {
            this.FacadeService = Substitute.For<IPurchaseOrderReportFacadeService>();
            this.Log = Substitute.For<ILog>();

            this.Client = TestClient.With<PurchaseOrdersReportModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
