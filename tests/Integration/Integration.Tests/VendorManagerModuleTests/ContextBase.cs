namespace Linn.Purchasing.Integration.Tests.VendorManagerModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        protected IRepository<VendorManager, string> VendorManagerRepository { get; private set; }

        protected ILog Log { get; private set; }

        protected IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource> VendorManagerFacadeService
        {
            get;
            private set;
        }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.VendorManagerRepository = Substitute.For<IRepository<VendorManager, string>>();
            this.Log = Substitute.For<ILog>();
            this.VendorManagerFacadeService =
                Substitute.For<IFacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource>>();

            this.Client = TestClient.With<VendorManagerModule>(
                services =>
                {
                    services.AddSingleton(this.TransactionManager);
                    services.AddSingleton(this.Log);
                    services.AddSingleton(this.VendorManagerFacadeService);
                    services.AddHandlers();
                },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
