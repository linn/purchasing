namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class SupplierSpend
    {
        public decimal? BaseTotal { get; set; }

        public int LedgerPeriod { get; set; }

        public int? OrderLine { get; set; }

        public int? OrderNumber { get; set; }

        public int PlTRef { get; set; }

        public int SupplierId { get; set; }

        public string SupplierName { get; set; }

        public string VendorManager { get; set; }

        public string PartNumber { get; set; }

        public string PartDescription { get; set; }
    }
}
