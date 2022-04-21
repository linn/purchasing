namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Models;

    public class WhatsInInspectionReport
    {
        public IEnumerable<PartsInInspectionReportEntry> PartsInInspection { get; set; }

        public ResultsModel BackOrderData { get; set; }
    }
}
