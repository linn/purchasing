namespace Linn.Purchasing.Resources.Messages
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class EmailPurchaseOrderReminderMessageResource
    {
        public IEnumerable<PurchaseOrderDeliveryKey> Deliveries { get; set; }

        public DateTime Timestamp { get; set; }

        public bool? Test { get; set; }
    }
}
