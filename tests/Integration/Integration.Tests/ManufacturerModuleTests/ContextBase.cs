namespace Linn.Purchasing.Integration.Tests.ManufacturerModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
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

        protected ILog Log { get; private set; }

        protected IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource> ManufacturerFacadeService
        {
            get;
            private set;
        }

        [SetUp]
        public void EstablishContext()
        {
            this.TransactionManager = Substitute.For<ITransactionManager>();

            this.Log = Substitute.For<ILog>();
            this.ManufacturerFacadeService =
                Substitute.For<IFacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource>>();

            this.Client = TestClient.With<ManufacturerModule>(
                services =>
                {
                    services.AddSingleton(this.TransactionManager);
                    services.AddSingleton(this.Log);
                    services.AddSingleton(this.ManufacturerFacadeService);
                    services.AddHandlers();
                },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
