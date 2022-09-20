namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    public class BomResource
    {
        public int BomId { get; set; }

        public string BomName { get; set; }

        public IEnumerable<BomDetailResource> Details { get; set; }
    }
}
