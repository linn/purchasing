namespace Linn.Purchasing.Domain.LinnApps.Tests.ForecastOrdersReportServiceTests
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
        protected IQueryRepository<MonthlyForecastPart> MonthlyForecastPartsRepository { get; private set; }

        protected IQueryRepository<MonthlyForecastPartValues> MonthlyForecastRepository { get; private set; }

        protected IQueryRepository<ForecastReportMonth> ForecastReportMonthsRepository { get; private set; }

        protected IForecastOrdersReportService Sut { get; private set; }

        protected IEnumerable<ForecastReportMonth> MonthStrings { get; private set; }

        protected IEnumerable<MonthlyForecastPart> Parts { get; private set; }

        protected IEnumerable<MonthlyForecastPartValues> Values { get; private set; }

        [SetUp]
        public void SetUpContext()
        {
            this.MonthlyForecastPartsRepository = Substitute.For<IQueryRepository<MonthlyForecastPart>>();
            this.MonthlyForecastRepository = Substitute.For<IQueryRepository<MonthlyForecastPartValues>>();
            this.ForecastReportMonthsRepository = Substitute.For<IQueryRepository<ForecastReportMonth>>();
            var monthStrings = new List<string>()
                                   {
                                       "Aug22",
                                       "Sep22",
                                       "Oct22",
                                       "Nov22",
                                       "Dec22",
                                       "Jan23",
                                       "Feb23",
                                       "Mar23",
                                       "Apr23",
                                       "May23",
                                       "Jun23",
                                       "Jul23",
                                   };
            this.Parts = new List<MonthlyForecastPart>
                             {
                                 new MonthlyForecastPart
                                     { 
                                         MrPartNumber = "BPLINTH/2",
                                         SupplierDesignation = "LP12 PLINTH IN BLACK",
                                         UnitPrice = 133.74m,
                                         PreferredSupplier = 23348,
                                         MinimumOrderQty = 40,
                                         StartingQty = 50
                                     }
                             };

            this.Values = new List<MonthlyForecastPartValues>
                              {
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "2",
                                          Stock = "48",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "7",
                                          Stock = "41",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "13",
                                          Stock = "68",
                                          ForecastOrders = "0",
                                          Orders = "40"
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "16",
                                          Stock = "52",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "13",
                                          Stock = "39",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "12",
                                          Stock = "27",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "14",
                                          Stock = "13",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "16",
                                          Stock = "-3",
                                          ForecastOrders = "40",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "13",
                                          Stock = "-16",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "14",
                                          Stock = "-30",
                                          ForecastOrders = "40",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "11",
                                          Stock = "-41",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                                  new MonthlyForecastPartValues
                                      {
                                          PartNumber = "BPLINTH/2",
                                          Usages = "9",
                                          Stock = "-50",
                                          ForecastOrders = "0",
                                          Orders = null
                                      },
                              };

            this.MonthStrings = monthStrings.Select(s => new ForecastReportMonth { MmmYy = s });

            this.MonthlyForecastPartsRepository.FilterBy(Arg.Any<Expression<Func<MonthlyForecastPart, bool>>>())
                .Returns(this.Parts.AsQueryable());

            this.ForecastReportMonthsRepository.FindAll().Returns(this.MonthStrings.AsQueryable());

            this.MonthlyForecastRepository.FilterBy(Arg.Any<Expression<Func<MonthlyForecastPartValues, bool>>>())
                .Returns(this.Values.AsQueryable());

            this.Sut = new ForecastOrdersReportService(
                this.MonthlyForecastPartsRepository,
                this.MonthlyForecastRepository,
                this.ForecastReportMonthsRepository);
        }
    }
}
