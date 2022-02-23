namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierGroupResourceBuilder : IBuilder<SupplierGroup>
    {
        public SupplierGroupResource Build(SupplierGroup entity, IEnumerable<string> claims)
        {
            return new SupplierGroupResource
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public string GetLocation(SupplierGroup supplierGroup)
        {
            return $"/purchasing/supplier-groups/{supplierGroup.Id}";
        }

        object IBuilder<SupplierGroup>.Build(SupplierGroup entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(SupplierGroup model, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
