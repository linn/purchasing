namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrReportResourceBuilder : IBuilder<MrReport>
    {
        public MrReportResource Build(MrReport entity, IEnumerable<string> claims)
        {
            return new MrReportResource
                       {
                           Results = entity.Headers.Select(h => this.BuildHeader(h, entity.RunWeekNumber))
                       };
        }

        public string GetLocation(MrReport entity)
        {
            throw new NotImplementedException();
        }

        object IBuilder<MrReport>.Build(MrReport entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private MrHeaderResource BuildHeader(MrHeader entity, int runWeekNumber)
        {
            return new MrHeaderResource
            {
                JobRef = entity.JobRef,
                PartNumber = entity.PartNumber,
                PartDescription = entity.PartDescription,
                QuantityInStock = entity.QuantityInStock,
                QuantityForSpares = entity.QuantityForSpares,
                QuantityInInspection = entity.QuantityInInspection,
                QuantityFaulty = entity.QuantityFaulty,
                QuantityAtSupplier = entity.QuantityAtSupplier,
                PreferredSupplierId = entity.PreferredSupplierId,
                PreferredSupplierName = entity.PreferredSupplierName,
                AnnualUsage = entity.AnnualUsage,
                BaseUnitPrice = entity.BaseUnitPrice,
                OurUnits = entity.OurUnits,
                OrderUnits = entity.OrderUnits,
                LeadTimeWeeks = entity.LeadTimeWeeks,
                CurrencyCode = entity.CurrencyCode,
                CurrencyUnitPrice = entity.CurrencyUnitPrice,
                MinimumOrderQuantity = entity.MinimumOrderQuantity,
                MinimumDeliveryQuantity = entity.MinimumDeliveryQuantity,
                OrderIncrement = entity.OrderIncrement,
                HasProductionRequirement = entity.HasProductionRequirement,
                HasDeliveryForecast = entity.HasDeliveryForecast,
                VendorManager = entity.VendorManager,
                VendorManagerInitials = entity.VendorManagerInitials,
                MrDetails = entity.MrDetails?.Select(d => this.BuildDetail(d, runWeekNumber)),
                Links = this.BuildHeaderLinks(entity).ToArray()
            };
        }

        private MrDetailResource BuildDetail(MrDetail detail, int runWeekNumber)
        {
            return new MrDetailResource();
        }

        private IEnumerable<LinkResource> BuildHeaderLinks(MrHeader entity)
        {
            if (entity == null)
            {
                yield break;
            }

            yield return new LinkResource
                             {
                                 Rel = "part-used-on",
                                 Href = $"/purchasing/material-requirements/used-on-report?partNumber={entity.PartNumber}"
                             };
            yield return new LinkResource { Rel = "part", Href = $"/parts/{entity.PartId}" };
        }
    }
}
