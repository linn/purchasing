namespace Linn.Purchasing.Facade.Tests.ChangeRequestFacadeServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<ChangeRequest, int> repository;

        protected ITransactionManager transactionManager;

        protected IAuthorisationService authorisationService;

        protected IDatabaseService databaseService;

        protected ChangeRequestFacadeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.repository = Substitute.For<IRepository<ChangeRequest, int>>();
            this.transactionManager = Substitute.For<ITransactionManager>();
            this.authorisationService = Substitute.For<IAuthorisationService>();
            this.databaseService = Substitute.For<IDatabaseService>();

            this.Sut = new ChangeRequestFacadeService(
                this.repository,
                this.transactionManager,
                new ChangeRequestResourceBuilder(
                    new BomChangeResourceBuilder(),
                    new PcasChangeResourceBuilder(),
                    this.authorisationService),
                new ChangeRequestService(this.authorisationService, this.repository),
                this.databaseService);
        }
    }
}
