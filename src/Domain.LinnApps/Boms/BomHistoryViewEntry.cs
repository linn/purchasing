namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;

    public class BomHistoryViewEntry
    {
        public int? ChangeId { get; set; }

        public string BomName { get; set; }

        public string DocumentType { get; set; }

        public int? DocumentNumber { get; set; }

        public DateTime? DateApplied { get; set; }

        public string AppliedBy { get; set; }

        public string Operation { get; set; }

        public string PartNumber { get; set; }

        public int? Qty { get; set; }

        public string GenerateRequirement { get; set; }

        public int? ReplaceSeq { get; set; }

        public int? DetailId { get; set; }
    }
}
