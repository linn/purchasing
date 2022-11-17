namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class BomTypeChangeResourceBuilder : IBuilder<BomTypeChange>
    {
        private readonly IAuthorisationService authService;

        public BomTypeChangeResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public BomTypeChangeResource Build(BomTypeChange model, IEnumerable<string> claims)
        {
            var resource = new BomTypeChangeResource
                               {
                                   PartNumber = model.PartNumber,
                                   OldBomType = model.OldBomType,
                                   OldSupplierId = model.OldSupplierId,
                                   NewBomType = model.NewBomType,
                                   NewSupplierId = model.NewSupplierId,
                                   ChangedBy = model.ChangedBy,
                                   Links = claims != null ? this.BuildLinks(claims).ToArray() : null
            };

            if (model.Part != null)
            {
                resource.PartDescription = model.Part.Description;
                resource.PartBomType = model.Part.BomType;
                resource.PartCurrency = model.Part.Currency?.Code;
                resource.PartCurrencyUnitPrice = model.Part.CurrencyUnitPrice;
                resource.PartBaseUnitPrice = model.Part.BaseUnitPrice;
            }

            return resource;
        }

        public string GetLocation(BomTypeChange model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<BomTypeChange>.Build(BomTypeChange entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(IEnumerable<string> claims)
        {
            var privileges = claims as string[] ?? claims.ToArray();

            if (this.authService.HasPermissionFor(AuthorisedAction.ChangeBomType, privileges))
            {
                yield return new LinkResource { Rel = "change-bom-type", Href = $"/purchasing/bom-type-change" };
            }
        }
    }
}
