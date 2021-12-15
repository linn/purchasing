namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class ManufacturerResourceBuilder : IBuilder<Manufacturer>
    {
        public ManufacturerResource Build(Manufacturer entity, IEnumerable<string> claims)
        {
            return new ManufacturerResource
                       {
                           Code = entity.Code,
                           Name = entity.Name
                       };
        }

        public string GetLocation(Manufacturer p)
        {
            return $"/purchasing/manufacturers/{p.Code}";
        }

        object IBuilder<Manufacturer>.Build(Manufacturer entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Manufacturer model, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
