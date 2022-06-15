namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;

    public class MrPurchaseOrderDetail
    {
        public string JobRef { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public DateTime DateOfOrder { get; set; }

        public string PartNumber { get; set; }

        public decimal OurQuantity { get; set; }

        public decimal? QuantityReceived { get; set; }
        
        public decimal? QuantityInvoiced { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string SupplierContact { get; set; }
        
        public string Remarks { get; set; }

        public string AuthorisedBy { get; set; }

        public IEnumerable<MrPurchaseOrderDelivery> Deliveries { get; set; }
    }
}
