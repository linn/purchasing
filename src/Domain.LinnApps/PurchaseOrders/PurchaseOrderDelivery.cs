namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using System.Collections.Generic;

    public class PurchaseOrderDelivery
    {
        public string Cancelled { get; set; }

        public DateTime? DateAdvised { get; set; }

        public DateTime? DateRequested { get; set; }

        public int DeliverySeq { get; set; }

        public decimal? NetTotalCurrency { get; set; }

        public decimal BaseNetTotal { get; set; }

        public decimal? OrderDeliveryQty { get; set; }

        public int OrderLine { get; set; }

        public int OrderNumber { get; set; }

        public decimal? OurDeliveryQty { get; set; }

        public PurchaseOrderDetail PurchaseOrderDetail { get; set; }

        public decimal? QtyNetReceived { get; set; }

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

        public string RescheduleReason { get; set; }

        public string AvailableAtSupplier { get; set; }

        public string CallOffRef { get; set; }

        public string FilCancelled { get; set; }

        public decimal? QtyPassedForPayment { get; set; }

        public IEnumerable<PurchaseOrderDeliveryHistory> DeliveryHistories { get; set; }
    }
}
