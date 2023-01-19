namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class Bom
    {
        public int BomId { get; set; }

        public string BomName { get; set; }

        public Part Part { get; set; }

        public IList<BomDetailViewEntry> Details { get; set; }

        public int Depth { get; set; }

        public string CommonBom { get; set; }
    }
}
