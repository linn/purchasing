namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PackagingGroupResourceBuilder : IBuilder<PackagingGroup>
    {
        public PackagingGroupResource Build(PackagingGroup entity, IEnumerable<string> claims)
        {
            return new PackagingGroupResource
                       {
                           Id = entity.Id,
                           Description = entity.Description
                       };
        }

        public string GetLocation(PackagingGroup p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PackagingGroup>.Build(PackagingGroup entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
