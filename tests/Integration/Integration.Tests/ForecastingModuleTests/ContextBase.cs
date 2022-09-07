namespace Linn.Purchasing.Integration.Tests.ForecastingModuleTests
{
    using System.Net.Http;

    using Linn.Common.Reporting.Models;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;
    using Linn.Purchasing.Domain.LinnApps.Reports;
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

        protected IForecastingFacadeService FacadeService { get; set; }

        protected IForecastWeekChangesFacadeService ReportFacadeService { get; set; }

        protected IForecastingService MockDomainService { get; set; }

        protected IForecastWeekChangesReportService MockReportService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockDomainService = Substitute.For<IForecastingService>();
            this.FacadeService = new ForecastingFacadeService(this.MockDomainService);
            this.MockReportService = Substitute.For<IForecastWeekChangesReportService>();

            this.ReportFacadeService = new ForecastWeekChangesFacadeService(
                this.MockReportService,
                new ReportReturnResourceBuilder());

            this.MockReportService.GetReport()
                .Returns(new ResultsModel 
                             { 
                                 ReportTitle = new NameModel("Forecast % Change Report")
                             });

            this.Client = TestClient.With<ForecastingModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
