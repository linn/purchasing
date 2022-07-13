namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class ShortagesPlannerEntry
    {
        public int? Planner { get; set; }

        public string VendorManagerCode { get; set; }

        public string PurchaseLevel { get; set; }

        public string VendorManagerInitials { get; set; }

        public string VendorManagerName { get; set; }

        public int? PreferredSupplier { get; set; }

        public string SupplierName { get; set; }

        public string PartNumber { get; set; }

        public string Description { get; set; }

        public decimal? QtyAvailable { get; set; }

        public decimal? TotalWoReqt { get; set; }

        public decimal? TotalBiReqt { get; set; }

        public decimal? TotalBeReqt { get; set; }

        public decimal? TotalBtReqt { get; set; }

        public string EdPartNumber { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySeq { get; set; }

        public DateTime? RequestedDate { get; set; }

        public DateTime? AdvisedDate { get; set; }

        public int? QtyOutstanding { get; set; }
    }
}
