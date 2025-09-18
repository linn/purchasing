namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    public class ChangeSerno
    {
        public string DocumentType { get; set; }
        
        public int DocumentNumber { get; set; }
        
        public string SernosSequence { get; set; }
        
        public int? StartingSerialNumber { get; set; }
        
        public string Product { get; set;  }
    }
}