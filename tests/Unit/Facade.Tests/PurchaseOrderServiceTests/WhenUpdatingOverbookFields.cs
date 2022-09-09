namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingOverbookFields : ContextBase
    {
        private PurchaseOrder model;

        private IResult<PurchaseOrderResource> result;

        private PurchaseOrderResource updateResource;

        [SetUp]
        public void SetUp()
        {
            this.model = new PurchaseOrder
                             {
                                 OrderNumber = 600179,
                                 Cancelled = string.Empty,
                                 OrderDate = 10.January(2021),
                                 Overbook = "Y",
                                 OverbookQty = 1,
                                 SupplierId = 1224,
                                 Supplier = new Supplier { SupplierId = 1224 },
                                 CurrencyCode = "GBP",
                                 OrderMethodName = "carrier pigeon",
                                 RequestedById = 111,
                                 EnteredById = 222,
            };

            this.updateResource = new PurchaseOrderResource
                                      {
                                          OrderNumber = 600179,
                                          Cancelled = string.Empty,
                                          OrderDate = 10.January(2021).ToString("O"),
                                          Overbook = "Y",
                                          OverbookQty = 1,
                                          Supplier = new SupplierResource { Id = 1224 },
                                          CurrentlyUsingOverbookForm = true

            };

            this.PurchaseOrderRepository.Add(this.model);
            this.PurchaseOrderRepository.FindById(this.model.OrderNumber).Returns(this.model);
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.result = this.Sut.Update(this.updateResource.OrderNumber, this.updateResource, new List<string>(), 33111);
        }

        [Test]
        public void ShouldBuildCorrectResourceWithLinks()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>) this.result).Data;
            dataResult.Links.Length.Should().Be(5);
            dataResult.Links.First().Rel.Should().Be("allow-over-book-search");
            dataResult.Links.First().Href.Should().Be("/purchasing/purchase-orders/allow-over-book");
        }

        [Test]
        public void ShouldCallOverbookDomainMethod()
        {
            this.DomainService.Received().AllowOverbook(
                Arg.Any<PurchaseOrder>(),
                this.model.Overbook,
                this.model.OverbookQty,
                Arg.Any<List<string>>());
        }

        [Test]
        public void ShouldAddToOverbookLogRepo()
        {
            this.OverbookAllowedByLogRepository.Received().Add(Arg.Any<OverbookAllowedByLog>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>) this.result).Data;
            dataResult.OrderNumber.Should().Be(this.model.OrderNumber);
            dataResult.Cancelled.Should().Be(this.model.Cancelled);
            dataResult.OrderDate.Should().Be(this.model.OrderDate.ToString("O"));
            dataResult.Overbook.Should().Be(this.model.Overbook);
            dataResult.OverbookQty.Should().Be(this.model.OverbookQty);
            dataResult.Supplier.Id.Should().Be(this.model.SupplierId);
        }
    }
}
