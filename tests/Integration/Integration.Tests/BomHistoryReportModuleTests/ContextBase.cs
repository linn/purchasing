namespace Linn.Purchasing.Integration.Tests.BomHistoryReportModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IBomHistoryReportFacadeService ReportFacadeService { get; set; }

        protected IBomHistoryReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockDomainService = Substitute.For<IBomHistoryReportService>();
            this.ReportFacadeService = new BomHistoryReportFacadeService(this.MockDomainService);

            this.Client = TestClient.With<BomHistoryReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
