namespace Linn.Purchasing.Domain.LinnApps.Tests.PurchaseOrderDetailTests
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using NUnit.Framework;

    public class ContextBase
    {
        public PurchaseOrderDetail Sut { get; set; }

        [SetUp]
        public void SetUpContext()
        {
            this.Sut = new PurchaseOrderDetail
                           {
                               OrderNumber = 123,
                               Line = 1,
                               Part = new Part(),
                               OurQty = 1,
                               PurchaseOrder = new PurchaseOrder { OrderNumber = 123 },
                               PurchaseDeliveries = new List<PurchaseOrderDelivery>
                                                        {
                                                            new PurchaseOrderDelivery
                                                                {
                                                                    DeliverySeq = 1,
                                                                    OurDeliveryQty = 1,
                                                                    QtyNetReceived = 0
                                                                }
                                                        }
                           };
        }
    }
}
