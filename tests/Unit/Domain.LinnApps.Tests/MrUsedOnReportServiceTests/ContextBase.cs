namespace Linn.Purchasing.Domain.LinnApps.Tests.MrUsedOnReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<MrUsedOnRecord> MockRepository { get; private set; }

        protected ISingleRecordRepository<MrMaster> MockMrMasterRecordRepository { get; private set; }

        protected IMrUsedOnReportService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockRepository = Substitute.For<IQueryRepository<MrUsedOnRecord>>();
            this.MockMrMasterRecordRepository = Substitute.For<ISingleRecordRepository<MrMaster>>();
            this.Sut = new MrUsedOnReportService(
                this.MockRepository,
                this.MockMrMasterRecordRepository,
                new ReportingHelper());
        }
    }
}
