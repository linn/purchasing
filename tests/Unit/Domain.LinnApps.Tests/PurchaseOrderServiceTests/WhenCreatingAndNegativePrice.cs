namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAndNegativePrice : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrder order;

        private Action act;

        [SetUp]
        public void SetUp()
        {
            this.order = new PurchaseOrder
            {
                OrderNumber = this.orderNumber,
                Cancelled = string.Empty,
                DocumentTypeName = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                Supplier = new Supplier { SupplierId = 1224 },
                Details =
                                     new List<PurchaseOrderDetail>
                                         {
                                             new PurchaseOrderDetail
                                                 {
                                                     OriginalOrderNumber = null,
                                                     OriginalOrderLine = null,
                                                     OurUnitOfMeasure = "cups?",
                                                     OrderUnitOfMeasure = "boxes",
                                                     OurUnitPriceCurrency = -200.22m,
                                                     OrderUnitPriceCurrency = -120m,
                                                     BaseOurUnitPrice = -100m,
                                                     BaseOrderUnitPrice = -100m,
                                                     VatTotalCurrency = 0m,
                                                     BaseVatTotal = 0m,
                                                     DetailTotalCurrency = -120m,
                                                     BaseDetailTotal = -100m,
                                                     DeliveryInstructions = "deliver it",
                                                     DeliveryConfirmedBy = new Employee { Id = 33107, FullName = "me" }
                                                 }
                                         },
                DocumentType = new DocumentType { Description = "Purchase order", Name = "PO" }
            };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.act = () => this.Sut.CreateOrder(this.order, new List<string>(), out _);
        }

        [Test]
        public void ShouldThrow()
        {
            this.act.Should().Throw<PurchaseOrderException>("Prices must be positive numbers");
        }

        [Test]
        public void ShouldNotAdd()
        {
            this.PurchaseOrderRepository.DidNotReceive().Add(Arg.Any<PurchaseOrder>());
        }
    }
}
