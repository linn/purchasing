namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class PartUsedOn
    {
        public string PartNumber { get; set; }
        
        public int Seq { get; set; }

        public string RootProduct { get; set;  }
    }
}
