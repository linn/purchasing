namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    public class BoardComponent
    {
        public string BoardCode { get; set; }

        public int BoardLine { get; set; }

        public string CRef { get; set; }

        public string PartNumber { get; set; }

        public string AssemblyTechnology { get; set; }

        public string ChangeState { get; set; }

        public int FromLayoutVersion { get; set; }
        
        public int FromRevisionVersion { get; set; }
        
        public int? ToLayoutVersion { get; set; }

        public int? ToRevisionVersion { get; set; }
        
        public int AddChangeId { get; set; }

        public PcasChange AddChange { get; set; }

        public int? DeleteChangeId { get; set; }

        public PcasChange DeleteChange { get; set; }

        public decimal Quantity { get; set; }
    }
}
