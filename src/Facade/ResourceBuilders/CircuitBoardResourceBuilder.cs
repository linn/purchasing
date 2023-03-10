namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources.Boms;

    public class CircuitBoardResourceBuilder : IBuilder<CircuitBoard>
    {
        private readonly IAuthorisationService authService;

        public CircuitBoardResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public CircuitBoardResource Build(CircuitBoard entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new CircuitBoardResource { Links = this.BuildLinks(null, claims.ToList()).ToArray() };
            }

            return new CircuitBoardResource
                       {
                           BoardCode = entity.BoardCode,
                           Description = entity.Description,
                           ChangeId = entity.ChangeId,
                           ChangeState = entity.ChangeState,
                           SplitBom = entity.SplitBom,
                           DefaultPcbNumber = entity.DefaultPcbNumber,
                           VariantOfBoardCode = entity.VariantOfBoardCode,
                           LoadDirectory = entity.LoadDirectory,
                           BoardsPerSheet = entity.BoardsPerSheet,
                           CoreBoard = entity.CoreBoard,
                           ClusterBoard = entity.ClusterBoard,
                           IdBoard = entity.IdBoard,
                           Layouts = entity.Layouts?.OrderBy(a => a.LayoutSequence).Select(MakeLayoutResource),
                           Components = entity.Components?.Select(this.MakeComponentResource),
                           Links = this.BuildLinks(entity, claims?.ToList()).ToArray()
                       };
        }

        public string GetLocation(CircuitBoard entity)
        {
            return $"/purchasing/boms/boards/{entity.BoardCode}";
        }

        object IBuilder<CircuitBoard>.Build(CircuitBoard entity, IEnumerable<string> claims)
        {
            return this.Build(entity, claims);
        }

        private static BoardLayoutResource MakeLayoutResource(BoardLayout layout)
        {
            return new BoardLayoutResource
                       {
                           BoardCode = layout.BoardCode,
                           LayoutCode = layout.LayoutCode,
                           LayoutSequence = layout.LayoutSequence,
                           PcbNumber = layout.PcbNumber,
                           LayoutType = layout.LayoutType,
                           LayoutNumber = layout.LayoutNumber,
                           PcbPartNumber = layout.PcbPartNumber,
                           ChangeId = layout.ChangeId,
                           ChangeState = layout.ChangeState,
                           Revisions = layout.Revisions?.OrderBy(a => a.RevisionNumber).Select(MakeRevisionResource)
                       };
        }

        private static BoardRevisionResource MakeRevisionResource(BoardRevision revision)
        {
            return new BoardRevisionResource
                       {
                           BoardCode = revision.BoardCode,
                           LayoutCode = revision.LayoutCode,
                           RevisionCode = revision.RevisionCode,
                           LayoutSequence = revision.LayoutSequence,
                           VersionNumber = revision.VersionNumber,
                           RevisionType = MakeRevisionTypeResource(revision.RevisionType),
                           RevisionNumber = revision.RevisionNumber,
                           SplitBom = revision.SplitBom,
                           PcasPartNumber = revision.PcasPartNumber,
                           PcsmPartNumber = revision.PcsmPartNumber,
                           PcbPartNumber = revision.PcbPartNumber,
                           AteTestCommissioned = revision.AteTestCommissioned,
                           ChangeId = revision.ChangeId,
                           ChangeState = revision.ChangeState
                       };
        }

        private static BoardRevisionTypeResource MakeRevisionTypeResource(BoardRevisionType revisionType)
        {
            if (revisionType is null)
            {
                return null;
            }

            return new BoardRevisionTypeResource
                       {
                           TypeCode = revisionType.TypeCode,
                           Description = revisionType.Description,
                           ReferenceRevision = revisionType.ReferenceRevision,
                           ShowLayoutCode = revisionType.ShowLayoutCode,
                           RevisionCode = revisionType.RevisionCode,
                           ShowRevisionNumber = revisionType.ShowRevisionNumber,
                           DefaultLayoutType = revisionType.DefaultLayoutType,
                           DateObsolete = revisionType.DateObsolete?.ToString("o"),
                           RevisionSuffix = revisionType.RevisionSuffix
                       };
        }

        private BoardComponentResource MakeComponentResource(BoardComponent boardComponent)
        {
            if (boardComponent is null)
            {
                return null;
            }

            return new BoardComponentResource
                       {
                           BoardCode = boardComponent.BoardCode,
                           BoardLine = boardComponent.BoardLine,
                           CRef = boardComponent.CRef,
                           PartNumber = boardComponent.PartNumber,
                           AssemblyTechnology = boardComponent.AssemblyTechnology,
                           ChangeState = boardComponent.ChangeState,
                           FromLayoutVersion = boardComponent.FromLayoutVersion,
                           FromRevisionVersion = boardComponent.FromRevisionVersion,
                           ToLayoutVersion = boardComponent.ToLayoutVersion,
                           ToRevisionVersion = boardComponent.ToRevisionVersion,
                           AddChangeId = boardComponent.AddChangeId,
                           DeleteChangeId = boardComponent.DeleteChangeId,
                           Quantity = boardComponent.Quantity,
                           AddChangeDocumentType = boardComponent.AddChange.DocumentType,
                           AddChangeDocumentNumber = boardComponent.AddChange.DocumentNumber,
                           DeleteChangeDocumentType = boardComponent.DeleteChange?.DocumentType,
                           DeleteChangeDocumentNumber = boardComponent.DeleteChange?.DocumentNumber,
                           DeleteChangeState = boardComponent.DeleteChange?.ChangeState
                       };
        }

        private IEnumerable<LinkResource> BuildLinks(CircuitBoard model, IList<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

                if (this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, claims))
                {
                    yield return new LinkResource { Rel = "edit", Href = $"/purchasing/boms/boards/{model.BoardCode}" };
                    yield return new LinkResource { Rel = "edit-components", Href = $"/purchasing/boms/board-components/{model.BoardCode}" };
                }
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, claims))
            {
                yield return new LinkResource { Rel = "create", Href = "/purchasing/boms/boards/create" };
            }
        }
    }
}
