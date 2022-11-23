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

        protected IQueryRepository<BoardComponentSummary> ComponentSummaryRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomDetailRepository = Substitute.For<IBomDetailRepository>();
            this.ComponentSummaryRepository = Substitute.For<IQueryRepository<BoardComponentSummary>>();

            this.Sut = new BomReportsService(
                this.BomDetailRepository, new ReportingHelper(), this.ComponentSummaryRepository);
        }
    }
}
