namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class DeliveryPerformanceDetail
    {
        public int SupplierId { get; set; }

        public DateTime DateArrived { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public string PartNumber { get; set; }

        public DateTime RequestedDate { get; set; }
        
        public DateTime? AdvisedDate { get; set; }

        public string RescheduleReason { get; set; }

        public int OnTime { get; set; }
    }
}
