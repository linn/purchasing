namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class UnacknowledgedOrders
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int OrganisationId { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliveryNumber { get; set; }

        public string PartNumber { get; set; }

        public string SuppliersDesignation { get; set; }

        public decimal OurDeliveryQuantity { get; set; }

        public decimal OrderDeliveryQuantity { get; set; }

        public DateTime RequestedDate { get; set; }

        public decimal OrderUnitPrice { get; set; }

        public DateTime CallOffDate { get; set; }
    }
}
