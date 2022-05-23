namespace Linn.Purchasing.Domain.LinnApps.Tests.ShortagesReportTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IShortagesReportService Sut { get; private set; }

        protected IQueryRepository<ShortagesEntry> ShortagesRepository { get; private set; }

        protected IEnumerable<ShortagesEntry> Data { get; private set; }

        protected IReportingHelper ReportingHelper { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Data = new List<ShortagesEntry>
                            {
                                new ShortagesEntry()
                                    {
                                        VendorManagerName = "Test",
                                        PartNumber = "Test Part",
                                        PlannerName = "Test Planner",
                                        VendorManagerCode = "R",
                                        Planner = 1,
                                        PurchaseLevel = 1
                                    },
                                new ShortagesEntry()
                                    {
                                        VendorManagerName = "Test",
                                        PartNumber = "Second Test Part",
                                        PlannerName = "Second Test Planner",
                                        VendorManagerCode = "S",
                                        Planner = 1,
                                        PurchaseLevel = 1
                                    },
                                new ShortagesEntry()
                                    {
                                        VendorManagerName = "Test",
                                        PartNumber = "Third Test Part",
                                        PlannerName = "Test Planner",
                                        VendorManagerCode = "T",
                                        Planner = 1,
                                        PurchaseLevel = 1
                                    },
                                new ShortagesEntry()
                                    {
                                        VendorManagerName = "Test",
                                        PartNumber = "Fourth Test Part",
                                        PlannerName = "Test Planner",
                                        VendorManagerCode = "T",
                                        Planner = 1,
                                        PurchaseLevel = 5
                                    }
                            };
            this.ShortagesRepository = Substitute.For<IQueryRepository<ShortagesEntry>>();
            this.ShortagesRepository.FilterBy(Arg.Any<Expression<Func<ShortagesEntry, bool>>>())
                .Returns(this.Data.AsQueryable());

            var reportingHelper = new ReportingHelper();
            this.Sut = new ShortagesReportService(this.ShortagesRepository);
        }
    }
}
