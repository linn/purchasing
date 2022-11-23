namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingFOCPurchaseOrder : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrder order;

        private PurchaseOrder result;

        [SetUp]
        public void SetUp()
        {
            this.order = new PurchaseOrder
                             {
                                 OrderNumber = this.orderNumber,
                                 Cancelled = string.Empty,
                                 DocumentTypeName = string.Empty,
                                 OrderDate = 10.November(2021),
                                 Overbook = string.Empty,
                                 OverbookQty = 0,
                                 Supplier = new Supplier { SupplierId = 1224 },
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
                                                     OurQty = 99m,
                                                     OrderQty = 12m,
                                                     PartNumber = "Freebie",
                                                     RohsCompliant = "No",
                                                     SuppliersDesignation = "FOC Order",
                                                     StockPoolCode = "0141987645",
                                                     OriginalOrderNumber = null,
                                                     OriginalOrderLine = null,
                                                     OurUnitOfMeasure = "cups?",
                                                     OrderUnitOfMeasure = "boxes",
                                                     OurUnitPriceCurrency = 0m,
                                                     OrderUnitPriceCurrency = 120m,
                                                     BaseOurUnitPrice = 100m,
                                                     BaseOrderUnitPrice = 100m,
                                                     VatTotalCurrency = 0m,
                                                     BaseVatTotal = 0m,
                                                     DetailTotalCurrency = 120m,
                                                     BaseDetailTotal = 100m,
                                                     DeliveryInstructions = "deliver it",
                                                     DeliveryConfirmedBy = new Employee { Id = 33107, FullName = "me" },
                                                     InternalComments = "New Freebie Order",
                                                     OrderPosting = new PurchaseOrderPosting
                                                                        {
                                                                            Building = "ABC",
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
                                                                            Notes = "freebie",
                                                                            OrderNumber = this.orderNumber,
                                                                            Person = 33107,
                                                                            Product = "Free",
                                                                            Qty = 1,
                                                                            Vehicle = "van"
                                                                        },
                                                     PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                                              {
                                                                                  new PurchaseOrderDelivery
                                                                                      {
                                                                                          DateRequested = DateTime.UnixEpoch
                                                                                      }
                                                                              }
                                                 }
                                         },
                                 Currency = new Currency { Code = "EUR", Name = "Euros" },
                                 OrderContactName = "Rick.F.Pile",
                                 OrderMethod = new OrderMethod { Name = "online", Description = "website" },
                                 ExchangeRate = 0.8m,
                                 IssuePartsToSupplier = "N",
                                 DeliveryAddress = new LinnDeliveryAddress { AddressId = 1555 },
                                 RequestedBy = new Employee { FullName = "Employee O'RealPerson", Id = 1111 },
                                 EnteredBy = new Employee { FullName = "Person McPerson", Id = 2222 },
                                 QuotationRef = "ref11101",
                                 AuthorisedBy = new Employee { FullName = "Mr Manager", Id = 3333 },
                                 SentByMethod = "EMAIL",
                                 FilCancelled = string.Empty,
                                 Remarks = "updated remarks",
                                 DateFilCancelled = null,
                                 PeriodFilCancelled = null,
                                 DocumentType = new DocumentType { Description = "Purchase order", Name = "PO" }
                             };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.PurchaseOrdersPack.GetVatAmountSupplier(Arg.Any<decimal>(), Arg.Any<int>()).Returns(0m);

            this.MockDatabaseService.GetIdSequence("PLORP_SEQ").Returns(123);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(
                new PartSupplier { UnitOfMeasure = "Carrots", LeadTimeWeeks = 2 });

            this.result = this.Sut.CreateOrder(this.order, new List<string>());
        }

        [Test]
        public void ShouldSetFields()
        {
            this.result.SentByMethod.Should().Be("EMAIL");

            var firstDetail = this.result.Details.First();

            firstDetail.OrderPosting.Id.Should().Be(123);
            firstDetail.OrderConversionFactor.Should().Be(1, "conversion factor set to 1 for everything for now");

            firstDetail.OurQty.Should().Be(99m);

            // updated based on conv factor
            firstDetail.OrderQty.Should().Be(99m);

            firstDetail.OurUnitPriceCurrency.Should().Be(0m);

            // updated based on conv factor
            firstDetail.OrderUnitPriceCurrency.Should().Be(0m);

            firstDetail.BaseOurUnitPrice.Should().Be(0m);

            // our qty * our unit price
            firstDetail.NetTotalCurrency.Should().Be(0m);
            firstDetail.BaseNetTotal.Should().Be(0m);

            firstDetail.VatTotalCurrency.Should().Be(0m);
            firstDetail.BaseVatTotal.Should().Be(0m);

            // net total + vat total
            firstDetail.DetailTotalCurrency.Should().Be(0m);
            firstDetail.BaseDetailTotal.Should().Be(0m);

            firstDetail.OrderPosting.NominalAccountId.Should().Be(911);

            firstDetail.PurchaseDeliveries
                .First().DateRequested.Should().Be(DateTime.UnixEpoch);
           
            //check delivery values 
            firstDetail.PurchaseDeliveries.First().NetTotalCurrency.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().OrderUnitPriceCurrency.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().VatTotalCurrency.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().OurUnitPriceCurrency.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().BaseVatTotal.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().BaseNetTotal.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().BaseOrderUnitPrice.Should().Be(0);
            firstDetail.PurchaseDeliveries.First().BaseOurUnitPrice.Should().Be(0);
        }

        [Test]
        public void ShouldCallRepositoryAdd()
        {
            this.PurchaseOrderRepository.Received().Add(Arg.Any<PurchaseOrder>());
        }
    }
}
