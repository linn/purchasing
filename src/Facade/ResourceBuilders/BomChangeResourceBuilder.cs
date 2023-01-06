namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class BomChangeResourceBuilder : IBuilder<BomChange>
    {
        public BomChangeResource Build(BomChange model, IEnumerable<string> claims)
        {
            return new BomChangeResource
                       {
                           ChangeId = model.ChangeId,
                           DocumentType = model.DocumentType,
                           DocumentNumber = model.DocumentNumber,
                           ChangeState = model.ChangeState,
                           BomName = model.BomName,
                           PartNumber = model.PartNumber,
                           DateEntered = model.DateEntered.ToString("o"),
                           DateApplied = model.DateApplied.HasValue ? model.DateApplied.Value.ToString("o") : null,
                           DateCancelled = model.DateCancelled.HasValue ? model.DateCancelled.Value.ToString("o") : null,
                           PhaseInWeekNumber = model.PhaseInWeekNumber,
                           PhaseInWWYYYY = model.PhaseInWeek == null ? string.Empty : model.PhaseInWeek.WwYyyy,
                           LifecycleText = model.LifecycleText(),
                           BomChangeDetails = model.BomChangeDetails()
            };
        }

        public string GetLocation(BomChange model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<BomChange>.Build(BomChange entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
