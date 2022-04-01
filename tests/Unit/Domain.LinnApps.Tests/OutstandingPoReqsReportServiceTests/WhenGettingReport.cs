namespace Linn.Purchasing.Domain.LinnApps.Tests.OutstandingPoReqsReportServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingReport : ContextBase
    {
        private IQueryable<PurchaseOrderReq> dataResult;

        private ResultsModel results;

        private string stateParameter; 

        [SetUp]
        public void SetUp()
        {
            this.stateParameter = "STATE 2";
            this.dataResult = new List<PurchaseOrderReq>
                                  {
                                      new PurchaseOrderReq
                                          {
                                              ReqNumber = 4,
                                              ReqState = new PurchaseOrderReqState { State = "STATE 2", IsFinalState = "N" },
                                              ReqDate = DateTime.UnixEpoch,
                                              PartNumber = "PART 4",
                                              Description = "A description for req 4",
                                              SupplierId = 4,
                                              SupplierName = "SUPPLIER 4",
                                              Qty = 4,
                                              UnitPrice = 4,
                                              Carriage = 4,
                                              TotalReqPrice = 16
                                          },
                                      new PurchaseOrderReq
                                          {
                                            ReqNumber = 3,
                                            ReqState = new PurchaseOrderReqState { State = "STATE 3", IsFinalState = "N" },
                                            ReqDate = DateTime.UnixEpoch,
                                            PartNumber = "PART 3",
                                            Description = "A description for req 3",
                                            SupplierId = 3,
                                            SupplierName = "SUPPLIER 3",
                                            Qty = 3,
                                            UnitPrice = 3,
                                            Carriage = 3,
                                            TotalReqPrice = 9
                                          },
                                      new PurchaseOrderReq
                                          {
                                              ReqNumber = 2,
                                              ReqState = new PurchaseOrderReqState { State = "STATE 2", IsFinalState = "N" },
                                              ReqDate = DateTime.UnixEpoch,
                                              PartNumber = "PART 2",
                                              Description = "A description for req 2",
                                              SupplierId = 2,
                                              SupplierName = "SUPPLIER 2",
                                              Qty = 2,
                                              UnitPrice = 2,
                                              Carriage = 2,
                                              TotalReqPrice = 4
                                          },
                                      new PurchaseOrderReq
                                          {
                                              ReqNumber = 1,
                                              ReqState = new PurchaseOrderReqState { State = "STATE 1", IsFinalState = "N" },
                                              ReqDate = DateTime.UnixEpoch,
                                              PartNumber = "PART 1",
                                              Description = "A description for req 1",
                                              SupplierId = 1,
                                              SupplierName = "SUPPLIER 1",
                                              Qty = 1,
                                              UnitPrice = 1,
                                              Carriage = 1,
                                              TotalReqPrice = 1
                                          }
                                  }.AsQueryable();
            this.MockRepository.FilterBy(Arg.Any<Expression<Func<PurchaseOrderReq, bool>>>()).Returns(this.dataResult);

            this.results = this.Sut.GetReport(this.stateParameter);
        }

        [Test]
        public void ShouldReturnReport()
        {
            this.results.ReportTitle.DisplayValue.Should().Be(
                $"{this.stateParameter} Outstanding PO Reqs");
        }

        [Test]
        public void ShouldFilterByState()
        {
            this.results.Rows.Count().Should().Be(2);
            this.results.GetGridTextValue(0, 0).Should().Be(this.stateParameter);
            this.results.GetGridTextValue(1, 0).Should().Be(this.stateParameter);
        }

        [Test]
        public void ShouldOrderByReqNumber()
        {
            for (var i = 1; i < this.results.Rows.Count(); i++)
            {
                var prevNo = this.results.Rows.ElementAt(i - 1).RowId;
                Assert.IsTrue(string.CompareOrdinal(this.results.Rows.ElementAt(i).RowId, prevNo) > 0);
            }
        }
    }
}
