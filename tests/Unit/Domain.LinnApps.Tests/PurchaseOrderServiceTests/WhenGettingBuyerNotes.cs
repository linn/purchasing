namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingBuyerNotes : ContextBase
    {
        private PurchaseOrder order;

        private int supplierId;

        private string result;

        [SetUp]
        public void SetUp()
        {
            this.supplierId = 123;
            this.order = new PurchaseOrder
                             {
                                 OrderNumber = 808,
                                 SupplierId = this.supplierId,
                                 Supplier = new Supplier { SupplierId = this.supplierId, NotesForBuyer = "Supplier Notes" },
                                 Details = new List<PurchaseOrderDetail>
                                               {
                                                   new PurchaseOrderDetail
                                                       {
                                                           Line = 1,
                                                           PartNumber = "P1"
                                                       },
                                                   new PurchaseOrderDetail
                                                       {
                                                           Line = 2,
                                                           PartNumber = "P2"
                                                       }
                                               }
                             };

            this.PartSupplierRepository
                .FindById(Arg.Is<PartSupplierKey>(a => a.PartNumber == "P1" && a.SupplierId == this.supplierId))
                .Returns(new PartSupplier { NotesForBuyer = "P1 Notes" });
            this.PartSupplierRepository
                .FindById(Arg.Is<PartSupplierKey>(a => a.PartNumber == "P2" && a.SupplierId == this.supplierId))
                .Returns(new PartSupplier { NotesForBuyer = "P2 Notes" });
            this.result = this.Sut.GetOrderNotesForBuyer(this.order);
        }

        [Test]
        public void ShouldReturnNotes()
        {
            this.result.Should().Be($"Supplier Notes {Environment.NewLine}P1 Notes {Environment.NewLine}P2 Notes {Environment.NewLine}");
        }
    }
}
