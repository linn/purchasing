namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class UnitOfMeasureResourceBuilder : IBuilder<UnitOfMeasure>
    {
        public UnitOfMeasureResource Build(UnitOfMeasure entity, IEnumerable<string> claims)
        {
            return new UnitOfMeasureResource
                       {
                           Unit = entity.Unit
                       };
        }

        public string GetLocation(UnitOfMeasure p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<UnitOfMeasure>.Build(UnitOfMeasure entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
