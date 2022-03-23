namespace Linn.Purchasing.Domain.LinnApps.Tests.PartsReceivedReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IQueryRepository<PartsReceivedViewModel> PartsReceivedView { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        protected IPartsReceivedReportService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.PartsReceivedView = Substitute.For<IQueryRepository<PartsReceivedViewModel>>();
            this.ReportingHelper = new ReportingHelper();
            this.Sut = new PartsReceivedReportService(this.PartsReceivedView, this.ReportingHelper);
        }
    }
}
