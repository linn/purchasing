namespace Linn.Purchasing.Integration.Tests.SupplierGroupModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
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

        protected IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource> SupplierGroupFacadeService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.SupplierGroupFacadeService = Substitute.For<IFacadeResourceService<SupplierGroup, int, SupplierGroupResource, SupplierGroupResource>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();

            this.Log = Substitute.For<ILog>();
            this.Client = TestClient.With<SupplierGroupModule>(
                services =>
                    {
                        services.AddSingleton(this.TransactionManager);
                        services.AddSingleton(this.Log);
                        services.AddSingleton(this.SupplierGroupFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }

        public ILog Log { get; set; }

        public ITransactionManager TransactionManager { get; set; }
    }
}
