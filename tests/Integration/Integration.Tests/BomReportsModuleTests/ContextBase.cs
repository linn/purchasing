namespace Linn.Purchasing.Integration.Tests.BomReportsModuleTests
{
    using System.Net.Http;

    using Linn.Common.Reporting.Resources.ResourceBuilders;
    using Linn.Purchasing.Domain.LinnApps.Boms;
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

        [SetUp]
        public void EstablishContext()
        {
            this.DomainService = Substitute.For<IBomReportsService>();
            this.FacadeService = new BomReportsFacadeService(this.DomainService, new ReportReturnResourceBuilder());

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
