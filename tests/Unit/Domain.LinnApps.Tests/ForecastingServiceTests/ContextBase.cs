namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IForecastingService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IForecastingPack MockForecastingPack { get; private set; }

        protected IRepository<LinnWeek, int> MockLinnWeekRepository { get; private set; }

        protected IRepository<LedgerPeriod, int> MockLedgerPeriodRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockForecastingPack = Substitute.For<IForecastingPack>();
            this.MockLedgerPeriodRepository = Substitute.For<IRepository<LedgerPeriod, int>>();
            this.MockLinnWeekRepository = Substitute.For<IRepository<LinnWeek, int>>();
            this.Sut = new ForecastingService(
                this.MockForecastingPack, 
                this.MockAuthService, 
                this.MockLinnWeekRepository, 
                this.MockLedgerPeriodRepository);
        }
    }
}
