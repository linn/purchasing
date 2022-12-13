namespace Linn.Purchasing.Resources.Boms
{
    public class BoardComponentSummaryResource
    {
        public string BoardCode { get; set; }

        public string RevisionCode { get; set; }

        public string Cref { get; set; }

        public string PartNumber { get; set; }

        public string AssemblyTechnology { get; set; }

        public decimal Quantity { get; set; }

        public int BoardLine { get; set; }

        public string ChangeState { get; set; }

        public int AddChangeId { get; set; }

        public int? DeleteChangeId { get; set; }

        public int FromLayoutVersion { get; set; }

        public int FromRevisionVersion { get; set; }

        public int? ToLayoutVersion { get; set; }

        public int? ToRevisionVersion { get; set; }

        public int LayoutSequence { get; set; }

        public int VersionNumber { get; set; }

        public string BomPartNumber { get; set; }

        public string PcasPartNumber { get; set; }

        public string PcsmPartNumber { get; set; }

        public string PcbPartNumber { get; set; }
    }
}
