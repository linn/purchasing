namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    public class BomTypeChange
    {
        public string PartNumber { get; set; }

        public string OldBomType { get; set; }

        public string NewBomType { get; set; }

        public int? OldSupplierId { get; set; }

        public int? NewSupplierId { get; set; }

        public int? ChangedBy { get; set; }

        public Part Part { get; set; }
    }
}
