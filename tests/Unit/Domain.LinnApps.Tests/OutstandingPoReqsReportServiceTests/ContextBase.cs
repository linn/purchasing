namespace Linn.Purchasing.Domain.LinnApps.Tests.OutstandingPoReqsReportServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IRepository<PurchaseOrderReq, int> MockRepository { get; private set; }

        protected IOutstandingPoReqsReportService Sut { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MockRepository = Substitute.For<IRepository<PurchaseOrderReq, int>>();
            this.Sut = new OutstandingPoReqsReportService(this.MockRepository, new ReportingHelper());
        }
    }
}
