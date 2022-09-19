namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class BomResourceBuilder : IBuilder<Bom>
    {
        public object Build(Bom model, IEnumerable<string> claims)
        {
            return new BomResource
                       {
                           BomName = model.BomName,
                           BomId = model.BomId,
                           Details = model.Details.Select(
                               d => new BomDetailResource
                                        {
                                            BomType = d.Part.BomType, 
                                            PartNumber = d.PartNumber, 
                                            PartDescription = d.Part.Description,
                                            BomId = d.Part.BomId
                                        })
                       };
        }

        public string GetLocation(Bom model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<Bom>.Build(Bom entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
