namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http.Json;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingPurchaseOrder : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrderResource resource;

        [SetUp]
        public void SetUp()
        {
            this.resource = new PurchaseOrderResource
            {
                OrderNumber = 600179,
                Supplier = new SupplierResource { Id = 1111, Name = "seller" },
                Cancelled = "N",
                DocumentType = new DocumentTypeResource { Description = "order", Name = "PO" },
                OrderDate = 1.January(2022).ToString("O"),
                Overbook = "N",
                OverbookQty = null,
                Details =
                                        new List<PurchaseOrderDetailResource>
                                            {
                                                new PurchaseOrderDetailResource
                                                    {
                                                        Cancelled = "N",
                                                        Line = 1,
                                                        BaseNetTotal = 100m,
                                                        NetTotalCurrency = 120m,
                                                        OrderNumber = this.orderNumber,
                                                        OurQty = 12m,
                                                        OrderQty = 12m,
                                                        PartNumber = "macbookz",
                                                        PurchaseDeliveries =
                                                            new List<PurchaseOrderDeliveryResource>
                                                                {
                                                                    new PurchaseOrderDeliveryResource
                                                                        {
                                                                            Cancelled = "N",
                                                                            DateAdvised =
                                                                                1.February(2022).ToString("O"),
                                                                            DateRequested =
                                                                                23.January(2022).ToString("O"),
                                                                            DeliverySeq = 1,
                                                                            NetTotalCurrency = 120m,
                                                                            BaseNetTotal = 100m,
                                                                            OrderDeliveryQty = 1,
                                                                            OrderLine = 1,
                                                                            OrderNumber = this.orderNumber,
                                                                            OurDeliveryQty = 1,
                                                                            QtyNetReceived = 0,
                                                                            QuantityOutstanding = 1,
                                                                            CallOffDate = null,
                                                                            BaseOurUnitPrice = 100m,
                                                                            SupplierConfirmationComment = "supplied",
                                                                            OurUnitPriceCurrency = 120m,
                                                                            OrderUnitPriceCurrency = 120m,
                                                                            BaseOrderUnitPrice = 100m,
                                                                            VatTotalCurrency = 0m,
                                                                            BaseVatTotal = 0m,
                                                                            DeliveryTotalCurrency = 120m,
                                                                            BaseDeliveryTotal = 100m,
                                                                            RescheduleReason = string.Empty,
                                                                            AvailableAtSupplier = "N"
                                                                        }
                                                                },
                                                        RohsCompliant = "No",
                                                        SuppliersDesignation = "macbooks",
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
                                                            new EmployeeResource { Id = 33107, FullName = "me" },
                                                        CancelledDetails =
                                                            new List<CancelledPurchaseOrderDetailResource>(),
                                                        InternalComments = "comment for internal staff",
                                                        OrderPosting = new PurchaseOrderPostingResource
                                                                           {
                                                                               Building = "HQ",
                                                                               Id = 1551,
                                                                               LineNumber = 1,
                                                                               NominalAccount =
                                                                                   new NominalAccountResource
                                                                                       {
                                                                                           AccountId = 3939,
                                                                                           Department =
                                                                                               new DepartmentResource
                                                                                                   {
                                                                                                       Description =
                                                                                                           "dpt1",
                                                                                                       DepartmentCode =
                                                                                                           "0001111"
                                                                                                   },
                                                                                           Nominal = new NominalResource
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
                                                                               Product = "macs",
                                                                               Qty = 1,
                                                                               Vehicle = "van"
                                                                           }
                                                    }
                                            },
                Currency = new CurrencyResource { Code = "EUR", Name = "Euros" },
                OrderContactName = "Jim",
                OrderMethod = new OrderMethodResource { Name = "online", Description = "website" },
                ExchangeRate = 1.2m,
                IssuePartsToSupplier = "N",
                DeliveryAddress = new LinnDeliveryAddressResource { AddressId = 1555 },
                RequestedBy = new EmployeeResource { FullName = "Jim Halpert", Id = 1111 },
                EnteredBy = new EmployeeResource { FullName = "Pam Beesley", Id = 2222 },
                QuotationRef = "ref11101",
                AuthorisedBy = new EmployeeResource { FullName = "Dwight Schrute", Id = 3333 },
                SentByMethod = "EMAIL",
                FilCancelled = string.Empty,
                Remarks = "applebooks",
                DateFilCancelled = null,
                PeriodFilCancelled = null,
                InvoiceAddressId = 1066,
                OrderAddress = new AddressResource { AddressId = 1914 }
            };

            this.MockSupplierRepository.FindById(Arg.Any<int>())
                .Returns(new Supplier { SupplierId = 1111, Name = "seller", VendorManagerId = "007 🔫" });

            this.MockNominalAccountRepository.FindById(Arg.Any<int>())
                .Returns(new NominalAccount { NominalCode = "00030405", DepartmentCode = "00001892", AccountId = 918 });

            this.MockDomainService.CreateOrder(Arg.Any<PurchaseOrder>(), Arg.Any<IEnumerable<string>>())
                .Returns(new PurchaseOrder { DocumentType = new DocumentType { Name = "PO" } });

            this.Response = this.Client.PostAsJsonAsync("/purchasing/purchase-orders", this.resource).Result;
        }

        [Test]
        public void ShouldCallCreate()
        {
            this.MockDomainService.Received().CreateOrder(
                Arg.Any<PurchaseOrder>(),
                Arg.Any<IEnumerable<string>>());
        }

        [Test]
        public void ShouldNotCallCreateDebitNote()
        {
            this.MockPlCreditDebitNoteService.DidNotReceive().CreateDebitOrCreditNoteFromPurchaseOrder(
                Arg.Any<PurchaseOrder>());
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resultResource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resultResource.Should().NotBeNull();
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}
