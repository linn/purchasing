namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SupplierGroupsWithUnacknowledgedOrders
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? SupplierGroupId { get; set; }

        public string SupplierGroupName { get; set; }

        public string VendorManager { get; set; }

        public int? Planner { get; set; }
    }
}
