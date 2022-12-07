namespace Linn.Purchasing.Resources.Boms
{
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class PostBomResource
    {
        public int? CrNumber { get; set; }

        public BomTreeNode TreeRoot { get; set; }

        public int? EnteredBy { get; set; }
    }
}
