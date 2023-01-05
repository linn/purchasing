namespace Linn.Purchasing.Resources.Boms
{
    public class BoardComponentResource
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

        public string AddChangeDocumentType { get; set; }

        public int? AddChangeDocumentNumber { get; set; }

        public int? DeleteChangeId { get; set; }

        public string DeleteChangeDocumentType { get; set; }

        public int? DeleteChangeDocumentNumber { get; set; }

        public decimal Quantity { get; set; }
    }
}
