namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class PcasChangeResourceBuilder : IBuilder<PcasChange>
    {
        public PcasChangeResource Build(PcasChange model, IEnumerable<string> claims)
        {
            return new PcasChangeResource
                       {
                           ChangeId = model.ChangeId,
                           DocumentType = model.DocumentType,
                           DocumentNumber = model.DocumentNumber,
                           ChangeState = model.ChangeState,
                           BoardCode = model.BoardCode,
                           RevisionCode = model.RevisionCode,
                           DateEntered = model.DateEntered.ToString("o"),
                           DateApplied = model.DateApplied.HasValue ? model.DateApplied.Value.ToString("o") : null,
                           DateCancelled = model.DateCancelled.HasValue ? model.DateCancelled.Value.ToString("o") : null,
                       };
        }

        public string GetLocation(PcasChange model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<PcasChange>.Build(PcasChange entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
