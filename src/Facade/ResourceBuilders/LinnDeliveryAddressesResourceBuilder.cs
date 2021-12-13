namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class LinnDeliveryAddressesResourceBuilder : IBuilder<IEnumerable<LinnDeliveryAddress>>
    {
        private readonly IBuilder<LinnDeliveryAddress> resourceBuilder;

        public LinnDeliveryAddressesResourceBuilder(IBuilder<LinnDeliveryAddress> resourceBuilder)
        {
            this.resourceBuilder = resourceBuilder;
        }

        public IEnumerable<LinnDeliveryAddressResource> Build(
            IEnumerable<LinnDeliveryAddress> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => (LinnDeliveryAddressResource)this.resourceBuilder.Build(e, claims));
        }

        public string GetLocation(IEnumerable<LinnDeliveryAddress> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<LinnDeliveryAddress>>.Build(IEnumerable<LinnDeliveryAddress> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
