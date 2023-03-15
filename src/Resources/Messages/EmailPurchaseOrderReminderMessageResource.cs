namespace Linn.Purchasing.Resources.Messages
{
    using System;

    public class EmailPurchaseOrderReminderMessageResource
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySeq { get; set; }

        public DateTime Timestamp { get; set; }

        public bool? Test { get; set; }
    }
}
