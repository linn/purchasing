namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingOrderAndPartIsPhasedOut : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrder order;

        private Action action;

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
                                                     PartNumber = "P1",
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

            this.PurchaseOrdersPack.GetVatAmountSupplier(Arg.Any<decimal>(), Arg.Any<int>()).Returns(40.55m);

            this.MockDatabaseService.GetIdSequence("PLORP_SEQ").Returns(123);

            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.PartSupplierRepository.FindById(Arg.Any<PartSupplierKey>()).Returns(
                new PartSupplier { UnitOfMeasure = "Potatoes", LeadTimeWeeks = 2 });

            this.PartQueryRepository.FindBy(Arg.Any<Expression<Func<Part, bool>>>())
                .Returns(new Part { DateLive = 1.January(2020), DatePurchPhasedOut = 1.January(2021) });

            this.action = () => this.Sut.CreateOrder(this.order, new List<string>());
        }

        [Test]
        public void ShouldThrowException()
        {
            this.action.Should().Throw<PurchaseOrderException>()
                .WithMessage($"Cannot order part P1 as it has been phased out");
        }
    }
}
