namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class BomHistoryReportLine
    {
        public int? ChangeId { get; set; }

        public string BomName { get; set; }

        public string DocumentType { get; set; }

        public int? DocumentNumber { get; set; }

        public DateTime? DateApplied { get; set; }

        public string AppliedBy { get; set; }

        public string Operation { get; set; }

        public string PartNumber { get; set; }

        public string Qty { get; set; }

        public string GenerateRequirement { get; set; }

        public int? ReplaceSeq { get; set; }

        public string DetailId { get; set; }
    }
}
