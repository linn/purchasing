namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections.Generic;

    public class BomTreeNodeResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<BomTreeNodeResource> Children { get; set; }
    }
}
