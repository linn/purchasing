namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PriceChangeReasonsResourceBuilder : IBuilder<IEnumerable<PriceChangeReason>>
    {
        public IEnumerable<PriceChangeReasonResource> Build(IEnumerable<PriceChangeReason> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new PriceChangeReasonResource { ReasonCode = e.ReasonCode, Description = e.Description });
        }

        public string GetLocation(IEnumerable<PriceChangeReason> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<PriceChangeReason>>.Build(IEnumerable<PriceChangeReason> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
