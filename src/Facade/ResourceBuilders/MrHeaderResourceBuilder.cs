namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Resources.MaterialRequirements;

    public class MrHeaderResourceBuilder : IBuilder<MrHeader>
    {
        private readonly IBuilder<MrDetail> detailBuilder;

        public MrHeaderResourceBuilder(IBuilder<MrDetail> detailBuilder)
        {
            this.detailBuilder = detailBuilder;
        }

        public MrHeaderResource Build(MrHeader entity, IEnumerable<string> claims)
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
                           MrDetails = entity.MrDetails?.Select(d => (MrDetailResource)this.detailBuilder.Build(d, claims)),
                           Links = this.BuildLinks(entity, claims).ToArray()
            };
        }

        public string GetLocation(MrHeader entity)
        {
            throw new NotImplementedException();
        }

        object IBuilder<MrHeader>.Build(MrHeader entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(MrHeader entity, IEnumerable<string> claims)
        {
            if (entity != null)
            {
                yield return new LinkResource { Rel = "part-used-on", Href = $"/purchasing/material-requirements/used-on-report?partNumber={entity.PartNumber}" };
            }
        }
    }
}
