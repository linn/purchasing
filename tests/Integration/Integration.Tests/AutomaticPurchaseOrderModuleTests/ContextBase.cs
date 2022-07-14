namespace Linn.Purchasing.Integration.Tests.AutomaticPurchaseOrderModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource> FacadeService { get; private set; }
        
        protected IFacadeResourceFilterService<AutomaticPurchaseOrderSuggestion, int, AutomaticPurchaseOrderSuggestionResource, AutomaticPurchaseOrderSuggestionResource, PlannerSupplierRequestResource> SuggestionFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.FacadeService = Substitute.For<IFacadeResourceService<AutomaticPurchaseOrder, int, AutomaticPurchaseOrderResource, AutomaticPurchaseOrderResource>>();
            this.SuggestionFacadeService = Substitute.For<IFacadeResourceFilterService<AutomaticPurchaseOrderSuggestion, int, AutomaticPurchaseOrderSuggestionResource, AutomaticPurchaseOrderSuggestionResource, PlannerSupplierRequestResource>>();

            this.Client = TestClient.With<AutomaticPurchaseOrderModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.SuggestionFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
