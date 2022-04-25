namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using Linn.Common.Reporting.Models;

    public class PartsInInspectionReportEntry : PartsInInspection
    {
        public ResultsModel OrdersBreakdown { get; set; }

        public ResultsModel LocationsBreakdown { get; set; }

        public string Batch { get; set; }
    }
}
