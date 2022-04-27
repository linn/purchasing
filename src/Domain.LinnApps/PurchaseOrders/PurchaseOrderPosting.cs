namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    public class PurchaseOrderPosting
    {
        public string Building { get; set; }

        public int Id { get; set; }

        public int LineNumber { get; set; }

        public NominalAccount NominalAccount { get; set; }

        public int NominalAccountId { get; set; }

        public string Notes { get; set; }

        public int OrderNumber { get; set; }

        public string Person { get; set; }

        public string Product { get; set; }

        public decimal Qty { get; set; }

        public string Vehicle { get; set; }
    }
}
