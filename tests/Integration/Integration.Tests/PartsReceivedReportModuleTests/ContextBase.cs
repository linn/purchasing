namespace Linn.Purchasing.Integration.Tests.PartsReceivedReportModuleTests
{
    using System.Net.Http;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Service.Modules.Reports;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected ITqmsJobRefService FacadeService { get; private set; }

        protected HttpResponseMessage Response { get; set; }

        protected IRepository<TqmsJobref, string> MockRepository { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.MockRepository = Substitute.For<IRepository<TqmsJobref, string>>();
            this.FacadeService = new TqmsJobRefService(this.MockRepository);

            this.Client = TestClient.With<PartsReceivedReportModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
