namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenUpdatingAndNegativePrice : ContextBase
    {
        private readonly int orderNumber = 600179;

        private PurchaseOrder updated;

        private Action act;

        [SetUp]
        public void SetUp()
        {
            this.updated = new PurchaseOrder
                               {
                                   OrderNumber = this.orderNumber,
                                   Details =
                                       new List<PurchaseOrderDetail>
                                           {
                                               new PurchaseOrderDetail
                                                   {
                                                       Cancelled = "N",
                                                       Line = 1,
                                                       BaseNetTotal = -100m,
                                                       NetTotalCurrency = -120m,
                                                       OrderNumber = this.orderNumber,
                                                       OurQty = 99m,
                                                       OrderQty = 12m,
                                                       PartNumber = "P"
                                                   }
                                           }
                               };

            this.MockAuthService.HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);


            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });


            this.act = () => this.Sut.UpdateOrder(new PurchaseOrder(), this.updated, new List<string>());
        }


        [Test]
        public void ShouldThrow()
        {
            this.act.Should().Throw<PurchaseOrderException>("Prices must be positive numbers");
        }
    }
}
