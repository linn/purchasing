namespace Linn.Purchasing.Integration.Tests.BomReportsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Pdf;
    using Linn.Common.Rendering;
    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected IBomReportsFacadeService FacadeService { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        protected IBomReportsService DomainService { get; private set; }

        protected IHtmlTemplateService<BomCostReports> BomCostReportsHtmlService
        {
            get; private set;
        }

        protected IPdfService PdfService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.DomainService = Substitute.For<IBomReportsService>();
            this.PdfService = Substitute.For<IPdfService>();
            this.BomCostReportsHtmlService = Substitute.For<IHtmlTemplateService<BomCostReports>>();
            this.FacadeService = new BomReportsFacadeService(
                this.DomainService, 
                new ReportReturnResourceBuilder(),
                this.BomCostReportsHtmlService,
                this.PdfService);

            this.Client = TestClient.With<BomReportsModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
