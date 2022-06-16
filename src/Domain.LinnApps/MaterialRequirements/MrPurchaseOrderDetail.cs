namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

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

        public string OrderType { get; set; }

        public string SubType { get; set; }

        public DateTime? DateCancelled { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? AdvisedDeliveryDate { get; set; }

        public DateTime? LinnDeliveryDate { get; set; }

        public DateTime? BestDeliveryDate { get; set; }

        public PartSupplier PartSupplierRecord { get; set; }
    }
}
