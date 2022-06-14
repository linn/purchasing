namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    using Linn.Common.Resources;

    public class MrPurchaseOrderResource : HypermediaResource
    {
        public string JobRef { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public string DateOfOrder { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string PartNumber { get; set; }

        public decimal Quantity { get; set; }
        
        public decimal? QuantityReceived { get; set; }
        
        public decimal? QuantityInvoiced { get; set; }

        public string SupplierContact { get; set; }

        public string Remarks { get; set; }

        public string UnauthorisedWarning { get; set; }

        public IEnumerable<MrDeliveryResource> Deliveries { get; set; }
    }
}
