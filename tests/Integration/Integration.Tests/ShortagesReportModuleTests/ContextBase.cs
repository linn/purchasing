namespace Linn.Purchasing.Integration.Tests.ShortagesReportModuleTests
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

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IShortagesReportFacadeService ReportFacadeService { get; set; }

        protected IShortagesReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockDomainService = Substitute.For<IShortagesReportService>();
            this.ReportFacadeService = new ShortagesReportFacadeService(
                this.MockDomainService, new ReportReturnResourceBuilder());

            this.Client = TestClient.With<ShortagesReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
