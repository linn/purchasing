namespace Linn.Purchasing.Domain.LinnApps.Tests.BomReportsServiceTests
{
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Boms;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IBomReportsService Sut { get; private set; }

        protected IBomDetailRepository BomDetailRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.BomDetailRepository = Substitute.For<IBomDetailRepository>();
            this.Sut = new BomReportsService(this.BomDetailRepository, new ReportingHelper());
        }
    }
}
