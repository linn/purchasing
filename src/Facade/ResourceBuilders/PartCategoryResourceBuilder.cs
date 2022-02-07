namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartCategoryResourceBuilder : IBuilder<PartCategory>
    {
        public PartCategoryResource Build(PartCategory entity, IEnumerable<string> claims)
        {
            return new PartCategoryResource
                       {
                           Category = entity.Category,
                           Description = entity.Description
                       };
        }

        public string GetLocation(PartCategory p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PartCategory>.Build(PartCategory entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
