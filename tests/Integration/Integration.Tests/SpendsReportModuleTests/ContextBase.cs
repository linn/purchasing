namespace Linn.Purchasing.Integration.Tests.SpendsReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Logging;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected ISpendsReportFacadeService FacadeService { get; private set; }

        protected ILog Log { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.FacadeService = Substitute.For<ISpendsReportFacadeService>();
            this.Log = Substitute.For<ILog>();

            this.Client = TestClient.With<SpendsReportModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
