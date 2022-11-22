namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders.MiniOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingPurchaseOrderOverrideQtyFields : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrder current;

        private PurchaseOrder updated;

        private MiniOrder miniOrder;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
                               {
                                   OrderNumber = this.orderNumber,
                                   Cancelled = string.Empty,
                                   DocumentTypeName = string.Empty,
                                   OrderDate = 10.January(2021),
                                   Overbook = string.Empty,
                                   OverbookQty = 0,
                                   SupplierId = 1224,
                                   Details =
                                       new List<PurchaseOrderDetail>
                                           {
                                               new PurchaseOrderDetail
                                                   {
                                                       Cancelled = "N",
                                                       Line = 1,
                                                       BaseNetTotal = 100m,
                                                       NetTotalCurrency = 120m,
                                                       OrderNumber = this.orderNumber,
                                                       OurQty = 12m,
                                                       OrderQty = 12m,
                                                       PartNumber = "PART 31",
                                                       PurchaseDeliveries =
                                                           new List<PurchaseOrderDelivery>
                                                               {
                                                                   new PurchaseOrderDelivery
                                                                       {
                                                                           Cancelled = "N",
                                                                           DateAdvised = 1.February(2022),
                                                                           DateRequested = 23.January(2022),
                                                                           DeliverySeq = 1,
                                                                           NetTotalCurrency = 120m,
                                                                           BaseNetTotal = 100m,
                                                                           OrderDeliveryQty = 1,
                                                                           OrderLine = 1,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurDeliveryQty = 5,
                                                                           QtyNetReceived = 0,
                                                                           QuantityOutstanding = 1,
                                                                           CallOffDate = null,
                                                                           BaseOurUnitPrice = 0m,
                                                                           SupplierConfirmationComment = "supplied",
                                                                           OurUnitPriceCurrency = 0m,
                                                                           OrderUnitPriceCurrency = 0m,
                                                                           BaseOrderUnitPrice = 0m,
                                                                           VatTotalCurrency = 0m,
                                                                           BaseVatTotal = 0m,
                                                                           DeliveryTotalCurrency = 0m,
                                                                           BaseDeliveryTotal = 0m,
                                                                           RescheduleReason = string.Empty,
                                                                           AvailableAtSupplier = "N"
                                                                       },
                                                                   new PurchaseOrderDelivery
                                                                       {
                                                                           Cancelled = "N",
                                                                           DateAdvised = 1.February(2022),
                                                                           DateRequested = 29.January(2022),
                                                                           DeliverySeq = 1,
                                                                           NetTotalCurrency = 120m,
                                                                           BaseNetTotal = 100m,
                                                                           OrderDeliveryQty = 1,
                                                                           OrderLine = 1,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurDeliveryQty = 25,
                                                                           QtyNetReceived = 1,
                                                                           QuantityOutstanding = 1,
                                                                           CallOffDate = null,
                                                                           BaseOurUnitPrice = 100m,
                                                                           SupplierConfirmationComment = "supplied",
                                                                           OurUnitPriceCurrency = 120m,
                                                                           OrderUnitPriceCurrency = 120m,
                                                                           BaseOrderUnitPrice = 100m,
                                                                           VatTotalCurrency = 60m,
                                                                           BaseVatTotal = 60m,
                                                                           DeliveryTotalCurrency = 120m,
                                                                           BaseDeliveryTotal = 100m,
                                                                           RescheduleReason = string.Empty,
                                                                           AvailableAtSupplier = "N"
                                                                       }
                                                               },
                                                       RohsCompliant = "No",
                                                       SuppliersDesignation = "Demo",
                                                       StockPoolCode = "0141noidea",
                                                       OriginalOrderNumber = null,
                                                       OriginalOrderLine = null,
                                                       OurUnitOfMeasure = "cups?",
                                                       OrderUnitOfMeasure = "boxes",
                                                       OurUnitPriceCurrency = 120m,
                                                       OrderUnitPriceCurrency = 120m,
                                                       BaseOurUnitPrice = 100m,
                                                       BaseOrderUnitPrice = 100m,
                                                       VatTotalCurrency = 0m,
                                                       BaseVatTotal = 0m,
                                                       DetailTotalCurrency = 120m,
                                                       BaseDetailTotal = 100m,
                                                       DeliveryInstructions = "deliver it",
                                                       DeliveryConfirmedBy =
                                                           new Employee { Id = 33107, FullName = "me" },
                                                       CancelledDetails = new List<CancelledOrderDetail>(),
                                                       InternalComments = "comment for internal staff",
                                                       OrderConversionFactor = 2m,
                                                       OrderPosting = new PurchaseOrderPosting
                                                                          {
                                                                              Building = "HQ",
                                                                              Id = 1551,
                                                                              LineNumber = 1,
                                                                              NominalAccount =
                                                                                  new NominalAccount
                                                                                      {
                                                                                          AccountId = 3939,
                                                                                          Department =
                                                                                              new Department
                                                                                                  {
                                                                                                      Description =
                                                                                                          "dpt1",
                                                                                                      DepartmentCode =
                                                                                                          "0001111"
                                                                                                  },
                                                                                          Nominal = new Nominal
                                                                                              {
                                                                                                  Description =
                                                                                                      "nom1",
                                                                                                  NominalCode =
                                                                                                      "00002222"
                                                                                              }
                                                                                      },
                                                                              NominalAccountId = 3939,
                                                                              Notes = "new laptops",
                                                                              OrderNumber = this.orderNumber,
                                                                              Person = 33107,
                                                                              Product = "A part",
                                                                              Qty = 1,
                                                                              Vehicle = "van"
                                                                          }
                                                   }
                                           },
                                   Currency = new Currency { Code = "EUR", Name = "Euros" },
                                   OrderContactName = "Jim",
                                   OrderMethod = new OrderMethod { Name = "online", Description = "website" },
                                   ExchangeRate = 1.2m,
                                   IssuePartsToSupplier = "N",
                                   DeliveryAddress = new LinnDeliveryAddress { AddressId = 1555 },
                                   RequestedBy = new Employee { FullName = "Rick F. Pile", Id = 1111 },
                                   EnteredBy = new Employee { FullName = "Pick F. Rile", Id = 2222 },
                                   QuotationRef = "ref11101",
                                   AuthorisedBy = new Employee { FullName = "Fick P. Rile", Id = 3333 },
                                   SentByMethod = "EMAIL",
                                   FilCancelled = string.Empty,
                                   Remarks = "a remark!",
                                   DateFilCancelled = null,
                                   PeriodFilCancelled = null
                               };

            this.updated = new PurchaseOrder
                               {
                                   OrderNumber = this.orderNumber,
                                   Cancelled = string.Empty,
                                   DocumentTypeName = string.Empty,
                                   OrderDate = 10.January(2021),
                                   Overbook = string.Empty,
                                   OverbookQty = 0,
                                   SupplierId = 1224,
                                   Details =
                                       new List<PurchaseOrderDetail>
                                           {
                                               new PurchaseOrderDetail
                                                   {
                                                       Cancelled = "N",
                                                       Line = 1,
                                                       BaseNetTotal = 100m,
                                                       NetTotalCurrency = 120m,
                                                       OrderNumber = this.orderNumber,
                                                       OurQty = 50m,
                                                       OrderQty = 50m,
                                                       PartNumber = "PART 32",
                                                       PurchaseDeliveries =
                                                           new List<PurchaseOrderDelivery>
                                                               {
                                                                   new PurchaseOrderDelivery
                                                                       {
                                                                           Cancelled = "N",
                                                                           DateAdvised = 1.February(2022),
                                                                           DateRequested = 29.January(2022),
                                                                           DeliverySeq = 1,
                                                                           NetTotalCurrency = 120m,
                                                                           BaseNetTotal = 100m,
                                                                           OrderDeliveryQty = 1,
                                                                           OrderLine = 1,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurDeliveryQty = 25,
                                                                           QtyNetReceived = 1,
                                                                           QuantityOutstanding = 1,
                                                                           CallOffDate = null,
                                                                           BaseOurUnitPrice = 100m,
                                                                           SupplierConfirmationComment = "supplied",
                                                                           OurUnitPriceCurrency = 120m,
                                                                           OrderUnitPriceCurrency = 120m,
                                                                           BaseOrderUnitPrice = 100m,
                                                                           VatTotalCurrency = 60m,
                                                                           BaseVatTotal = 60m,
                                                                           DeliveryTotalCurrency = 120m,
                                                                           BaseDeliveryTotal = 100m,
                                                                           RescheduleReason = string.Empty,
                                                                           AvailableAtSupplier = "N"
                                                                       },
                                                                   new PurchaseOrderDelivery
                                                                       {
                                                                           Cancelled = "N",
                                                                           DateAdvised = 1.February(2022),
                                                                           DateRequested = 29.January(2022),
                                                                           DeliverySeq = 1,
                                                                           NetTotalCurrency = 120m,
                                                                           BaseNetTotal = 100m,
                                                                           OrderDeliveryQty = 1,
                                                                           OrderLine = 1,
                                                                           OrderNumber = this.orderNumber,
                                                                           OurDeliveryQty = 25,
                                                                           QtyNetReceived = 1,
                                                                           QuantityOutstanding = 1,
                                                                           CallOffDate = null,
                                                                           BaseOurUnitPrice = 100m,
                                                                           SupplierConfirmationComment = "supplied",
                                                                           OurUnitPriceCurrency = 120m,
                                                                           OrderUnitPriceCurrency = 120m,
                                                                           BaseOrderUnitPrice = 100m,
                                                                           VatTotalCurrency = 60m,
                                                                           BaseVatTotal = 60m,
                                                                           DeliveryTotalCurrency = 120m,
                                                                           BaseDeliveryTotal = 100m,
                                                                           RescheduleReason = string.Empty,
                                                                           AvailableAtSupplier = "N"
                                                                       }
                                                               },
                                                       RohsCompliant = "No",
                                                       SuppliersDesignation = "updated suppliers designation",
                                                       StockPoolCode = "0141noidea",
                                                       OriginalOrderNumber = null,
                                                       OriginalOrderLine = null,
                                                       OurUnitOfMeasure = "cups?",
                                                       OrderUnitOfMeasure = "boxes",
                                                       OurUnitPriceCurrency = 200.22m,
                                                       OrderUnitPriceCurrency = 200m,
                                                       BaseOurUnitPrice = 100m,
                                                       BaseOrderUnitPrice = 100m,
                                                       VatTotalCurrency = 0m,
                                                       BaseVatTotal = 0m,
                                                       DetailTotalCurrency = 120m,
                                                       BaseDetailTotal = 100m,
                                                       DeliveryInstructions = "deliver it",
                                                       DeliveryConfirmedBy =
                                                           new Employee { Id = 33107, FullName = "me" },
                                                       CancelledDetails = new List<CancelledOrderDetail>(),
                                                       InternalComments = "updated internal comment",
                                                       OrderPosting = new PurchaseOrderPosting
                                                                          {
                                                                              Building = "HQ",
                                                                              Id = 1551,
                                                                              LineNumber = 1,
                                                                              NominalAccount =
                                                                                  new NominalAccount
                                                                                      {
                                                                                          AccountId = 911,
                                                                                          Department =
                                                                                              new Department
                                                                                                  {
                                                                                                      Description =
                                                                                                          "Emergency stuff",
                                                                                                      DepartmentCode =
                                                                                                          "0000911"
                                                                                                  },
                                                                                          Nominal = new Nominal
                                                                                              {
                                                                                                  Description =
                                                                                                      "emergcy",
                                                                                                  NominalCode =
                                                                                                      "00009222"
                                                                                              }
                                                                                      },
                                                                              NominalAccountId = 911,
                                                                              Notes = "new parts",
                                                                              OrderNumber = this.orderNumber,
                                                                              Person = 33107,
                                                                              Product = "macs",
                                                                              Qty = 1,
                                                                              Vehicle = "van"
                                                                          }
                                                   }
                                           },
                                   Currency = new Currency { Code = "EUR", Name = "Euros" },
                                   OrderContactName = "Rick",
                                   OrderMethod = new OrderMethod { Name = "online", Description = "website" },
                                   ExchangeRate = 0.8m,
                                   IssuePartsToSupplier = "N",
                                   DeliveryAddress = new LinnDeliveryAddress { AddressId = 1555 },
                                   RequestedBy = new Employee { FullName = "Rick F. Pile", Id = 1111 },
                                   EnteredBy = new Employee { FullName = "Pick F. Rile", Id = 2222 },
                                   QuotationRef = "ref11101",
                                   AuthorisedBy = new Employee { FullName = "Fick P. Rile", Id = 3333 },
                                   SentByMethod = "EMAIL",
                                   FilCancelled = string.Empty,
                                   Remarks = "updated remarks",
                                   DateFilCancelled = null,
                                   PeriodFilCancelled = null
                               };

            this.miniOrder = new MiniOrder
                                 {
                                     OrderNumber = this.orderNumber,
                                     OurQty = 12m,
                                     OrderQty = 12m,
                                     OurPrice = 120m,
                                     OrderPrice = 120m,
                                     NetTotal = 120m,
                                     BaseOurPrice = 100m,
                                     BaseOrderPrice = 100m,
                                     BaseNetTotal = 100m,
                                     Remarks = "applebooks",
                                     Department = "0000911",
                                     Nominal = "00009222",
                                     RequestedDeliveryDate = 23.January(2022),
                                     InternalComments = "comment for internal staff",
                                     SuppliersDesignation = "macbooks",
                                     OrderConvFactor = 2m,
            };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.MiniOrderRepository.FindById(this.orderNumber).Returns(this.miniOrder);

            this.PurchaseOrdersPack.GetVatAmountSupplier(Arg.Any<decimal>(), Arg.Any<int>()).Returns(40.55m);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.NominalAccountRepository.FindById(Arg.Any<int>()).Returns(
                new NominalAccount
                    {
                        AccountId = 911,
                        NominalCode = "00009222",
                        DepartmentCode = "0000911"
                    });

            this.Sut.UpdateOrder(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ShouldUpdateMiniOrderFields()
        {
            this.miniOrder.OrderNumber.Should().Be(600179);
            this.miniOrder.Remarks.Should().Be("updated remarks");

            this.miniOrder.InternalComments.Should().Be("updated internal comment");
            this.miniOrder.SuppliersDesignation.Should().Be("updated suppliers designation");

            this.miniOrder.Nominal.Should().Be("00009222");
            this.miniOrder.Department.Should().Be("0000911");

            this.miniOrder.OurQty.Should().Be(50m);

            //manually overridden here
            this.miniOrder.OrderQty.Should().Be(50m);
            // manually overridden 
            this.miniOrder.OrderPrice.Should().Be(200m);

            this.miniOrder.OurPrice.Should().Be(200.22m);
            this.miniOrder.BaseOurPrice.Should().Be(250.28m);

            // our qty * our unit price
            this.miniOrder.NetTotal.Should().Be(10011.00m);
            this.miniOrder.BaseNetTotal.Should().Be(12513.75m);

            this.miniOrder.VatTotal.Should().Be(40.55m);
            this.miniOrder.BaseVatTotal.Should().Be(50.69m);

            // net total + vat total
            this.miniOrder.OrderTotal.Should().Be(10051.55m);
            this.miniOrder.BaseOrderTotal.Should().Be(12564.44m);
        }

        [Test]
        public void ThirdDeliveryShouldBeCreatedWithRemainder()
        {
            this.current.OrderNumber.Should().Be(600179);
            this.current.Remarks.Should().Be("updated remarks");

            var firstDetail = this.current.Details.First();

            // get third and final delivery
            var count = firstDetail.PurchaseDeliveries.Count;
            var delivery = firstDetail.PurchaseDeliveries.ElementAt(2);
            count.Should().Be(3);

            // new delivery of (38) added to reflect the update on the qty from 12 to 50.
            delivery.OrderUnitPriceCurrency.Should().Be(200m);
            delivery.OurUnitPriceCurrency.Should().Be(200.22m);
            delivery.OurDeliveryQty.Should().Be(38);

            // our delivery qty (38) * our unit currency = net total currency
            delivery.NetTotalCurrency.Should().Be(7608.36m);
            delivery.VatTotalCurrency.Should().Be(40.55m);
            delivery.DeliveryTotalCurrency.Should().Be(7648.91m);

            // base our unit price, base order unit price
            delivery.BaseOurUnitPrice.Should().Be(250.28m);
            delivery.BaseOrderUnitPrice.Should().Be(250m);

            // (our delivery qty (38) * base our unit price) + base vat total  = base delivery total
            delivery.BaseNetTotal.Should().Be(9510.64m);
            delivery.BaseVatTotal.Should().Be(50.69m);
            delivery.BaseDeliveryTotal.Should().Be(9561.33m);
        }
    }
}
