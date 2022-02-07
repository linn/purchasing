namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenTakingSupplierOffHold : AuthorisedContext
    {
        private Supplier result;

        private SupplierOrderHoldHistoryEntry data;

        private SupplierOrderHoldHistoryEntry onHoldHistoryEntry;


        [SetUp]
        public void SetUp()
        {
            this.data = new SupplierOrderHoldHistoryEntry
                            {
                                SupplierId = 1,
                                TakenOffHoldBy = 33087,
                                ReasonOffHold = "ALL GOOD"
                            };

            this.onHoldHistoryEntry = new SupplierOrderHoldHistoryEntry
                                          {
                                              SupplierId = 1,
                                              PutOnHoldBy = 33087,
                                              ReasonOnHold = "SOME REASON"
                                          };

            this.MockSupplierRepository.FindById(1).Returns(new Supplier { SupplierId = 1, OrderHold = "N" });

            this.MockSupplierOrderHoldHistory.FilterBy(
                    Arg.Any<Expression<Func<SupplierOrderHoldHistoryEntry, bool>>>())
                .Returns(new List<SupplierOrderHoldHistoryEntry> { this.onHoldHistoryEntry }.AsQueryable());

            this.result = this.Sut.ChangeSupplierHoldStatus(this.data, new List<string> { AuthorisedAction.SupplierHoldChange });
        }

        [Test]
        public void ShouldUpdateHoldStatus()
        {
            this.result.OrderHold.Should().Be("N");
        }

        [Test]
        public void ShouldUpdateHistoryEntry()
        {
            this.onHoldHistoryEntry.DateOffHold.Should().NotBeNull();
            this.onHoldHistoryEntry.ReasonOffHold.Should().Be("ALL GOOD");
            this.onHoldHistoryEntry.TakenOffHoldBy.Should().Be(33087);
        }
    }
}
