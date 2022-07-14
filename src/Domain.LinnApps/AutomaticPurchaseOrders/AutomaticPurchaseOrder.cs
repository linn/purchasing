namespace Linn.Purchasing.Domain.LinnApps.AutomaticPurchaseOrders
{
    using System;
    using System.Collections.Generic;

    public class AutomaticPurchaseOrder
    {
        public int Id { get; set; }

        public int StartedBy { get; set; }

        public string JobRef { get; set; }

        public DateTime DateRaised { get; set; }

        public int? SupplierId { get; set; }

        public int? Planner { get; set; }

        public IEnumerable<AutomaticPurchaseOrderDetail> Details { get; set; }
    }
}
