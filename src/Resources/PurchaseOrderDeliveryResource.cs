namespace Linn.Purchasing.Resources
{
    using System;

    public class PurchaseOrderDeliveryResource
    {
        public string Cancelled { get; set; }

        public DateTime? DateAdvised { get; set; }

        public DateTime? DateRequested { get; set; }

        public int DeliverySeq { get; set; }

        public decimal? NetTotalCurrency { get; set; }

        public decimal BaseNetTotal { get; set; }

        public int? OrderDeliveryQty { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public int? OurDeliveryQty { get; set; }

        public int? QtyNetReceived { get; set; }

        public decimal? QuantityOutstanding { get; set; }

        public DateTime? CallOffDate { get; set; }

        public decimal? BaseOurUnitPrice { get; set; }

        public string SupplierConfirmationComment { get; set; }

        public decimal? OurUnitPriceCurrency { get; set; }

        public decimal? OrderUnitPriceCurrency { get; set; }

        public decimal? BaseOrderUnitPrice { get; set; }

        public decimal? VatTotalCurrency { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public decimal? DeliveryTotalCurrency { get; set; }

        public decimal? BaseDeliveryTotal { get; set; }
    }
}
