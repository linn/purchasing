namespace Linn.Purchasing.Integration.Tests.ForecastingModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Forecasting;
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

        protected IForecastingService MockDomainService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockDomainService = Substitute.For<IForecastingService>();
            this.FacadeService = new ForecastingFacadeService(this.MockDomainService);

            this.Client = TestClient.With<ForecastingModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
