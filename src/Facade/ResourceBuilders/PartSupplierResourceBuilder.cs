namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PartSupplierResourceBuilder : IBuilder<PartSupplier>
    {
        private readonly IAuthorisationService authService;

        public PartSupplierResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PartSupplierResource Build(PartSupplier entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new PartSupplierResource
                           {
                               Links = this.BuildLinks(null, claims).ToArray()
                           };
            }

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
                           Designation = entity.SupplierDesignation,
                           CurrencyCode = entity.Currency?.Code,
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(PartSupplier p)
        {
            return $"/purchasing/part-suppliers/record?partId={p.Part.Id}&supplierId={p.SupplierId}";
        }

        object IBuilder<PartSupplier>.Build(PartSupplier entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PartSupplier model, IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
                yield return new LinkResource { Rel = "part", Href = $"/parts/{model.Part.Id}" };

                yield return new LinkResource { Rel = "supplier", Href = $"/purchasing/{model.SupplierId}" };
                if (this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
                {
                    yield return new LinkResource { Rel = "edit", Href = this.GetLocation(model) };
                }
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.PartSupplierCreate, privileges))
            {
                yield return new LinkResource { Rel = "create", Href = $"/purchasing/part-suppliers/create" };
            }
        }
    }
}
