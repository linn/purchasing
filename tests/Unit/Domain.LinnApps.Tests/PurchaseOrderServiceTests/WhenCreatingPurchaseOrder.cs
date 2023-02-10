namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPurchaseOrder : ContextBase
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
                                 OrderDate = 10.January(2021),
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
                                                     PartNumber = "macbookz",
                                                     RohsCompliant = "No",
                                                     SuppliersDesignation = "updated suppliers designation",
                                                     StockPoolCode = "0141noidea",
                                                     OriginalOrderNumber = null,
                                                     OriginalOrderLine = null,
                                                     OurUnitOfMeasure = "cups?",
                                                     OrderUnitOfMeasure = "boxes",
                                                     OurUnitPriceCurrency = 200.22m,
                                                     OrderUnitPriceCurrency = 120m,
                                                     BaseOurUnitPrice = 100m,
                                                     BaseOrderUnitPrice = 100m,
                                                     VatTotalCurrency = 0m,
                                                     BaseVatTotal = 0m,
                                                     DetailTotalCurrency = 120m,
                                                     BaseDetailTotal = 100m,
                                                     DeliveryInstructions = "deliver it",
                                                     DeliveryConfirmedBy = new Employee { Id = 33107, FullName = "me" },
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
                                                                            Notes = "new laptops",
                                                                            OrderNumber = this.orderNumber,
                                                                            Person = 33107,
                                                                            Product = "macs",
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
                                 OrderContactName = "Jim",
                                 OrderMethod = new OrderMethod { Name = "online", Description = "website" },
                                 ExchangeRate = 0.8m,
                                 IssuePartsToSupplier = "N",
                                 DeliveryAddress = new LinnDeliveryAddress { AddressId = 1555 },
                                 RequestedBy = new Employee { FullName = "Jim Halpert", Id = 1111 },
                                 EnteredBy = new Employee { FullName = "Pam Beesley", Id = 2222 },
                                 QuotationRef = "ref11101",
                                 AuthorisedBy = new Employee { FullName = "Dwight Schrute", Id = 3333 },
                                 SentByMethod = "EMAIL",
                                 FilCancelled = string.Empty,
                                 Remarks = "updated remarks",
                                 DateFilCancelled = null,
                                 PeriodFilCancelled = null,
                                 DocumentType = new DocumentType { Description = "Purchase order", Name = "PO" }
                             };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderCreate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.NominalAccountRepository.FindBy(Arg.Any<Expression<Func<NominalAccount, bool>>>())
                .Returns(new NominalAccount
                             {
                                 NominalCode = "00009222"
                             });

            this.PurchaseOrdersPack.GetVatAmountSupplier(Arg.Any<decimal>(), Arg.Any<int>()).Returns(40.55m);

            this.MockDatabaseService.GetIdSequence("PLORP_SEQ").Returns(123);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(
                new PartSupplier { UnitOfMeasure = "Potatoes", LeadTimeWeeks = 2 });

            this.PartQueryRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DateLive = DateTime.Today });

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

            firstDetail.OurUnitPriceCurrency.Should().Be(200.22m);

            // updated based on conv factor
            firstDetail.OrderUnitPriceCurrency.Should().Be(200.22m);

            firstDetail.BaseOurUnitPrice.Should().Be(250.28m);

            // our qty * our unit price
            firstDetail.NetTotalCurrency.Should().Be(19821.78m);
            firstDetail.BaseNetTotal.Should().Be(24777.23m);

            firstDetail.VatTotalCurrency.Should().Be(40.55m);
            firstDetail.BaseVatTotal.Should().Be(50.69m);

            // net total + vat total
            firstDetail.DetailTotalCurrency.Should().Be(19862.33m);
            firstDetail.BaseDetailTotal.Should().Be(24827.91m);

            firstDetail.OrderPosting.NominalAccountId.Should().Be(911);

            firstDetail.PurchaseDeliveries
                .First().DateRequested.Should().Be(DateTime.UnixEpoch);

            firstDetail.OrderPosting.NominalAccount.NominalCode.Should().Be("00009222");
        }

        [Test]
        public void ShouldCallRepositoryAdd()
        {
            this.PurchaseOrderRepository.Received().Add(Arg.Any<PurchaseOrder>());
        }
    }
}
