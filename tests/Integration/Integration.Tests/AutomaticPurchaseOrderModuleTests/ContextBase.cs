namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> FacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.FacadeService = Substitute.For<IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource>>();

            this.Client = TestClient.With<AutomaticPurchaseOrderModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
