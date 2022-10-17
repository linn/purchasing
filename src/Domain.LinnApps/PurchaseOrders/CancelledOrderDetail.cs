namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;

    public class CancelledOrderDetail
    {
        public int OrderNumber { get; set; }

        public int? LineNumber { get; set; }

        public int? DeliverySequence { get; set; }

        public DateTime? DateCancelled { get; set; }

        public Employee CancelledBy { get; set; }

        public int? CancelledById { get; set; }

        public DateTime? DateFilCancelled { get; set; }

        public Employee FilCancelledBy { get; set; }

        public int? FilCancelledById { get; set; }

        public string ReasonCancelled { get; set; }

        public int Id { get; set; }

        public int? PeriodCancelled { get; set; }

        public int? PeriodFilCancelled { get; set; }

        public decimal? ValueCancelled { get; set; }

        public DateTime? DateUncancelled { get; set; }

        public DateTime? DateFilUncancelled { get; set; }

        public DateTime? DatePreviouslyCancelled { get; set; }

        public DateTime? DatePreviouslyFilCancelled { get; set; }

        public decimal? ValueFilCancelled { get; set; }

        public decimal? BaseValueFilCancelled { get; set; }

        public string ReasonFilCancelled { get; set; }
    }
}
