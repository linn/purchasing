namespace Linn.Purchasing.Integration.Tests.PartsReceivedReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
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

        protected ITqmsJobRefService JobrefFacadeService { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        protected IRepository<TqmsJobref, string> MockRepository { get; private set; }

        protected IPartsReceivedReportFacadeService ReportFacadeService { get; set; }

        protected IPartsReceivedReportService MockDomainService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockRepository = Substitute.For<IRepository<TqmsJobref, string>>();
            this.JobrefFacadeService = new TqmsJobRefService(this.MockRepository);
            this.MockDomainService = Substitute.For<IPartsReceivedReportService>();
            this.ReportFacadeService = new PartsReceivedReportFacadeService(
                this.MockDomainService, new ResultsModelResourceBuilder());

            this.Client = TestClient.With<PartsReceivedReportModule>(
                services =>
                    {
                        services.AddSingleton(this.JobrefFacadeService);
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
