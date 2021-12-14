namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class UnitsOfMeasureResourceBuilder : IBuilder<IEnumerable<UnitOfMeasure>>
    {
        public IEnumerable<UnitOfMeasureResource> Build(IEnumerable<UnitOfMeasure> entityList, IEnumerable<string> claims)
        {
            return entityList.Select(
                e => new UnitOfMeasureResource { Unit = e.Unit });
        }

        public string GetLocation(IEnumerable<UnitOfMeasure> p)
        {
            throw new NotImplementedException();
        }

        object IBuilder<IEnumerable<UnitOfMeasure>>.Build(IEnumerable<UnitOfMeasure> entityList, IEnumerable<string> claims)
            => this.Build(entityList, claims);
    }
}
