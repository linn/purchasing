namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class LedgerPeriodResourceBuilder : IBuilder<LedgerPeriod>
    {
        public LedgerPeriodResource Build(LedgerPeriod entity, IEnumerable<string> claims)
        {
            return new LedgerPeriodResource
                       {
                           PeriodNumber = entity.PeriodNumber,
                           MonthName = entity.MonthName,
                           Links = this.BuildLinks(entity, claims).ToArray()
                       };
        }

        public string GetLocation(LedgerPeriod entity)
        {
            return $"/purchasing/ledger-periods/{entity.PeriodNumber}";
        }

        object IBuilder<LedgerPeriod>.Build(LedgerPeriod entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private IEnumerable<LinkResource> BuildLinks(LedgerPeriod entity, IEnumerable<string> claims)
        {
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(entity) };
        }
    }
}
