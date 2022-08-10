namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SupplierDeliveryPerformance
    {
        public int LedgerPeriod { get; set; }

        public string MonthName { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string VendorManager { get; set; }

        public int NumberOfDeliveries { get; set; }

        public int NumberOnTime { get; set; }

        public int NumberOfEarlyDeliveries { get; set; }

        public int NumberOfUnacknowledgedDeliveries { get; set; }

        public int NumberOfLateDeliveries { get; set; }
    }
}
