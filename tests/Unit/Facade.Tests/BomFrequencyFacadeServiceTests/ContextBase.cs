namespace Linn.Purchasing.Facade.Tests.BomFrequencyFacadeServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Facade.ResourceBuilders;
    using Linn.Purchasing.Facade.Services;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<BomFrequencyWeeks, string> Repository { get; private set; }

        protected ITransactionManager TransactionManager { get; private set; }

        protected IDatabaseService DatabaseService { get; private set; }

        protected BomFrequencyFacadeService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IRepository<BomFrequencyWeeks, string>>();
            this.TransactionManager = Substitute.For<ITransactionManager>();
            this.DatabaseService = Substitute.For<IDatabaseService>();

            this.Sut = new BomFrequencyFacadeService(
                this.Repository,
                this.TransactionManager,
                new BomFrequencyWeeksResourceBuilder(),
                this.DatabaseService);
        }
    }
}
