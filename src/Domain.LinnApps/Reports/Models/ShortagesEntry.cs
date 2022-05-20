namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class ShortagesEntry
    {
        public int Planner { get; set; }

        public string PlannerName { get; set; }

        public string VendorManagerCode { get; set; }

        public string PartNumber { get; set; }

        public string VendorManagerName { get; set; }

        public string PurchaseLevel { get; set; }
    }
}
