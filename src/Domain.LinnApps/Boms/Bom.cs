namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    public class Bom
    {
        public int BomId { get; set; }

        public string BomName { get; set; }

        public IEnumerable<BomDetail> Details { get; set; }
    }
}
