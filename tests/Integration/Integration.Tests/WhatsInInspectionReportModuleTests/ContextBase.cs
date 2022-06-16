namespace Linn.Purchasing.Integration.Tests.WhatsInInspectionReportModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IWhatsInInspectionReportFacadeService ReportFacadeService { get; set; }

        protected IWhatsInInspectionReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockDomainService = Substitute.For<IWhatsInInspectionReportService>();
            this.ReportFacadeService = new WhatsInInspectionReportFacadeService(
                this.MockDomainService, new ReportReturnResourceBuilder());

            this.Client = TestClient.With<WhatsInInspectionReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
