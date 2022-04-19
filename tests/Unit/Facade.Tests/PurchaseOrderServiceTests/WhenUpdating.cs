namespace Linn.Purchasing.Facade.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PurchaseOrderResource updateResource;

        private IResult<PurchaseOrderResource> result;

        private PurchaseOrder model;

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
                                 SupplierId = 1224
            };

            this.updateResource = new PurchaseOrderResource()
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DateOfOrder = 10.January(2021),
                Overbook = "Y",
                OverbookQty = 1,
                SupplierId = 1224
            };
            this.PurchaseOrderRepository.Add(this.model);
            this.PurchaseOrderRepository.FindById(this.model.OrderNumber).Returns(this.model);
            this.AuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>()).Returns(true);
            this.result = this.Sut.Update(this.updateResource.OrderNumber, this.updateResource, new List<string>());
        }

        [Test]
        public void ShouldReturnSuccess()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
            dataResult.OrderNumber.Should().Be(this.model.OrderNumber);
            dataResult.Cancelled.Should().Be(this.model.Cancelled);
            dataResult.DateOfOrder.Should().Be(this.model.OrderDate);
            dataResult.Overbook.Should().Be(this.model.Overbook);
            dataResult.OverbookQty.Should().Be(this.model.OverbookQty);
            dataResult.SupplierId.Should().Be(this.model.SupplierId);
        }

        [Test]
        public void ShouldBuildCorrectResourceWithLinks()
        {
            this.result.Should().BeOfType<SuccessResult<PurchaseOrderResource>>();
            var dataResult = ((SuccessResult<PurchaseOrderResource>)this.result).Data;
            dataResult.Links.Length.Should().Be(4);
            dataResult.Links.First().Rel.Should().Be("allow-over-book-search");
            dataResult.Links.First().Href.Should().Be("purchasing/purchase-orders/allow-over-book");
        }
    }
}
