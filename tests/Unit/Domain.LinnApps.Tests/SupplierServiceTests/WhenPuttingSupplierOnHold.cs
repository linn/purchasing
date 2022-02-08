namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenPuttingSupplierOnHold : AuthorisedContext
    {
        private Supplier result;

        private SupplierOrderHoldHistoryEntry data;

        [SetUp]
        public void SetUp()
        {
            this.data = new SupplierOrderHoldHistoryEntry
                            {
                                SupplierId = 1, 
                                PutOnHoldBy = 33087, 
                                ReasonOnHold = "SOME REASON",
                                Id = 123
                            };

            this.MockSupplierRepository.FindById(1).Returns(new Supplier { SupplierId = 1, OrderHold = "N" });

            this.result = this.Sut.ChangeSupplierHoldStatus(this.data, new List<string> { AuthorisedAction.SupplierHoldChange });
        }

        [Test]
        public void ShouldUpdateHoldStatus()
        {
            this.result.OrderHold.Should().Be("Y");
        }

        [Test]
        public void ShouldAddHistoryEntry()
        {
            this.MockSupplierOrderHoldHistory
                .Received().Add(Arg.Is<SupplierOrderHoldHistoryEntry>(
                    e => e.ReasonOnHold == this.data.ReasonOnHold 
                         && e.PutOnHoldBy == this.data.PutOnHoldBy
                         && e.DateOnHold == DateTime.Today
                         && e.Id == this.data.Id));
        }
    }
}
