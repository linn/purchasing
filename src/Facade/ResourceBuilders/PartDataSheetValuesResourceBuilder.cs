namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class PartDataSheetValuesResourceBuilder : IBuilder<PartDataSheetValues>
    {
        public PartDataSheetValuesResource Build(PartDataSheetValues model, IEnumerable<string> claims)
        {
            return new PartDataSheetValuesResource
                       {
                           AttributeSet = model.AttributeSet,
                           Field = model.Field,
                           Value = model.Value,
                           Description = model.Description,
                           AssemblyTechnology = model.AssemblyTechnology,
                           ImdsNumber = model.ImdsNumber,
                           ImdsWeight = model.ImdsWeight
                       };
        }

        public string GetLocation(PartDataSheetValues model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<PartDataSheetValues>.Build(PartDataSheetValues entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
