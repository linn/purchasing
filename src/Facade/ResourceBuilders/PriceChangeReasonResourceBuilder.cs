namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PriceChangeReasonResourceBuilder : IBuilder<PriceChangeReason>
    {
        public PriceChangeReasonResource Build(PriceChangeReason entity, IEnumerable<string> claims)
        {
            return new PriceChangeReasonResource
                       {
                           Description = entity.Description,
                           ReasonCode = entity.ReasonCode
                       };
        }

        public string GetLocation(PriceChangeReason p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PriceChangeReason>.Build(PriceChangeReason entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
