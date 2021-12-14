namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PackagingGroupsResourceBuilder : IBuilder<IEnumerable<PackagingGroup>>
    {
        public IEnumerable<PackagingGroupResource> Build(IEnumerable<PackagingGroup> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new PackagingGroupResource { Id = e.Id, Description = e.Description });
        }

        public string GetLocation(IEnumerable<PackagingGroup> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<PackagingGroup>>.Build(IEnumerable<PackagingGroup> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
