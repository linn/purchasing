namespace Linn.Purchasing.Resources
{
    public class CancelledPurchaseOrderDetailResource
    {
        public int OrderNumber { get; set; }

        public int? LineNumber { get; set; }

        public int? DeliverySequence { get; set; }

        public string DateCancelled { get; set; }

        public EmployeeResource CancelledBy { get; set; }

        public string DateFilCancelled { get; set; }

        public EmployeeResource FilCancelledBy { get; set; }

        public string ReasonCancelled { get; set; }

        public int Id { get; set; }

        public int? PeriodCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public decimal? ValueCancelled { get; set; }

        public string DateUncancelled { get; set; }

        public string DateFilUncancelled { get; set; }

        public decimal? ValueFilCancelled { get; set; }

        public decimal? BaseValueFilCancelled { get; set; }

        public string ReasonFilCancelled { get; set; }
    }
}
