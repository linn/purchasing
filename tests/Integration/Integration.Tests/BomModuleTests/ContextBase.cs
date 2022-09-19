namespace Linn.Purchasing.Integration.Tests.BomModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
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

        protected IFacadeResourceService<Bom, int, BomResource, BomResource> FacadeService { get; set; }

        protected IRepository<Bom, int> Repository { get; set; }

        protected IEdiOrderService MockDomainService { get; set; }

        protected ITransactionManager TransactionManager { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<Bom, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.FacadeService = new BomFacadeService(
                this.Repository,
                this.TransactionManager,
                new BomResourceBuilder());

            this.Client = TestClient.With<BomModule>(
                services =>
                    {
                        services.AddSingleton(this.FacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
