namespace Linn.Purchasing.Resources
{
    using Linn.Common.Reporting.Resources.ReportResultResources;

    public class WhatsInInspectionReportEntryResource
    {
        public string PartNumber { get; set; }

        public string Description { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public decimal QtyInStock { get; set; }

        public decimal QtyInInspection { get; set; }

        public ReportReturnResource OrdersBreakdown { get; set; }

        public ReportReturnResource LocationsBreakdown { get; set; }
    }
}
