namespace Linn.Purchasing.Integration.Tests.SupplierModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Persistence.LinnApps.Keys;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected
            IApplicationStateService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>
            PartSupplierFacadeService
        {
            get; private set;
        }

        protected ILog Log { get; private set; }

        protected IPartService PartFacadeService { get; private set; }

        protected IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource> SupplierFacadeService
        {
            get;
            private set;
        }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.PartSupplierFacadeService = 
                Substitute
                    .For<IApplicationStateService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>>();
            this.PartFacadeService = Substitute.For<IPartService>();
            this.Log = Substitute.For<ILog>();
            this.SupplierFacadeService =
                Substitute.For<IFacadeResourceService<Supplier, int, SupplierResource, SupplierResource>>();

            this.Client = TestClient.With<SupplierModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.PartSupplierFacadeService);
                        services.AddSingleton(this.Log);
                        services.AddSingleton(this.PartFacadeService);
                        services.AddSingleton(this.SupplierFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
