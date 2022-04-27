namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastingServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IForecastingService Sut { get; private set; }

        protected IAuthorisationService MockAuthService { get; private set; }

        protected IForecastingPack MockForecastingPack { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockAuthService = Substitute.For<IAuthorisationService>();
            this.MockForecastingPack = Substitute.For<IForecastingPack>();
            this.Sut = new ForecastingService(this.MockForecastingPack, this.MockAuthService);
        }
    }
}
