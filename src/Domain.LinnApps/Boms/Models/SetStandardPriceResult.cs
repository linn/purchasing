namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    using System.Collections.Generic;

    public class SetStandardPriceResult : ProcessResult
    {
        public int? ReqNumber { get; set; }

        public IEnumerable<BomStandardPrice> Lines { get; set; }
    }
}

