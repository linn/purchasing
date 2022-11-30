namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using Linn.Common.Reporting.Models;

    public class BomCostReport
    {
        public ResultsModel Breakdown { get; set; }

        public string SubAssembly { get; set; }

        public decimal MaterialTotal { get; set; }

        public decimal LabourTotal { get; set; }

        public decimal OverallTotal { get; set; }
    }
}
