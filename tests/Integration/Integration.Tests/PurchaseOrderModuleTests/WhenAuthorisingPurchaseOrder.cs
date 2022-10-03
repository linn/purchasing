namespace Linn.Purchasing.Integration.Tests.PurchaseOrderModuleTests
{
    using System.Collections.Generic;
    using System.Net;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Integration.Tests.Extensions;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenAuthorisingPurchaseOrder : ContextBase
    {
        private readonly int orderNumber = 999;
        [SetUp]
        public void SetUp()
        {
            var order = new PurchaseOrder
            {
                OrderNumber = this.orderNumber,
                Cancelled = string.Empty,
                DocumentTypeName = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                SupplierId = 1224,
                Supplier = new Supplier { SupplierId = 1224, Name = "suppliyah", VendorManagerId = "JIMBO" },
                Currency = new Currency { Code = "EUR", Name = "Euros" },
                OrderContactName = "Jim",
                OrderMethod = new OrderMethod { Name = "online", Description = "website" },
                ExchangeRate = 0.8m,
                IssuePartsToSupplier = "N",
                DeliveryAddress = new LinnDeliveryAddress
                                      {
                                          AddressId = 5678,
                                          Description = "Linn",
                                          IsMainDeliveryAddress = "Y",
                                          FullAddress = new FullAddress { AddressString = "Linn HQ, waterfut" }
                                      },
                RequestedBy = new Employee { FullName = "Jim Halpert", Id = 1111 },
                EnteredBy = new Employee { FullName = "Pam Beesley", Id = 2222 },
                QuotationRef = "ref11101",
                AuthorisedBy = new Employee { FullName = "Dwight Schrute", Id = 3333 },
                SentByMethod = "EMAIL",
                FilCancelled = string.Empty,
                Remarks = "updated remarks",
                DateFilCancelled = null,
                PeriodFilCancelled = null
            };

            this.MockDomainService.AuthorisePurchaseOrder(
                    Arg.Any<PurchaseOrder>(),
                    Arg.Any<int>(),
                    Arg.Any<IEnumerable<string>>())
                .Returns(new ProcessResult(true, "authorised 👻"));

            this.MockPurchaseOrderRepository.FindById(this.orderNumber).Returns(order);

            this.Response = this.Client.Post(
                "/purchasing/purchase-orders/999/authorise",
                with => { with.Accept("application/json"); }).Result;
        }

        [Test]
        public void ShouldReturnJsonBody()
        {
            var resource = this.Response.DeserializeBody<PurchaseOrderResource>();
            resource.Should().NotBeNull();
            resource.OrderNumber.Should().Be(999);
        }

        [Test]
        public void ShouldReturnJsonContentType()
        {
            this.Response.Content.Headers.ContentType.Should().NotBeNull();
            this.Response.Content.Headers.ContentType?.ToString().Should().Be("application/json");
        }

        [Test]
        public void ShouldReturnOk()
        {
            this.Response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void ShouldCommit()
        {
            this.TransactionManager.Received().Commit();
        }
    }
}
