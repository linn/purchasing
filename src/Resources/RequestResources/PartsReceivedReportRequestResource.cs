namespace Linn.Purchasing.Resources.RequestResources
{
    public class PartsReceivedReportRequestResource
    {
        public string Jobref { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int? Supplier { get; set; }

        public bool IncludeNegativeValues { get; set; }

        public string OrderBy { get; set; }
    }
}
