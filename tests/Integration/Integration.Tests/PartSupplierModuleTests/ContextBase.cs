namespace Linn.Purchasing.Integration.Tests.PartSupplierModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
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
            IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource> 
            FacadeService
        {
            get; private set;
        }

        protected ILog Log { get; private set; }

        protected IPartService PartFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.FacadeService = 
                Substitute
                    .For<IFacadeResourceFilterService<PartSupplier, PartSupplierKey, PartSupplierResource, PartSupplierResource, PartSupplierSearchResource>>();
            this.PartFacadeService = Substitute.For<IPartService>();
            this.Log = Substitute.For<ILog>();

            this.Client = TestClient.With<PartSupplierModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.FacadeService);
                        services.AddSingleton(this.Log);
                        services.AddSingleton(this.PartFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
