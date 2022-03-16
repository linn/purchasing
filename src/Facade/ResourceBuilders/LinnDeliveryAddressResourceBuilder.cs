namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class LinnDeliveryAddressResourceBuilder : IBuilder<LinnDeliveryAddress>
    {
        public LinnDeliveryAddressResource Build(LinnDeliveryAddress entity, IEnumerable<string> claims)
        {
            return new LinnDeliveryAddressResource
                       {
                           Description = entity.Description,
                           AddressId = entity.AddressId,
                           Address = entity.FullAddress.AddressString
                       };
        }

        public string GetLocation(LinnDeliveryAddress p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<LinnDeliveryAddress>.Build(LinnDeliveryAddress entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
