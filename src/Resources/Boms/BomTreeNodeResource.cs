namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections.Generic;

    public class BomTreeNodeResource
    {
        public string BomName { get; set; }

        public string BomType { get; set; }

        public decimal Qty { get; set; }

        private IEnumerable<BomTreeNodeResource> Children { get; set; }
    }
}
