namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PurchaseOrder current;

        private PurchaseOrder updated;

        [SetUp]
        public void SetUp()
        {
            this.current = new PurchaseOrder
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentTypeName = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                SupplierId = 1224
            };
            this.updated = new PurchaseOrder
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentTypeName = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = "Y",
                OverbookQty = 1,
                SupplierId = 1224
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.Sut.AllowOverbook(this.current, this.updated.Overbook, this.updated.OverbookQty, new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.current.OrderNumber.Should().Be(600179);
            this.current.Overbook.Should().Be("Y");
            this.current.OverbookQty.Should().Be(1);
            this.current.SupplierId.Should().Be(1224);
        }
    }
}
