namespace Linn.Purchasing.Integration.Tests.BomStandardPriceModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Boms;
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

        protected IBomStandardPriceService DomainService { get; set; }

        protected IBomStandardPriceFacadeService FacadeService { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.DomainService = Substitute.For<IBomStandardPriceService>();
            this.FacadeService = new BomStandardPriceFacadeService(this.DomainService);
            this.Client = TestClient.With<BomStandardPriceModule>(
                services =>
                {
                    services.AddSingleton(this.FacadeService);
                    services.AddHandlers();
                    services.AddRouting();
                },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
