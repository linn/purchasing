namespace Linn.Purchasing.Integration.Tests.BomVerificationHistoryModuleTests
{
    using System.Net.Http;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
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

        protected IRepository<BomVerificationHistory, int> BomVerificationHistoryRepository { get; set; }

        [SetUp]
        public void EstablishContext()
        {
            this.BomVerificationHistoryFacadeService = Substitute.For<IFacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource>>();
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
