namespace Linn.Purchasing.Domain.LinnApps.Tests.BomHistoryReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomHistoryReportService Sut { get; private set; }

        protected IRepository<Bom, int> BomRepository { get; private set; }

        protected IQueryRepository<BomHistoryViewEntry> BomHistoryViewEntryRepository { get; private set; }

        protected IRepository<BomDetail, int> DetailRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomRepository = Substitute.For<IRepository<Bom, int>>();
            this.BomHistoryViewEntryRepository = Substitute.For<IQueryRepository<BomHistoryViewEntry>>();
            this.DetailRepository = Substitute.For<IRepository<BomDetail, int>>();

            this.Sut = new BomHistoryReportService(
                this.BomHistoryViewEntryRepository,
                this.DetailRepository,
                this.BomRepository);
        }
    }
}
