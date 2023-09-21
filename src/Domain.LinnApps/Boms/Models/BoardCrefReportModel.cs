namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    public class BoardCrefReportModel
    {
        public string CRef { get; set; }

        public string PartNumber { get; set; }

        public int BoardLine { get; set; }

        public string AssemblyTechnology { get; set; }

        public decimal Quantity { get; set; }

        public int Sequence { get; set; }
    }
}
