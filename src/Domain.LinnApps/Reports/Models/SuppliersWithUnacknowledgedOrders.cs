namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SuppliersWithUnacknowledgedOrders
    {
        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int? SupplierGroupId { get; set; }

        public string SupplierGroupName { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }
    }
}
