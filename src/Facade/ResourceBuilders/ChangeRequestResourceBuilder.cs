namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class ChangeRequestResourceBuilder : IBuilder<ChangeRequest>
    {
        private readonly IBuilder<BomChange> bomChangeBuilder;

        public ChangeRequestResourceBuilder(IBuilder<BomChange> bomChangeBuilder)
        {
            this.bomChangeBuilder = bomChangeBuilder;
        }

        public ChangeRequestResource Build(ChangeRequest model, IEnumerable<string> claims)
        {
            return new ChangeRequestResource
                       {
                           DocumentType = model.DocumentType, 
                           DocumentNumber = model.DocumentNumber, 
                           ChangeState = model.ChangeState, 
                           ReasonForChange = model.ReasonForChange, 
                           DescriptionOfChange = model.DescriptionOfChange,
                           DateEntered = model.DateEntered.ToString("o"),
                           BomChanges =
                               model.BomChanges?.Select(
                                   d => (BomChangeResource)this.bomChangeBuilder.Build(d, claims)),
            };
        }

        public string GetLocation(ChangeRequest model)
        {
            throw new NotImplementedException();
        }

        object IBuilder<ChangeRequest>.Build(ChangeRequest entity, IEnumerable<string> claims) => this.Build(entity, claims);
    }
}
