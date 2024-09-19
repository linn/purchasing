namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BoardComponentSummaryResourceBuilder : IBuilder<BoardComponentSummary>
    {
        public BoardComponentSummaryResource Build(BoardComponentSummary entity, IEnumerable<string> claims)
        {
            return new BoardComponentSummaryResource
                       {
                           BoardCode = entity.BoardCode,
                           RevisionCode = entity.RevisionCode,
                           Cref = entity.Cref,
                           PartNumber = entity.PartNumber,
                           AssemblyTechnology = entity.AssemblyTechnology,
                           Quantity = entity.Quantity,
                           BoardLine = entity.BoardLine,
                           ChangeState = entity.ChangeState,
                           AddChangeId = entity.AddChangeId,
                           DeleteChangeId = entity.DeleteChangeId,
                           DeleteChangeRequest = entity.DeleteChangeRequest,
                           FromLayoutVersion = entity.FromLayoutVersion,
                           FromRevisionVersion = entity.FromRevisionVersion,
                           ToLayoutVersion = entity.ToLayoutVersion,
                           ToRevisionVersion = entity.ToRevisionVersion,
                           LayoutSequence = entity.LayoutSequence,
                           VersionNumber = entity.VersionNumber,
                           BomPartNumber = entity.BomPartNumber,
                           PcasPartNumber = entity.PcasPartNumber,
                           PcsmPartNumber = entity.PcsmPartNumber,
                           PcbPartNumber = entity.PcbPartNumber,
                           PartDescription = entity.PartDescription
                       };
        }

        public string GetLocation(BoardComponentSummary model)
        {
            throw new System.NotImplementedException();
        }

        object IBuilder<BoardComponentSummary>.Build(BoardComponentSummary entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }
    }
}
