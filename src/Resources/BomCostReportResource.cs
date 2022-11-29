namespace Linn.Purchasing.Resources
{
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public class BomCostReportResource
    {
        public ReportReturnResource Breakdown { get; set; }

        public string SubAssembly { get; set; }

        public decimal MaterialTotal { get; set; }

        public decimal LabourTotal { get; set; }

        public decimal OverallTotal { get; set; }
    }
}
