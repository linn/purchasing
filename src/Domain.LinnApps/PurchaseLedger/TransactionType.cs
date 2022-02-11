namespace Linn.Purchasing.Domain.LinnApps.PurchaseLedger
{
    using System;

    public class TransactionType
    {
        public int CreditNomacc { get; set; }

        public DateTime DateInvalid { get; set; }

        public int DebitNomacc { get; set; }

        public string DebitOrCredit { get; set; }

        public string Description { get; set; }

        public string TransactionCategory { get; set; }

        public string TransType { get; set; }
    }
}
