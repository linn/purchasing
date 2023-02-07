namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public class BomStandardPricesResource
    {
        public IEnumerable<BomStandardPrice> Lines { get; set; }

        public int? ReqNumber { get; set; }

        public string Message { get; set; }

        public string Remarks { get; set; }
    }
}
