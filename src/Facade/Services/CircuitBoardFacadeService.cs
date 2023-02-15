namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Facade.Extensions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.Boms;

    public class CircuitBoardFacadeService :
        FacadeResourceService<CircuitBoard, string, CircuitBoardResource, CircuitBoardComponentsUpdateResource>,
        ICircuitBoardFacadeService
    {
        private readonly ITransactionManager transactionManager;

        private readonly IBuilder<CircuitBoard> resourceBuilder;

        private readonly IRepository<PcasChange, int> pcasChangeRepository;

        private readonly ICircuitBoardService circuitBoardService;

        private readonly IDatabaseService databaseService;

        private readonly IPcasPack pcasPack;

        private readonly IQueryable<BoardRevisionType> types;

        public CircuitBoardFacadeService(
            IRepository<CircuitBoard, string> repository,
            ITransactionManager transactionManager,
            IBuilder<CircuitBoard> resourceBuilder,
            IRepository<BoardRevisionType, string> boardRevisionTypeRepository,
            IRepository<PcasChange, int> pcasChangeRepository,
            ICircuitBoardService circuitBoardService,
            IDatabaseService databaseService,
            IPcasPack pcasPack)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.transactionManager = transactionManager;
            this.resourceBuilder = resourceBuilder;
            this.pcasChangeRepository = pcasChangeRepository;
            this.circuitBoardService = circuitBoardService;
            this.databaseService = databaseService;
            this.pcasPack = pcasPack;
            this.types = boardRevisionTypeRepository.FindAll();
        }

        public IResult<CircuitBoardResource> UpdateBoardComponents(
            string id,
            CircuitBoardComponentsUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            var pcasChange = this.GetPcasChangeDetails(
                id,
                updateResource.ChangeRequestId,
                updateResource.ChangeRequestRevisionCode,
                updateResource.UserNumber);

            CircuitBoard result;
            try
            {
                result = this.circuitBoardService.UpdateComponents(
                    id,
                    pcasChange,
                    updateResource.ChangeRequestId,
                    updateResource.Components.Where(a => a.Adding == true).Select(c => c.ToDomain()),
                    updateResource.Components.Where(b => b.Removing == true).Select(c => c.ToDomain()));
            }
            catch (DomainException e)
            {
                return new BadRequestResult<CircuitBoardResource>(e.Message);
            }

            var discrepancies = this.pcasPack.DiscrepanciesOnChange(
                id,
                updateResource.ChangeRequestRevisionCode,
                pcasChange.ChangeId);

            var resource = (CircuitBoardResource)this.resourceBuilder.Build(result, privileges);
            resource.Discrepancies = discrepancies;

            this.transactionManager.Commit();

            return new SuccessResult<CircuitBoardResource>(resource);
        }

        public IResult<ProcessResultResource> UploadBoardFile(
            string boardCode,
            string revisionCode,
            string fileType,
            string fileString,
            int? changeRequestId,
            bool makeChanges,
            IEnumerable<string> getPrivileges)
        {
            PcasChange pcasChange = null;
            if (makeChanges && changeRequestId.HasValue)
            {
                pcasChange = this.GetPcasChangeDetails(boardCode, changeRequestId.Value, revisionCode, 100);
            }

            var result = this.circuitBoardService.UpdateFromFile(
                boardCode,
                revisionCode,
                fileType,
                fileString,
                pcasChange,
                makeChanges);

            if (makeChanges)
            {
                this.transactionManager.Commit();
            }

            return new SuccessResult<ProcessResultResource>(new ProcessResultResource(result.Success, result.Message));
        }

        protected override CircuitBoard CreateFromResource(
            CircuitBoardResource resource,
            IEnumerable<string> privileges = null)
        {
            return new CircuitBoard
                       {
                           BoardCode = resource.BoardCode.ToUpper(),
                           Description = resource.Description,
                           ChangeId = null,
                           ChangeState = "LIVE",
                           SplitBom = resource.SplitBom,
                           DefaultPcbNumber = resource.DefaultPcbNumber?.ToUpper(),
                           VariantOfBoardCode = resource.VariantOfBoardCode,
                           LoadDirectory = resource.LoadDirectory,
                           BoardsPerSheet = resource.BoardsPerSheet,
                           CoreBoard = resource.CoreBoard,
                           ClusterBoard = resource.ClusterBoard,
                           IdBoard = resource.IdBoard,
                           Layouts = resource.Layouts
                               ?.Select(l => this.MakeLayout(l, resource.DefaultPcbNumber?.ToUpper())).ToList()
                       };
        }

        protected override void UpdateFromResource(
            CircuitBoard entity,
            CircuitBoardComponentsUpdateResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.ClusterBoard = updateResource.ClusterBoard;
            entity.CoreBoard = updateResource.CoreBoard;
            entity.IdBoard = updateResource.IdBoard;
            entity.Description = updateResource.Description;
            entity.DefaultPcbNumber = updateResource.DefaultPcbNumber?.ToUpper();
            entity.SplitBom = updateResource.SplitBom;
            entity.VariantOfBoardCode = updateResource.VariantOfBoardCode;
            if (entity.Layouts is null)
            {
                entity.Layouts = new List<BoardLayout>();
            }

            this.UpdateLayouts(
                entity.Layouts,
                updateResource.Layouts?.ToList(),
                updateResource.DefaultPcbNumber?.ToUpper());
        }

        protected override Expression<Func<CircuitBoard, bool>> SearchExpression(string searchTerm)
        {
            return a => a.BoardCode.Contains(searchTerm) || a.Description.Contains(searchTerm);
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            CircuitBoard entity,
            CircuitBoardResource resource,
            CircuitBoardComponentsUpdateResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(CircuitBoard entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        private PcasChange GetPcasChangeDetails(
            string boardCode,
            int changeRequestId,
            string revisionCode,
            int userNumber)
        {
            var pcasChange = this.pcasChangeRepository.FindBy(
                a => a.BoardCode == boardCode && a.RevisionCode == revisionCode
                                       && a.ChangeRequest.DocumentNumber == changeRequestId);
            if (pcasChange == null)
            {
                var nextChangeId = this.databaseService.GetIdSequence("CHG_SEQ");

                pcasChange = new PcasChange
                                 {
                                     ChangeId = nextChangeId,
                                     BoardCode = boardCode,
                                     RevisionCode = revisionCode,
                                     ChangeRequest = null,
                                     DocumentType = null,
                                     DocumentNumber = changeRequestId,
                                     DateEntered = DateTime.Now,
                                     EnteredById = userNumber,
                                     EnteredBy = null,
                                     DateApplied = null,
                                     AppliedById = null,
                                     AppliedBy = null,
                                     DateCancelled = null,
                                     CancelledById = null,
                                     CancelledBy = null,
                                     ChangeState = null,
                                     Comments = null
                                 };
                this.pcasChangeRepository.Add(pcasChange);
            }

            return pcasChange;
        }

        private void UpdateLayouts(
            IList<BoardLayout> layouts,
            IList<BoardLayoutResource> updatedResources,
            string defaultPcbNumber)
        {
            if (layouts == null || updatedResources == null)
            {
                return;
            }

            foreach (var boardLayout in layouts)
            {
                var resource = updatedResources.First(a => a.LayoutCode == boardLayout.LayoutCode);
                updatedResources.RemoveAt(updatedResources.IndexOf(resource));
                boardLayout.BoardCode = resource.BoardCode;
                boardLayout.LayoutCode = resource.LayoutCode.ToUpper();
                boardLayout.LayoutSequence = resource.LayoutSequence;
                boardLayout.PcbNumber = string.IsNullOrEmpty(resource.PcbNumber)
                                            ? defaultPcbNumber
                                            : resource.PcbNumber.ToUpper();
                boardLayout.LayoutType = resource.LayoutType;
                boardLayout.LayoutNumber = resource.LayoutNumber;
                boardLayout.PcbPartNumber = resource.PcbPartNumber;
                boardLayout.ChangeId = resource.ChangeId;
                boardLayout.ChangeState =
                    string.IsNullOrEmpty(resource.ChangeState)
                        ? "LIVE"
                        : resource.ChangeState;
                if (boardLayout.Revisions is null)
                {
                    boardLayout.Revisions = new List<BoardRevision>();
                }

                this.UpdateRevisions(boardLayout.Revisions, resource.Revisions?.ToList());
            }

            if (updatedResources.Count > 0)
            {
                foreach (var boardLayoutResource in updatedResources)
                {
                    layouts.Add(this.MakeLayout(boardLayoutResource, defaultPcbNumber));
                }
            }
        }

        private void UpdateRevisions(IList<BoardRevision> revisions, IList<BoardRevisionResource> updatedResources)
        {
            if (revisions == null || updatedResources == null)
            {
                return;
            }

            foreach (var boardRevision in revisions)
            {
                var boardRevisionResource = updatedResources.First(a => a.RevisionCode == boardRevision.RevisionCode);
                updatedResources.RemoveAt(updatedResources.IndexOf(boardRevisionResource));

                boardRevision.BoardCode = boardRevisionResource.BoardCode;
                boardRevision.LayoutCode = boardRevisionResource.LayoutCode.ToUpper();
                boardRevision.RevisionCode = boardRevisionResource.RevisionCode.ToUpper();
                boardRevision.LayoutSequence = boardRevisionResource.LayoutSequence;
                boardRevision.VersionNumber = boardRevisionResource.VersionNumber;
                boardRevision.RevisionType = boardRevisionResource.RevisionType?.TypeCode is null
                                                 ? null
                                                 : this.types.First(
                                                     t => t.TypeCode == boardRevisionResource.RevisionType.TypeCode);
                boardRevision.RevisionNumber = boardRevisionResource.RevisionNumber;
                boardRevision.SplitBom = boardRevisionResource.SplitBom;
                boardRevision.PcasPartNumber = boardRevisionResource.PcasPartNumber?.ToUpper();
                boardRevision.PcsmPartNumber = boardRevisionResource.PcsmPartNumber?.ToUpper();
                boardRevision.PcbPartNumber = boardRevisionResource.PcbPartNumber?.ToUpper();
                boardRevision.AteTestCommissioned = boardRevisionResource.AteTestCommissioned;
                boardRevision.ChangeId = boardRevisionResource.ChangeId;
                boardRevision.ChangeState = string.IsNullOrEmpty(boardRevisionResource.ChangeState)
                                                ? "LIVE"
                                                : boardRevisionResource.ChangeState;
            }

            if (updatedResources.Count > 0)
            {
                foreach (var boardRevisionResource in updatedResources)
                {
                    revisions.Add(this.MakeRevision(boardRevisionResource));
                }
            }
        }

        private BoardLayout MakeLayout(BoardLayoutResource boardLayoutResource, string defaultPcbNumber)
        {
            return new BoardLayout
                       {
                           BoardCode = boardLayoutResource.BoardCode,
                           LayoutCode = boardLayoutResource.LayoutCode.ToUpper(),
                           LayoutSequence = boardLayoutResource.LayoutSequence,
                           PcbNumber =
                               string.IsNullOrEmpty(boardLayoutResource.PcbNumber)
                                   ? defaultPcbNumber
                                   : boardLayoutResource.PcbNumber?.ToUpper(),
                           LayoutType = boardLayoutResource.LayoutType,
                           LayoutNumber = boardLayoutResource.LayoutNumber,
                           PcbPartNumber = boardLayoutResource.PcbPartNumber,
                           ChangeId = boardLayoutResource.ChangeId,
                           ChangeState =
                               string.IsNullOrEmpty(boardLayoutResource.ChangeState)
                                   ? "LIVE"
                                   : boardLayoutResource.ChangeState,
                           Revisions = boardLayoutResource.Revisions?.Select(this.MakeRevision).ToList()
                       };
        }

        private BoardRevision MakeRevision(BoardRevisionResource boardRevisionResource)
        {
            return new BoardRevision
                       {
                           BoardCode = boardRevisionResource.BoardCode,
                           LayoutCode = boardRevisionResource.LayoutCode.ToUpper(),
                           RevisionCode = boardRevisionResource.RevisionCode.ToUpper(),
                           LayoutSequence = boardRevisionResource.LayoutSequence,
                           VersionNumber = boardRevisionResource.VersionNumber,
                           RevisionType = boardRevisionResource.RevisionType?.TypeCode is null
                                              ? null
                                              : this.types.First(t => t.TypeCode == boardRevisionResource.RevisionType.TypeCode),
            RevisionNumber = boardRevisionResource.RevisionNumber,
                           SplitBom = boardRevisionResource.SplitBom,
                           PcasPartNumber = boardRevisionResource.PcasPartNumber?.ToUpper(),
                           PcsmPartNumber = boardRevisionResource.PcsmPartNumber?.ToUpper(),
                           PcbPartNumber = boardRevisionResource.PcbPartNumber?.ToUpper(),
                           AteTestCommissioned = boardRevisionResource.AteTestCommissioned,
                           ChangeId = boardRevisionResource.ChangeId,
                           ChangeState = string.IsNullOrEmpty(boardRevisionResource.ChangeState)
                                             ? "LIVE"
                                             : boardRevisionResource.ChangeState
            };
        }
    }
}
