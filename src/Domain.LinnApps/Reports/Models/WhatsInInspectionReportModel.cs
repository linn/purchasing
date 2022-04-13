namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using Linn.Common.Reporting.Models;

    public class WhatsInInspectionReportModel : WhatsInInspectionViewModel
    {
        public ResultsModel LocationBreakdown { get; set; }
    }
}
