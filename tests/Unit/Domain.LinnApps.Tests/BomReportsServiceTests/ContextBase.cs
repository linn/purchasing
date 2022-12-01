namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomReportsService Sut { get; private set; }

        protected IBomDetailRepository BomDetailRepository { get; private set; }


        protected IQueryRepository<BomCostReportDetail> BomCostReportDetailsRepository { get; private set; }

        protected IBomTreeService BomTreeService { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomDetailRepository = Substitute.For<IBomDetailRepository>();
            this.BomTreeService = Substitute.For<IBomTreeService>();
            this.BomCostReportDetailsRepository = Substitute.For<IQueryRepository<BomCostReportDetail>>();
            this.Sut = new BomReportsService(
                this.BomDetailRepository, 
                new ReportingHelper(), 
                this.BomTreeService,
                this.BomCostReportDetailsRepository);
        }
    }
}
