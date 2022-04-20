namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    public class WhatsInInspectionBackOrderData
    {
        public string ArticleNumber { get; set; }

        public string Story { get; set; }

        public decimal QtyInInspection { get; set; }

        public decimal QtyNeeded { get; set; }
    }
}
