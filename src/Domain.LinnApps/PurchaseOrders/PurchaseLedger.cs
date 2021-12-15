namespace Linn.Purchasing.Domain.LinnApps.PurchaseLedger
{
    using System;

    public class PurchaseLedger
    {
        public string BaseCurrency { get; set; }

        public decimal? BaseNetTotal { get; set; }

        public decimal? BaseTotal { get; set; }

        public decimal? BaseVatTotal { get; set; }

        public decimal Carriage { get; set; }

        public string CompanyRef { get; set; }

        public int? CreditNomacc { get; set; }

        public string Currency { get; set; }

        public DateTime DatePosted { get; set; }

        public int? DebitNomacc { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime InvoiceDate { get; set; }

        public int LedgerPeriod { get; set; }

        public int LedgerStream { get; set; }

        public int? OrderLine { get; set; }

        public int? OrderNumber { get; set; }

        public string PlDeliveryRef { get; set; }

        public string PlInvoiceRef { get; set; }

        public decimal PlNetTotal { get; set; }

        public decimal? PlQuantity { get; set; }

        public string PlState { get; set; }

        public decimal PlTotal { get; set; }

        public string PlTransType { get; set; }

        public int Pltref { get; set; }

        public decimal PlVat { get; set; }

        public int? PostedBy { get; set; }

        public int SupplierId { get; set; }

        public decimal UnderOver { get; set; }

        public TransactionType TransactionType { get; }
    }
}
