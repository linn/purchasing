namespace Linn.Purchasing.Domain.LinnApps.Tests.MrOrderBookReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IMrOrderBookReportService Sut { get; private set; }

        protected IQueryRepository<MrPurchaseOrderDetail> Repository { get; private set; }

        protected ISingleRecordRepository<MrMaster> MrMaster { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Repository = Substitute.For<IQueryRepository<MrPurchaseOrderDetail>>();
            this.MrMaster = Substitute.For<ISingleRecordRepository<MrMaster>>();

            this.Sut = new MrOrderBookReportService(
                this.Repository, this.MrMaster, new ReportingHelper());
        }
    }
}
