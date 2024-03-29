﻿namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class BomDetail
    {
        public int DetailId { get; set; }

        public int BomId { get; set; }

        public string PartNumber { get; set; }

        public decimal? Qty { get; set; }

        public string GenerateRequirement { get; set; }

        public string ChangeState { get; set; }

        public BomChange AddChange { get; set; }

        public int? AddChangeId { get; set; }

        public int? AddReplaceSeq { get; set; }

        public BomChange DeleteChange { get; set; }

        public int? DeleteChangeId { get; set; }

        public int? DeleteReplaceSeq { get; set; }

        public string PcasLine { get; set; }

        public Part Part { get; set; }
    }
}
