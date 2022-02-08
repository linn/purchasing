namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndObjectFieldsNotChanged : ContextBase
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
                DocumentType = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                SupplierId = 1224
            };
            this.updated = new PurchaseOrder
            {
                OrderNumber = 600179,
                Cancelled = string.Empty,
                DocumentType = string.Empty,
                OrderDate = 10.January(2021),
                Overbook = string.Empty,
                OverbookQty = 0,
                SupplierId = 1224
            };
            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.Sut.UpdatePurchaseOrder(this.current, this.updated, new List<string>());
        }

        [Test]
        public void ItemShouldRemainTheSame()
        {
            this.current.OrderNumber.Should().Be(600179);
            this.current.Overbook.Should().Be(string.Empty);
            this.current.OverbookQty.Should().Be(0);
            this.current.SupplierId.Should().Be(1224);
        }
    }
}
