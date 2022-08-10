namespace Linn.Purchasing.Domain.LinnApps.Tests.AutomaticPurchaseOrderServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using FluentAssertions.Extensions;

    using Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenCreatingAutomaticOrder : ContextBase
    {
        private AutomaticPurchaseOrder proposedOrder;

        private AutomaticPurchaseOrder result;

        private int startedBy;

        private string partNumber;

        private int supplierId;

        private DateTime requestedDate;

        private string jobRef;

        [SetUp]
        public void SetUp()
        {
            this.startedBy = 34;
            this.partNumber = "P1";
            this.jobRef = "ABC";
            this.supplierId = 56;
            this.requestedDate = 1.January(2032);

            this.SigningLimitRepository.FindById(this.startedBy)
                .Returns(new SigningLimit { ProductionLimit = 1234 });
            this.CurrencyPack.CalculateBaseValueFromCurrencyValue("GBP", 1.11m).Returns(1.11m);
            this.PurchaseOrdersPack.IssuePartsToSupplier(this.partNumber, this.supplierId).Returns(true);
            this.PurchaseOrderAutoOrderPack
                .CreateAutoOrder(this.partNumber, this.supplierId, 12, this.requestedDate, null, true)
                .Returns(new CreateOrderFromReqResult { Message = "ok", Success = true });
            this.proposedOrder = new AutomaticPurchaseOrder
                                     {
                                         StartedBy = this.startedBy,
                                         JobRef = this.jobRef,
                                         DateRaised = 1.July(2031),
                                         SupplierId = 56,
                                         Planner = 78,
                                         Details = new List<AutomaticPurchaseOrderDetail>
                                                       {
                                                           new AutomaticPurchaseOrderDetail
                                                               {
                                                                   Sequence = 1,
                                                                   PartNumber = this.partNumber,
                                                                   SupplierId = this.supplierId,
                                                                   SupplierName = "S1",
                                                                   QuantityRecommended = 12,
                                                                   RecommendationCode = "SOME",
                                                                   CurrencyCode = "GBP",
                                                                   CurrencyPrice = 1.11m,
                                                                   BasePrice = 1.11m,
                                                                   Quantity = 12,
                                                                   RequestedDate = this.requestedDate
                                                               }
                                                       }
                                     };
            this.result = this.Sut.CreateAutomaticPurchaseOrder(this.proposedOrder);
        }

        [Test]
        public void ShouldReturnOrder()
        {
            this.result.JobRef.Should().Be(this.jobRef);
            this.result.Details.Should().HaveCount(1);
            this.result.Details.First().SupplierId.Should().Be(this.supplierId);
            this.result.Details.First().PartNumber.Should().Be(this.partNumber);
            this.result.Details.First().Quantity.Should().Be(12);
        }
    }
}
