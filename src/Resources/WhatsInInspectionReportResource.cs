namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    using Linn.Common.Reporting.Resources.ReportResultResources;

    public class WhatsInInspectionReportResource
    {
        public IEnumerable<WhatsInInspectionReportEntryResource> PartsInInspection { get; set; }

        public ReportReturnResource BackOrderData { get; set; }
    }
}
