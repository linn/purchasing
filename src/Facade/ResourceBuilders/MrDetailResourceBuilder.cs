namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrDetailResourceBuilder : IBuilder<MrDetail>
    {
        public MrDetailResource Build(MrDetail entity, IEnumerable<string> claims)
        {
            return new MrDetailResource
                       {
                           JobRef = entity.JobRef,
                           PartNumber = entity.PartNumber,
                           LinnWeekNumber = entity.LinnWeekNumber,
                           DeliveryForecast = entity.DeliveryForecast,
                           ProductionRequirement = entity.ProductionRequirement
                       };
        }

        public string GetLocation(MrDetail entity)
        {
            throw new NotImplementedException();
        }

        object IBuilder<MrDetail>.Build(MrDetail entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
