namespace Linn.Purchasing.Domain.LinnApps.Tests.SupplierKitServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenGettingSupplierKitFromPurchaseOrder : ContextBase
    {
        private PurchaseOrder order;

        private Supplier supplier;

        private Part supplierPart;

        private Part regularPart;

        private IEnumerable<BomDetailViewEntry> bom;

        private IEnumerable<SupplierKit> results;

        [SetUp]
        public void SetUp()
        {
            this.supplier = new Supplier { SupplierId = 123, Name = "Acme Corp" };
            this.supplierPart = new Part
                                    {
                                        PartNumber = "COMP 001",
                                        LinnProduced = "N",
                                        BomType = "A",
                                        PreferredSupplier = this.supplier
                                    };
            this.regularPart = new Part
                                   {
                                       PartNumber = "COMP 002",
                                       LinnProduced = "N",
                                       BomType = "C",
                                       PreferredSupplier = this.supplier
                                   };
            var compPart1 = new Part { PartNumber = "FLAN 001" };
            var compPart2 = new Part { PartNumber = "EGG-AXT BOX" };
            this.bom = new List<BomDetailViewEntry>
                           {
                               new BomDetailViewEntry { Part = compPart1, PartNumber = compPart1.PartNumber, Qty = 1 },
                               new BomDetailViewEntry { Part = compPart2, PartNumber = compPart2.PartNumber, Qty = 10 }
                           };
            this.order = new PurchaseOrder
                             {
                                 Supplier = this.supplier, SupplierId = this.supplier.SupplierId, Details = new List<PurchaseOrderDetail>
                                     {
                                         new PurchaseOrderDetail
                                             {
                                                 Part = this.regularPart,
                                                 PartNumber = this.regularPart.PartNumber,
                                                 OrderQty = 10,
                                                 OurQty = 10
                                             },
                                         new PurchaseOrderDetail
                                             {
                                                 Part = this.supplierPart,
                                                 PartNumber = this.supplierPart.PartNumber,
                                                 OrderQty = 10,
                                                 OurQty = 10
                                             }
                                     }
                             };
            this.BomDetailRepository.GetLiveBomDetails(Arg.Any<string>()).Returns(this.bom.AsQueryable());

            this.results = this.Sut.GetSupplierKits(this.order, true);
        }

        [Test]
        public void ShouldReturnOneSupplierKit()
        {
            this.results.Count().Should().Be(1);
        }

        [Test]
        public void ShouldReturnSupplierAssemblyPart()
        {
            var kit = this.results.First();
            kit.Should().NotBeNull();
            kit.Part.PartNumber.Should().Be(this.supplierPart.PartNumber);
        }

        [Test]
        public void ShouldIncludeDetailsOfComponents()
        {
            var kit = this.results.First();
            kit.Should().NotBeNull();
            kit.Details.Should().NotBeNull();
            kit.Details.Count().Should().Be(2);
        }

        [Test]
        public void ShouldHaveRightQtyOfComponents()
        {
            var kit = this.results.First();
            var comp1 = kit.Details.ToList()[0];
            var comp2 = kit.Details.ToList()[1];
            comp1.Part.PartNumber.Should().Be("FLAN 001");
            comp1.Qty.Should().Be(10);
            comp2.Part.PartNumber.Should().Be("EGG-AXT BOX");
            comp2.Qty.Should().Be(100);
        }
    }
}
