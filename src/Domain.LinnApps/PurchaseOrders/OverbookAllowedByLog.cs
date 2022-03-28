namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System;
    using Linn.Common.Domain;

    public class OverbookAllowedByLog : LogTable
    {
        public int Id { get; set; }

        public int OrderNumber { get; set; }

        public int OverbookGrantedBy { get; set; }

        public DateTime OverbookDate { get; set; }

        public decimal? OverbookQty { get; set; }
    }
}
