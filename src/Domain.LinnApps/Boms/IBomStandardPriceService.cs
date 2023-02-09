namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface IBomStandardPriceService
    {
        IEnumerable<BomStandardPrice> GetPriceVarianceInfo(
            string searchExpression);

        SetStandardPriceResult SetStandardPrices(
            IEnumerable<BomStandardPrice> lines, int who, string remarks);
    }
}
