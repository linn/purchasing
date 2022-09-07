namespace Linn.Purchasing.Domain.LinnApps.Tests.ShortagesPlannerReportTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Reports;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    using NSubstitute;

    using NUnit.Framework;

    public class ContextBase
    {
        protected IShortagesReportService Sut { get; private set; }

        protected IQueryRepository<ShortagesEntry> ShortagesRepository { get; private set; }

        protected IQueryRepository<ShortagesPlannerEntry> ShortagesPlannerRepository { get; private set; }

        protected IEnumerable<ShortagesPlannerEntry> Data { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ShortagesRepository = Substitute.For<IQueryRepository<ShortagesEntry>>();

            this.Data = new List<ShortagesPlannerEntry>
                            {
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM T",
                                        PartNumber = "Fourth Test Part",
                                        VendorManagerCode = "T",
                                        Planner = 1,
                                        PurchaseLevel = "3",
                                        OrderNumber = 456809,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM L",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "L",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 12345,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM L",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "L",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 4567,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM M",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "M",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 89012,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM M",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "M",
                                        Planner = 1,
                                        PurchaseLevel = "2",
                                        OrderNumber = 21345,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM M",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "M",
                                        Planner = 1,
                                        PurchaseLevel = "2",
                                        OrderNumber = 87453,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM L",
                                        PartNumber = "Test Part",
                                        VendorManagerCode = "L",
                                        Planner = 1,
                                        PurchaseLevel = "2",
                                        OrderNumber = 45367,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM L",
                                        PartNumber = "Second Test Part",
                                        VendorManagerCode = "L",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 3453412,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM L",
                                        PartNumber = "Second Test Part",
                                        VendorManagerCode = "L",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 2135678,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM T",
                                        PartNumber = "Second Test Part",
                                        VendorManagerCode = "T",
                                        Planner = 1,
                                        PurchaseLevel = "1",
                                        OrderNumber = 786534,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    },
                                new ShortagesPlannerEntry
                                    {
                                        VendorManagerName = "Test VM T",
                                        PartNumber = "Third Test Part",
                                        VendorManagerCode = "T",
                                        Planner = 1,
                                        PurchaseLevel = "3",
                                        OrderNumber = 89764,
                                        OrderLine = 1,
                                        DeliverySeq = 1
                                    }
                            };
            this.ShortagesPlannerRepository = Substitute.For<IQueryRepository<ShortagesPlannerEntry>>();
            this.ShortagesPlannerRepository.FilterBy(Arg.Any<Expression<Func<ShortagesPlannerEntry, bool>>>())
                .Returns(this.Data.AsQueryable());

            this.Sut = new ShortagesReportService(this.ShortagesRepository, this.ShortagesPlannerRepository);
        }
    }
}
