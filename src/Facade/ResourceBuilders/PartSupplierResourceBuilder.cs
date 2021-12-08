namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PartSupplierResourceBuilder : IBuilder<PartSupplier>
    {
        public PartSupplierResource Build(PartSupplier entity, IEnumerable<string> claims)
        {
            return new PartSupplierResource
                       {
                           PartNumber = entity.PartNumber,
                           SupplierId = entity.SupplierId,
                           PartDescription = entity.Part.Description,
                           SupplierName = entity.Supplier.Name,
                           PackingGroup = entity.PackagingGroup?.Id,
                           PackingGroupDescription = entity.PackagingGroup?.Description,
                           CreatedBy = entity.CreatedBy?.Id,
                           CreatedByName = entity.CreatedBy?.FullName,
                           AddressId = entity.DeliveryAddress?.Id,
                           FullAddress = entity.DeliveryAddress?.FullAddress,
                           ManufacturerCode = entity.Manufacturer?.Code,
                           ManufacturerName = entity.Manufacturer?.Name,
                           TariffCode = entity.Tariff?.Code,
                           TariffDescription = entity.Tariff?.Description,
                           OrderMethodName = entity.OrderMethod?.Name,
                           OrderMethodDescription = entity.OrderMethod?.Description,
                           Links = this.BuildLinks(entity).ToArray()
                       };
        }

        public string GetLocation(PartSupplier p)
        {
            return $"/purchasing/part-supplier?partId={p.Part.Id}&supplierId={p.SupplierId}";
        }

        object IBuilder<PartSupplier>.Build(PartSupplier entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PartSupplier model)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
        }
    }
}
