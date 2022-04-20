namespace Linn.Purchasing.Integration.Tests.PrefSupReceiptsReportModuleTests
{
    using System.Net.Http;

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

        protected IPrefSupReceiptsReportFacadeService ReportFacadeService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.ReportFacadeService = Substitute.For<IPrefSupReceiptsReportFacadeService>();

            this.Client = TestClient.With<PrefSupReceiptsReportModule>(
                services =>
                    {
                        services.AddSingleton(this.ReportFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
