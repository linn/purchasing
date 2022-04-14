namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Resources;

    public class PurchaseOrderReqStateResourceBuilder : IBuilder<PurchaseOrderReqState>
    {
        public PurchaseOrderReqStateResource Build(PurchaseOrderReqState entity, IEnumerable<string> claims)
        {
            return new PurchaseOrderReqStateResource
                       {
                           State = entity.State,
                           Description = entity.Description,
                           DisplayOrder = entity.DisplayOrder,
                           IsFinalState = entity.IsFinalState
                       };
        }

        public string GetLocation(PurchaseOrderReqState model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PurchaseOrderReqState>.Build(PurchaseOrderReqState entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
