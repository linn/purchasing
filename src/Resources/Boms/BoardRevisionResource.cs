namespace Linn.Purchasing.Resources.Boms
{
    public class BoardRevisionResource
    {
        public string BoardCode { get; set; }
        
        public string LayoutCode { get; set; }
        
        public string RevisionCode { get; set; }
        
        public int LayoutSequence { get; set; }

        public int VersionNumber { get; set; }

        public BoardRevisionTypeResource RevisionType { get; set; }

        public int? RevisionNumber { get; set; }

        public string SplitBom { get; set; }

        public string PcasPartNumber { get; set; }

        public string PcsmPartNumber { get; set; }

        public string PcbPartNumber { get; set; }

        public string AteTestCommissioned { get; set; }

        public int? ChangeId { get; set; }

        public string ChangeState { get; set; }
    }
}
