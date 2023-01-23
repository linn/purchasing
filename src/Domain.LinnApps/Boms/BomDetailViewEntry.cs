namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class BomDetailViewEntry
    {
        public string BomName { get; set; }

        public int BomId { get; set; }

        public int DetailId { get; set; }

        public string PartNumber { get; set; }

        public Part Part { get; set; }

        public decimal Qty { get; set; }

        public string GenerateRequirement { get; set; }

        public string ChangeState { get; set; }

        public int? AddChangeId { get; set; }

        public int? AddReplaceSeq { get; set; }

        public int? DeleteChangeId { get; set; }

        public int? DeleteReplaceSeq { get; set; }

        public PartRequirement PartRequirement { get; set; }

        public string BomPartNumber { get; set; }

        public Part BomPart { get; set; }

        public string PcasLine { get; set; }

        public IEnumerable<BomDetailComponent> Components { get; set; }

        public BomChange AddChange { get; set; }

        public BomChange DeleteChange { get; set; }
    }
}
