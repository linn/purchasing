namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDeliveryServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentAssertions;

    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NSubstitute;

    using NUnit.Framework;

    public class WhenBatchUpdatingAndInvalidRescheduleReason : ContextBase
    {
        private IEnumerable<PurchaseOrderDeliveryUpdate> changes;

        private PurchaseOrderDeliveryKey key1;

        private BatchUpdateProcessResult result;

        [SetUp]
        public void SetUp()
        {
            this.PurchaseLedgerMaster.GetRecord().Returns(new PurchaseLedgerMaster { OkToRaiseOrder = "Y" });

            this.AuthService
                .HasPermissionFor(AuthorisedAction.PurchaseOrderUpdate, Arg.Any<IEnumerable<string>>())
                .Returns(true);
            this.key1 = new PurchaseOrderDeliveryKey { OrderNumber = 123456, OrderLine = 1, DeliverySequence = 1 };

            this.changes = new List<PurchaseOrderDeliveryUpdate>
                               {
                                   new PurchaseOrderDeliveryUpdate
                                       {
                                           Key = this.key1,
                                           NewReason = "INVALID REASON"
                                       }
                               };

            this.Repository.FindById(
                    Arg.Is<PurchaseOrderDeliveryKey>(
                        x => x.OrderLine == this.key1.OrderLine && x.OrderNumber == this.key1.OrderNumber
                                                               && x.DeliverySequence == this.key1.DeliverySequence))
                .Returns(
                    new PurchaseOrderDelivery
                    {
                        OrderNumber = this.key1.OrderNumber,
                        OrderLine = this.key1.OrderLine,
                        DeliverySeq = this.key1.DeliverySequence
                    });
           
            this.Repository.FilterBy(
                    Arg.Any<Expression<Func<PurchaseOrderDelivery, bool>>>())
                .Returns(
                    new List<PurchaseOrderDelivery>
                        {
                            new PurchaseOrderDelivery()
                        }.AsQueryable());
            this.RescheduleReasonRepository.FindAll().Returns(new List<RescheduleReason>
                                                                  {
                                                                      new RescheduleReason
                                                                          {
                                                                              Reason = "ADVISED"
                                                                          }
                                                                  }.AsQueryable());
            this.result = this.Sut.BatchUpdateDeliveries(this.changes, new List<string>());
        }

        [Test]
        public void ShouldReturnFailResult()
        {
            this.result.Success.Should().BeFalse();
            this.result.Message.Should().Be("0 records updated successfully. The following errors occurred: ");
            this.result.Errors.First().Descriptor.Should().Be("123456 / 1 / 1");
            this.result.Errors.First().Message.Should().Be("INVALID REASON is not a valid reason");
        }
    }
}
