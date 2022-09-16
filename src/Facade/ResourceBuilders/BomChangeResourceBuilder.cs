﻿namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class BomChangeResourceBuilder : IBuilder<BomChange>
    {
        public BomChangeResource Build(BomChange model, IEnumerable<string> claims)
        {
            return new BomChangeResource
                       {
                           DocumentType = model.DocumentType,
                           DocumentNumber = model.DocumentNumber,
                           ChangeState = model.ChangeState,
                           BomName = model.BomName,
                           PartNumber = model.PartNumber,
                           DateEntered = model.DateEntered.ToString("o"),
                           DateApplied = model.DateApplied.HasValue ? model.DateApplied.Value.ToString("o") : null,
                           DateCancelled = model.DateCancelled.HasValue ? model.DateCancelled.Value.ToString("o") : null
            };
        }

        public string GetLocation(BomChange model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<BomChange>.Build(BomChange entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
