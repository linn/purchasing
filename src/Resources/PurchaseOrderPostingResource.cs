namespace Linn.Purchasing.Resources
{
    public class PurchaseOrderPostingResource
    {
        public string Building { get; set; }

        public int Id { get; set; }

        public int LineNumber { get; set; }

        public NominalAccountResource NominalAccount { get; set; }

        public int? NominalAccountId { get; set; }

        public string Notes { get; set; }

        public int OrderNumber { get; set; }

        public int? Person { get; set; }

        public string Product { get; set; }

        public decimal Qty { get; set; }

        public string Vehicle { get; set; }
    }
}
