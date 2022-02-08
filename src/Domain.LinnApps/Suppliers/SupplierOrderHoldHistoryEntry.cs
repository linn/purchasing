namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;

    public class SupplierOrderHoldHistoryEntry
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }

        public DateTime DateOnHold { get; set; }

        public string ReasonOnHold { get; set; }

        public DateTime? DateOffHold { get; set; }

        public string ReasonOffHold { get; set; }

        public int PutOnHoldBy { get; set; }

        public int? TakenOffHoldBy { get; set; }
    }
}
