namespace Linn.Purchasing.Integration.Tests.EdiModuleTests
{
    using System.Net.Http;

    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Facade.ResourceBuilders;
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

        protected IEdiOrdersFacadeService FacadeService { get; set; }

        protected IEdiOrderService MockDomainService { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockDomainService = Substitute.For<IEdiOrderService>();
            this.FacadeService = new EdiOrdersFacadeService(this.MockDomainService, new EdiOrderResourceBuilder());

            this.Client = TestClient.With<EdiModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
