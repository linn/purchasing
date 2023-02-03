namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class CircuitBoardService : ICircuitBoardService
    {
        private readonly IRepository<ChangeRequest, int> changeRequestRepository;

        private readonly IRepository<CircuitBoard, string> boardRepository;

        private readonly IQueryRepository<Part> partRepository;

        public CircuitBoardService(
            IRepository<ChangeRequest, int> changeRequestRepository,
            IRepository<CircuitBoard, string> boardRepository,
            IQueryRepository<Part> partRepository)
        {
            this.changeRequestRepository = changeRequestRepository;
            this.boardRepository = boardRepository;
            this.partRepository = partRepository;
        }

        public CircuitBoard UpdateComponents(
            string boardCode,
            PcasChange pcasChange,
            int changeRequestId,
            IEnumerable<BoardComponent> componentsToAdd,
            IEnumerable<BoardComponent> componentsToRemove)
        {
            var board = this.boardRepository.FindById(boardCode);
            if (board == null)
            {
                throw new ItemNotFoundException($"Could not find board {boardCode}");
            }

            var changeRequest = this.changeRequestRepository.FindById(changeRequestId);
            if (changeRequest == null)
            {
                throw new ItemNotFoundException($"Could not find change request {changeRequestId}");
            }

            if (pcasChange.ChangeState is null)
            {
                pcasChange.ChangeRequest = changeRequest;
                pcasChange.ChangeState = changeRequest.ChangeState;
                pcasChange.DocumentNumber = changeRequestId;
                pcasChange.DocumentType = changeRequest.DocumentType;
            }

            var revision = board.Layouts.SelectMany(a => a.Revisions).First(r => r.RevisionCode == changeRequest.RevisionCode);

            if (componentsToRemove != null)
            {
                foreach (var boardComponent in componentsToRemove)
                {
                    var component = board.Components.First(a => a.BoardLine == boardComponent.BoardLine);
                    if (component == null)
                    {
                        throw new ItemNotFoundException(
                            $"Could not find component with board line {boardComponent.BoardLine} to remove");
                    }

                    if (component.DeleteChangeId.HasValue
                        && component.DeleteChangeId.Value != pcasChange.ChangeId)
                    {
                        throw new InvalidActionException(
                            $"Cannot remove board line {component.BoardLine} from {boardCode} as it is already being removed on change {component.DeleteChangeId}");
                    }

                    if (component.AddChangeId == pcasChange.ChangeId && component.ChangeState != "LIVE")
                    {
                        board.Components.Remove(component);
                    }
                    else
                    {
                        component.DeleteChangeId = pcasChange.ChangeId;
                        if (component.FromLayoutVersion != component.ToLayoutVersion
                            || component.FromRevisionVersion != component.ToRevisionVersion)
                        {
                            this.MaybeAddComponentPriorToCrfRevision(board, component, revision, pcasChange);
                            this.MaybeAddComponentAfterCrfRevision(board, component, revision, pcasChange);
                        }
                    }
                }
            }

            if (componentsToAdd != null)
            {
                foreach (var boardComponent in componentsToAdd)
                {
                    if (string.IsNullOrWhiteSpace(boardComponent.PartNumber)
                        || string.IsNullOrWhiteSpace(boardComponent.CRef) || boardComponent.Quantity == 0)
                    {
                        throw new InvalidOptionException(
                            $"Component at line {boardComponent.BoardLine} cRef {boardComponent.CRef} is malformed");
                    }

                    var part = this.partRepository.FindBy(a => a.PartNumber == boardComponent.PartNumber.ToUpper());
                    boardComponent.AddChangeId = pcasChange.ChangeId;
                    boardComponent.AssemblyTechnology = part.AssemblyTechnology;
                    boardComponent.FromLayoutVersion = revision.LayoutSequence;
                    boardComponent.FromRevisionVersion = revision.VersionNumber;
                    boardComponent.ChangeState = changeRequest.ChangeState;

                    if (this.LatestLayout(board, revision) && this.LatestVersion(board, revision))
                    {
                        boardComponent.ToRevisionVersion = null;
                        boardComponent.ToLayoutVersion = null;
                    }
                    else if (this.LatestLayout(board, revision) && !this.LatestVersion(board, revision))
                    {
                        boardComponent.ToRevisionVersion = revision.VersionNumber;
                        boardComponent.ToLayoutVersion = revision.LayoutSequence;
                    }
                    else if (!this.LatestLayout(board, revision) && this.LatestVersion(board, revision))
                    {
                        boardComponent.ToRevisionVersion = null;
                        boardComponent.ToLayoutVersion = revision.LayoutSequence;
                    }

                    board.Components.Add(boardComponent);
                }
            }

            return board;
        }

        public ProcessResult UpdateFromFile(string boardCode, string revisionCode, string fileType, string fileString, bool makeChanges)
        {
            if (fileType != "TSB")
            {
                throw new InvalidOptionException(
                    $"File type {fileType} has no supporting strategy and cannot be processed");
            }

            var board = this.boardRepository.FindById(boardCode);
            var revision = board.Layouts.SelectMany(a => a.Revisions).First(a => a.RevisionCode == revisionCode);
            var strategy = new TabSeparatedReadStrategy();
            var (fileContents, pcbPartNumber) = strategy.ReadFile(fileString);

            var message = string.Empty;
            if (revision.PcbPartNumber != pcbPartNumber)
            {
                message =
                    $"Pcb part number on revision is {revision.PcbPartNumber} but found {pcbPartNumber} in the file. \n";
            }

            var existingComponents = board.ComponentsOnRevision(revision.LayoutSequence, revision.VersionNumber);
            foreach (var fileComponent in fileContents)
            {
                var existing = existingComponents.FirstOrDefault(a => a.CRef == fileComponent.CRef);
                if (existing == null)
                {
                    message += $"Adding {fileComponent.PartNumber} at {fileComponent.CRef}. \n";
                } 
                else if (fileComponent.PartNumber != existing.PartNumber)
                {
                    message += $"Replacing {existing.PartNumber} with {fileComponent.PartNumber} at {existing.CRef}. \n";
                }
            }

            var missingComponents = existingComponents.Select(a => a.CRef).Except(fileContents.Select(b => b.CRef));
            foreach (var cRef in missingComponents)
            {
                message += $"Removing {existingComponents.First(a => a.CRef == cRef).PartNumber} from {cRef}. \n";
            }

            return new ProcessResult(true, message);
        }

        private void MaybeAddComponentPriorToCrfRevision(
            CircuitBoard board,
            BoardComponent component,
            BoardRevision revision,
            PcasChange pcasChange)
        {
            if (component.FromLayoutVersion != revision.LayoutSequence
                || component.FromRevisionVersion != revision.VersionNumber)
            {
                var (layout, version) = this.GetPreviousLayoutAndRevision(revision.LayoutSequence, revision.VersionNumber);
                board.Components.Add(new BoardComponent
                                         {
                                             BoardCode = board.BoardCode,
                                             BoardLine = board.Components.Max(a => a.BoardLine) + 1,
                                             CRef = component.CRef,
                                             PartNumber = component.PartNumber,
                                             AssemblyTechnology = component.AssemblyTechnology,
                                             ChangeState = pcasChange.ChangeState,
                                             FromLayoutVersion = component.FromLayoutVersion,
                                             FromRevisionVersion = component.FromRevisionVersion,
                                             ToLayoutVersion = layout,
                                             ToRevisionVersion = version,
                                             AddChangeId = pcasChange.ChangeId,
                                             AddChange = pcasChange,
                                             DeleteChangeId = null,
                                             DeleteChange = null,
                                             Quantity = component.Quantity
                                         });
            }
        }

        private void MaybeAddComponentAfterCrfRevision(
            CircuitBoard board,
            BoardComponent component,
            BoardRevision revision,
            PcasChange pcasChange)
        {
            if (component.ToRevisionVersion != revision.LayoutSequence
                || component.ToRevisionVersion!= revision.VersionNumber)
            {
                if (!this.LatestLayout(board, revision) || !this.LatestVersion(board, revision))
                {
                    var (layout, version) = this.GetNextLayoutAndRevision(
                        revision.LayoutSequence,
                        revision.VersionNumber,
                        board.Layouts.First(a => a.LayoutCode == revision.LayoutCode).Revisions
                            .Max(r => r.VersionNumber));
                    board.Components.Add(new BoardComponent
                                             {
                                                 BoardCode = board.BoardCode,
                                                 BoardLine = board.Components.Max(a => a.BoardLine) + 1,
                                                 CRef = component.CRef,
                                                 PartNumber = component.PartNumber,
                                                 AssemblyTechnology = component.AssemblyTechnology,
                                                 ChangeState = pcasChange.ChangeState,
                                                 FromLayoutVersion = layout,
                                                 FromRevisionVersion = version,
                                                 ToLayoutVersion = component.ToLayoutVersion,
                                                 ToRevisionVersion = component.ToRevisionVersion,
                                                 AddChangeId = pcasChange.ChangeId,
                                                 AddChange = pcasChange,
                                                 DeleteChangeId = null,
                                                 DeleteChange = null,
                                                 Quantity = component.Quantity
                                             });
                }
            }
        }

        private (int layout, int? version) GetPreviousLayoutAndRevision(int revisionLayoutSequence, int revisionVersionNumber)
        {
            if (revisionVersionNumber == 1)
            {
                return (revisionLayoutSequence - 1, null);
            } 

            return (revisionLayoutSequence, revisionVersionNumber - 1);
        }

        private (int layout, int version) GetNextLayoutAndRevision(int revisionLayoutSequence, int revisionVersionNumber, int maxVersionNumber)
        {
            if (revisionVersionNumber == maxVersionNumber)
            {
                return (revisionLayoutSequence + 1, 1);
            }

            return (revisionLayoutSequence, revisionVersionNumber + 1);
        }

        private bool LatestLayout(CircuitBoard board, BoardRevision revision)
        {
            var maxLayoutSequence = board.Layouts.Max(a => a.LayoutSequence);
            return revision.LayoutSequence == maxLayoutSequence;
        }

        private bool LatestVersion(CircuitBoard board, BoardRevision revision)
        {
            var maxVersionNumber = board.Layouts
                .First(a => a.LayoutCode == revision.LayoutCode).Revisions
                .Max(r => r.VersionNumber);
            return revision.VersionNumber == maxVersionNumber;
        }
    }
}
