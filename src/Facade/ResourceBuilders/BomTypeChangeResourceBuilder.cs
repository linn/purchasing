namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class BomTypeChangeResourceBuilder : IBuilder<BomTypeChange>
    {
        public BomTypeChangeResource Build(BomTypeChange model, IEnumerable<string> claims)
        {
            var resource = new BomTypeChangeResource
                               {
                                   PartNumber = model.PartNumber,
                                   OldBomType = model.OldBomType,
                                   OldSupplierId = model.OldSupplierId,
                                   NewBomType = model.NewBomType,
                                   NewSupplierId = model.NewSupplierId
                               };

            if (model.Part != null)
            {
                resource.PartDescription = model.Part.Description;
                resource.PartBomType = model.Part.BomType;
                resource.PartCurrency = model.Part.Currency.Name;
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
    }
}
