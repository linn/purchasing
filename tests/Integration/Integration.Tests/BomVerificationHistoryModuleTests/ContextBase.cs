namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Net.Http;
    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.IoC;
    using Linn.Purchasing.Resources.Boms;
    using Linn.Purchasing.Service.Modules;

    using Microsoft.Extensions.DependencyInjection;

    using NSubstitute;
    using NUnit.Framework;

    public abstract class ContextBase
    {
        protected HttpClient Client { get; set; }

        protected HttpResponseMessage Response { get; set; }

        protected IFacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource> BomVerificationHistoryFacadeService { get; private set; }

        protected IRepository<BomVerificationHistory, int> BomVerificationHistoryRepository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected IAuthorisationService AuthorisationService { get; private set; }

        [SetUp]
        public void EstablishContext()
        {
            this.BomVerificationHistoryRepository = Substitute.For<IRepository<BomVerificationHistory, int>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.DatabaseService= Substitute.For<IDatabaseService>();

            this.BomVerificationHistoryFacadeService = new BomVerificationHistoryFacadeService(this.BomVerificationHistoryRepository, 
                                                                                               this.TransactionManager, 
                                                                                               new BomVerificationHistoryResourceBuilder(),
                                                                                               this.DatabaseService);
            this.Client = TestClient.With<BomVerificationHistoryModule>(
                services =>
                    {
                        services.AddSingleton(this.BomVerificationHistoryFacadeService);
                        services.AddHandlers();
                    },
                FakeAuthMiddleware.EmployeeMiddleware);
        }
    }
}
