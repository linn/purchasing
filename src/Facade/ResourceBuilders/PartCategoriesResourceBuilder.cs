namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartCategoriesResourceBuilder : IBuilder<IEnumerable<PartCategory>>
    {
        public IEnumerable<PartCategoryResource> Build(IEnumerable<PartCategory> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new PartCategoryResource { Category = e.Category, Description = e.Description });
        }

        public string GetLocation(IEnumerable<PartCategory> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<PartCategory>>.Build(IEnumerable<PartCategory> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
