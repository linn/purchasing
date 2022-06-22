namespace Linn.Purchasing.Integration.Tests.MrOrderBookReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Reporting.Resources.ResourceBuilders;
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

        protected IMrOrderBookReportFacadeService ReportFacadeService { get; set; }

        protected IMrOrderBookReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockDomainService = Substitute.For<IMrOrderBookReportService>();
            this.ReportFacadeService = new MrOrderBookReportFacadeService(
                this.MockDomainService, new ReportReturnResourceBuilder());

            this.Client = TestClient.With<MrOrderBookReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
