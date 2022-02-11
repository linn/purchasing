﻿namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    public class PurchaseOrderDetail
    {
        public string Cancelled { get; set; }

        public int Line { get; set; }

        public decimal NetTotal { get; set; }

        public int OrderNumber { get; set; }

        public int? OurQty { get; set; }

        public string PartNumber { get; set; }

        public IEnumerable<PurchaseOrderDelivery> PurchaseDeliveries { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; }

        public string RohsCompliant { get; set; }

        public string SuppliersDesignation { get; set; }
    }
}
