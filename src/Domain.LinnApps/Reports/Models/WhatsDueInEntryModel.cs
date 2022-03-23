namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class WhatsDueInEntryModel
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public string SupplierName { get; set; }

        public int DeliverySequence { get; set; }

        public string PartNumber { get; set; }

        public decimal? OurDeliveryQty { get; set; }

        public decimal? QtyOutstanding { get; set; }

        public DateTime? ExpectedDate { get; set; }
        
        public string StockPoolCode { get; set; }

        public DateTime? CallOffDate { get; set; }

        public string DocumentType { get; set; }

        public int SupplierId { get; set; }

        public decimal? UnitPrice { get; set; }

        public DateTime? RequestedDate{ get; set; }

        public DateTime? AdvisedDate { get; set; }
    }
}
