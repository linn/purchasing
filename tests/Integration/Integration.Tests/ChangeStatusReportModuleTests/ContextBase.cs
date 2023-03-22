namespace Linn.Purchasing.Integration.Tests.ChangeStatusReportModuleTests
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

        protected IChangeStatusReportsFacadeService FacadeService { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        protected IChangeStatusReportService DomainService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.DomainService = Substitute.For<IChangeStatusReportService>();
            this.FacadeService = new ChangeStatusReportFacadeService(this.DomainService, new ReportReturnResourceBuilder());

            this.Client = TestClient.With<ChangeStatusReportModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
