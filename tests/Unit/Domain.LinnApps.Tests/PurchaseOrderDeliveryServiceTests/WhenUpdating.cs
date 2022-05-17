namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System.Collections.Generic;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdating : ContextBase
    {
        private PurchaseOrderDelivery fromState;

        private PurchaseOrderDelivery toState;

        private PurchaseOrderDeliveryKey key;

        private PurchaseOrderDelivery result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);

            this.key = new PurchaseOrderDeliveryKey
                           {
                               DeliverySequence = 1,
                               OrderLine = 1,
                               OrderNumber = 123456
                           };

            this.fromState = new PurchaseOrderDelivery();

            this.toState = new PurchaseOrderDelivery
                               {
                                   AdvisedDate = 10.March(2001),
                                   SupplierConfirmationComment = "COMMENT",
                                   RescheduleReason = "ADVISED",
                                   AvailableAtSupplier = "Y"
                               };

            this.Repository.FindById(
                Arg.Is<PurchaseOrderDeliveryKey>(
                    x => x.OrderLine == 1 && x.OrderNumber == 123456 && x.DeliverySequence == 1))
                .Returns(new PurchaseOrderDelivery
                             {
                                 DeliverySeq = 1,
                                 OrderLine = 1,
                                 OrderNumber = 123456
                });

            this.result = this.Sut
                .UpdateDelivery(this.key, this.fromState, this.toState, new List<string>());
        }

        [Test]
        public void ShouldUpdate()
        {
            this.result.AdvisedDate.Should().Be(this.toState.AdvisedDate);
            this.result.SupplierConfirmationComment.Should().Be(this.toState.SupplierConfirmationComment);
            this.result.RescheduleReason.Should().Be(this.toState.RescheduleReason);
            this.result.AvailableAtSupplier.Should().Be(this.toState.AvailableAtSupplier);
        }
    }
}
