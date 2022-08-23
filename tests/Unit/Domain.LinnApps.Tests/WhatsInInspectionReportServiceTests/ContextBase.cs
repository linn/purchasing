namespace Linn.Purchasing.Domain.LinnApps.Tests.WhatsInInspectionReportServiceTests
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
        protected WhatsInInspectionReportService Sut { get; private set; }

        protected IWhatsInInspectionRepository WhatsInInspectionRepository { get; private set; }

        protected IQueryRepository<WhatsInInspectionPurchaseOrdersData> 
            WhatsInInspectionPurchaseOrdersDataRepository
        {
            get; private set;
        }

        protected IQueryRepository<WhatsInInspectionStockLocationsData> 
            WhatsInInspectionStockLocationsDataRepository
        {
            get; private set;
        }

        protected IQueryRepository<WhatsInInspectionBackOrderData> 
            WhatsInInspectionBackOrderDataRepository
        {
            get; private set;
        }

        protected IReportingHelper ReportingHelper
        {
            get; private set;
        }

        protected IEnumerable<PartsInInspection> PartsInInspections { get; private set; }

        protected IEnumerable<WhatsInInspectionPurchaseOrdersData> OrdersData { get; private set; }

        protected IEnumerable<WhatsInInspectionStockLocationsData> LocationsData { get; private set; }

        protected IEnumerable<WhatsInInspectionBackOrderData> BackOrderData { get; private set; }

        protected IQueryRepository<StockLocator> StockLocatorRepository { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.ReportingHelper = new ReportingHelper();
            this.WhatsInInspectionRepository = Substitute.For<IWhatsInInspectionRepository>();
            this.WhatsInInspectionPurchaseOrdersDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionPurchaseOrdersData>>();
            this.WhatsInInspectionStockLocationsDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionStockLocationsData>>();
            this.WhatsInInspectionBackOrderDataRepository =
                Substitute.For<IQueryRepository<WhatsInInspectionBackOrderData>>();
            this.StockLocatorRepository = Substitute.For<IQueryRepository<StockLocator>>();

            this.PartsInInspections = new List<PartsInInspection>
                            {
                                new PartsInInspection
                                    {
                                        PartNumber = "PART A",
                                        Description = "DESC",
                                        OurUnitOfMeasure = "ONES",
                                        QtyInStock = 1,
                                        QtyInInspection = 1,
                                        RawOrFinished = "F",
                                        MinDate = new DateTime(1995, 3, 28)
                                    },
                                new PartsInInspection
                                    {
                                        PartNumber = "PART B",
                                        Description = "DESC",
                                        OurUnitOfMeasure = "ONES",
                                        QtyInStock = 1,
                                        QtyInInspection = 1,
                                        RawOrFinished = "F",
                                        MinDate = new DateTime(1993, 3, 28)
                                    },
                                new PartsInInspection
                                    {
                                        PartNumber = "PART C",
                                        Description = "DESC",
                                        OurUnitOfMeasure = "ONES",
                                        QtyInStock = 1,
                                        QtyInInspection = 1,
                                        RawOrFinished = "R",
                                        MinDate = new DateTime(1999, 3, 28)
                                    },
                                new PartsInInspection
                                    {
                                        PartNumber = "PART D",
                                        Description = "DESC",
                                        OurUnitOfMeasure = "ONES",
                                        QtyInStock = 1,
                                        QtyInInspection = 1,
                                        RawOrFinished = "R",
                                        MinDate = new DateTime(2007, 3, 28)
                                    }
                            };

            this.OrdersData = new List<WhatsInInspectionPurchaseOrdersData>
                                  {
                                      new WhatsInInspectionPurchaseOrdersData
                                          {
                                              PartNumber = "PART A",
                                              State = "QC"
                                          },
                                      new WhatsInInspectionPurchaseOrdersData
                                          {
                                              PartNumber = "PART B",
                                              State = "QC"
                                          },
                                      new WhatsInInspectionPurchaseOrdersData
                                          {
                                              PartNumber = "PART C",
                                              State = "QC"
                                          },
                                  };

            this.LocationsData = new List<WhatsInInspectionStockLocationsData>
                                     {
                                         new WhatsInInspectionStockLocationsData
                                             {
                                                 PartNumber = "PART A", 
                                                 State = "QC", 
                                                 Batch = "BA", 
                                                 Location = "LA",
                                                 Qty = 1
                                             },
                                         new WhatsInInspectionStockLocationsData
                                             {
                                                 PartNumber = "PART B",
                                                 State = "QC",
                                                 Batch = "BB",
                                                 Location = "LB",
                                                 Qty = 1
                                             },
                                         new WhatsInInspectionStockLocationsData
                                             {
                                                 PartNumber = "PART C",
                                                 State = "QC",
                                                 Batch = "BC",
                                                 Location = "LC",
                                                 Qty = 1
                                             },
                                         new WhatsInInspectionStockLocationsData
                                             {
                                                 PartNumber = "PART D",
                                                 State = "QC",
                                                 Batch = "BD",
                                                 Location = "LD",
                                                 Qty = 1
                                             }
                                     };

            this.BackOrderData = new List<WhatsInInspectionBackOrderData>
                                     {
                                         new WhatsInInspectionBackOrderData
                                             {
                                                 ArticleNumber = "PART A"
                                             },
                                         new WhatsInInspectionBackOrderData
                                             {
                                                 ArticleNumber = "PART B"
                                             },
                                         new WhatsInInspectionBackOrderData
                                             {
                                                 ArticleNumber = "PART C"
                                             },
                                         new WhatsInInspectionBackOrderData
                                             {
                                                 ArticleNumber = "PART D"
                                             }
                                     };

            this.WhatsInInspectionRepository
                .GetWhatsInInspection(Arg.Any<bool>())
                .Returns(this.PartsInInspections.AsQueryable());

            this.WhatsInInspectionPurchaseOrdersDataRepository
                .FilterBy(Arg.Any<Expression<Func<WhatsInInspectionPurchaseOrdersData, bool>>>())
                .Returns(this.OrdersData.AsQueryable());

            this.WhatsInInspectionStockLocationsDataRepository
                .FilterBy(Arg.Any<Expression<Func<WhatsInInspectionStockLocationsData, bool>>>())
                .Returns(this.LocationsData.AsQueryable());

            this.WhatsInInspectionBackOrderDataRepository
                .FindAll()
                .Returns(this.BackOrderData.AsQueryable());

            this.Sut = new WhatsInInspectionReportService(
                this.WhatsInInspectionRepository,
                this.WhatsInInspectionPurchaseOrdersDataRepository,
                this.WhatsInInspectionStockLocationsDataRepository,
                this.WhatsInInspectionBackOrderDataRepository,
                this.StockLocatorRepository,
                this.ReportingHelper);
        }
    }
}
