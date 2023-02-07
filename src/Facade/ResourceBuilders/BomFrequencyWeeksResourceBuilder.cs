namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class BomFrequencyWeeksResourceBuilder : IBuilder<BomFrequencyWeeks>
    {
        public BomFrequencyWeeksResource Build(BomFrequencyWeeks model, IEnumerable<string> claims)
        {
            if (model == null)
            {
                return new BomFrequencyWeeksResource { Links = this.BuildLinks(null, claims).ToArray() };
            }

            var resource = new BomFrequencyWeeksResource
            {
                FreqWeeks = model.FreqWeeks,
                PartNumber = model.PartNumber,
                Links = this.BuildLinks(model, claims).ToArray()
            };

            return resource;
        }

        public string GetLocation(BomFrequencyWeeks model)
        {
            return $"/purchasing/bom-frequency-weeks/{model.PartNumber}";
        }

        object IBuilder<BomFrequencyWeeks>.Build(BomFrequencyWeeks entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(BomFrequencyWeeks model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }
        }
    }
}
