namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomReportsService Sut { get; private set; }

        protected IBomDetailViewRepository BomDetailViewRepository { get; private set; }

        protected IQueryRepository<Part> PartRepository { get; private set; }

        protected IQueryRepository<BomCostReportDetail> BomCostReportDetailsRepository { get; private set; }

        protected IBomTreeService BomTreeService { get; private set; }

        protected IRepository<CircuitBoard, string> CircuitBoardRepository { get; private set; }

        protected IQueryRepository<BomDetailComponent> BomDetailComponentRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomDetailViewRepository = Substitute.For<IBomDetailViewRepository>();
            this.BomTreeService = Substitute.For<IBomTreeService>();
            this.PartRepository = Substitute.For<IQueryRepository<Part>>();
            this.BomCostReportDetailsRepository = Substitute.For<IQueryRepository<BomCostReportDetail>>();
            this.CircuitBoardRepository = Substitute.For<IRepository<CircuitBoard, string>>();
            this.BomDetailComponentRepository = Substitute.For<IQueryRepository<BomDetailComponent>>();

            this.Sut = new BomReportsService(
                this.BomDetailViewRepository, 
                new ReportingHelper(), 
                this.BomTreeService,
                this.BomCostReportDetailsRepository,
                this.CircuitBoardRepository,
                this.PartRepository,
                this.BomDetailComponentRepository);
        }
    }
}
