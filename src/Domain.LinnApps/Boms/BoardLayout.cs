namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    public class BoardLayout
    {
        public string BoardCode { get; set; }
        
        public string LayoutCode { get; set; }
        
        public int LayoutSequence { get; set; }

        public string PcbNumber { get; set; }

        public string LayoutType { get; set; }

        public int? LayoutNumber { get; set; }

        public string PcbPartNumber { get; set; }
        
        public int? ChangeId { get; set; }

        public string ChangeState { get; set; }
    }
}
