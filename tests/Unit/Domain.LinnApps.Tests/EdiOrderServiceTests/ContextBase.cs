namespace Linn.Purchasing.Domain.LinnApps.Tests.EdiOrderServiceTests
{
    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Edi;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Forecasting;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IEdiOrderService Sut { get; private set; }

        protected IEdiEmailPack MockEdiEmailPack { get; private set; }

        protected IRepository<EdiOrder, int> MockEdiOrderRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockEdiEmailPack = Substitute.For<IEdiEmailPack>();
            this.MockEdiOrderRepository = Substitute.For<IRepository<EdiOrder, int>>();
            this.Sut = new EdiOrderService(
                this.MockEdiEmailPack,
                this.MockEdiOrderRepository);
        }
    }

}
