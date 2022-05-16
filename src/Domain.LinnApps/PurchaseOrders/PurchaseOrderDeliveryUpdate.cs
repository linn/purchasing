﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PurchaseOrderDeliveryUpdate
    {
        public PurchaseOrderDeliveryKey Key { get; set; }

        public DateTime? NewDateAdvised { get; set; }

        public string NewReason { get; set; }
    }
}