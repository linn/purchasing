namespace Linn.Purchasing.Domain.LinnApps.Tests.PrefSupReceiptsReportTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IPrefSupReceiptsReportService Sut { get; private set; }

        protected IQueryRepository<ReceiptPrefSupDiff> Repository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            var reportingHelper = new ReportingHelper(); // not sure why we'd mock this

            this.Repository = Substitute.For<IQueryRepository<ReceiptPrefSupDiff>>();

            this.Sut = new PrefSupReceiptsReportService(reportingHelper, this.Repository);
        }
    }
}
