namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PreferredSupplierChangeResourceBuilder : IBuilder<PreferredSupplierChange>
    {
        private readonly IAuthorisationService authService;

        public PreferredSupplierChangeResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PreferredSupplierChangeResource Build(PreferredSupplierChange entity, IEnumerable<string> claims)
        {
            return new PreferredSupplierChangeResource
                       {
                           ChangeReasonCode = entity.ChangeReason?.ReasonCode,
                           ChangeReasonDescription = entity.ChangeReason?.Description,
                           ChangedById = entity.ChangedBy.Id,
                           ChangedByName = entity.ChangedBy.FullName,
                           OldCurrencyCode = entity.OldCurrency?.Code,
                           DateChanged = entity.DateChanged.ToString("o"),
                           NewPrice = entity.NewPrice,
                           NewSupplierId = entity.NewSupplier.SupplierId,
                           NewSupplierName = entity.NewSupplier.Name,
                           OldPrice = entity.OldPrice,
                           OldSupplierId = entity.OldSupplier?.SupplierId,
                           OldSupplierName = entity.OldSupplier?.Name,
                           PartNumber = entity.PartNumber,
                           Remarks = entity.Remarks
                       };
        }

        public string GetLocation(PreferredSupplierChange p)
        {
            throw new NotImplementedException();

        }

        object IBuilder<PreferredSupplierChange>.Build(PreferredSupplierChange entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(PreferredSupplierChange model, IEnumerable<string> claims)
        {
            throw new NotImplementedException();
        }
    }
}
