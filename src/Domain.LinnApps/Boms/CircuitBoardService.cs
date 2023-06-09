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
            var board = this.GetCircuitBoard(boardCode);

            var changeRequest = this.GetChangeRequestAndMaybePopulatePcasChange(pcasChange, changeRequestId);

            var revision = board.Layouts.SelectMany(a => a.Revisions).First(r => r.RevisionCode == changeRequest.RevisionCode);

            if (componentsToAdd != null)
            {
                foreach (var boardComponent in componentsToAdd)
                {
                    if (string.IsNullOrWhiteSpace(boardComponent.PartNumber) ||
                        string.IsNullOrWhiteSpace(boardComponent.CRef))
                    {
                        throw new InvalidOptionException($"Part or Cref missing - {boardComponent.PartNumber} at {boardComponent.CRef}");
                    }

                    var part = this.partRepository.FindBy(a => a.PartNumber == boardComponent.PartNumber.ToUpper());
                    if (part == null)
                    {
                        throw new ItemNotFoundException($"Could not find part {boardComponent.PartNumber.ToUpper()}");
                    }

                    this.AddComponent(board, revision, boardComponent, part, pcasChange);
                }
            }

            if (componentsToRemove != null)
            {
                foreach (var boardComponent in componentsToRemove)
                {
                    this.RemoveComponent(board, revision, boardComponent, pcasChange);
                }
            }

            return board;
        }

        public ProcessResult UpdateFromFile(
            string boardCode,
            string revisionCode,
            string fileType,
            string fileString,
            PcasChange pcasChange,
            bool makeChanges)
        {
            if (fileType != "TSB" && fileType != "SMT")
            {
                throw new InvalidOptionException($"File type {fileType} has no supporting strategy and cannot be processed");
            }

            var board = this.GetCircuitBoard(boardCode);
            if (makeChanges && pcasChange != null)
            {
                this.GetChangeRequestAndMaybePopulatePcasChange(pcasChange, pcasChange.DocumentNumber);
            }

            var revision = board.Layouts.SelectMany(a => a.Revisions).FirstOrDefault(a => a.RevisionCode == revisionCode);
            if (revision == null)
            {
                throw new ItemNotFoundException($"Could not find revision {revisionCode} on board {boardCode}");
            }

            IBoardFileReadStrategy strategy = fileType == "TSB" ? new TabSeparatedReadStrategy() : new SmtFileReadStrategy();

            var (fileContents, pcbPartNumber) = strategy.ReadFile(fileString);

            var message = makeChanges
                              ? $"THE FOLLOWING CHANGES HAVE BEEN MADE FOR BOARD {boardCode} revision {revisionCode} \n\n"
                              : $"Differences found in file against board {boardCode} revision {revisionCode} \n\n";
            var changeCounter = 0;

            if (fileType == "TSB" && revision.PcbPartNumber != pcbPartNumber)
            {
                message += $"Pcb part number on revision is {revision.PcbPartNumber} but found {pcbPartNumber} in the file. \n";
                changeCounter++;
                if (makeChanges)
                {
                    revision.PcbPartNumber = pcbPartNumber;
                }
            }

            var componentsOnRevision = board.ComponentsOnRevision(revision.LayoutSequence, revision.VersionNumber);
            if (fileType == "SMT")
            {
                componentsOnRevision = componentsOnRevision.Where(a => a.AssemblyTechnology == "SM").ToList();
            }

            foreach (var fileComponent in fileContents)
            {
                var existing = componentsOnRevision.FirstOrDefault(a => a.CRef == fileComponent.CRef);
                
                Part part = null;
                if (fileType != "SMT")
                {
                    part = this.partRepository.FindBy(a => a.PartNumber == fileComponent.PartNumber.ToUpper());

                    if (part == null)
                    {
                        message += $"******* ERROR {fileComponent.PartNumber} at {fileComponent.CRef} is not valid.  ******* \n";
                    }
                }

                if (string.IsNullOrWhiteSpace(fileComponent.CRef))
                {
                    message += $"******* ERROR \"{fileComponent.CRef}\" is not a valid Cref. Part on file line was {fileComponent.PartNumber}  ******* \n";
                }

                if (existing == null)
                {
                    message += this.AddToMessage(fileType, "add", fileComponent.CRef, fileComponent.PartNumber);
                    changeCounter++;
                    if (makeChanges && part!= null && !string.IsNullOrWhiteSpace(fileComponent.CRef))
                    {
                        this.AddComponent(board, revision, fileComponent, part, pcasChange);
                    }
                } 
                else if (fileComponent.PartNumber != existing.PartNumber)
                {
                    message += this.AddToMessage(fileType, "replace", fileComponent.CRef, fileComponent.PartNumber, existing.PartNumber);
                    changeCounter++;
                    if (makeChanges && part != null && !string.IsNullOrWhiteSpace(fileComponent.CRef))
                    {
                        this.RemoveComponent(board, revision, existing, pcasChange);
                        this.AddComponent(board, revision, fileComponent, part, pcasChange);
                    }
                }
            }

            var missingComponents = componentsOnRevision.Select(a => a.CRef).Except(fileContents.Select(b => b.CRef));
            foreach (var cRef in missingComponents)
            {
                var componentToRemove = componentsOnRevision.First(a => a.CRef == cRef);
                changeCounter++;
                message += AddToMessage(fileType, "remove", cRef, null, componentToRemove.PartNumber);
                if (makeChanges)
                {
                    this.RemoveComponent(board, revision, componentToRemove, pcasChange);
                }
            }

            if (changeCounter == 0)
            {
                message += "No changes found in selected file. \n";
            }

            return new ProcessResult(true, message);
        }

        private string AddToMessage(
            string fileType,
            string messageType,
            string cRef,
            string filePartNumber,
            string existingPartNumber = null)
        {
            if (fileType == "SMT")
            {
                switch (messageType)
                {
                    case "add":
                        return $"{cRef, -20}- PC File = {filePartNumber, -15} Database = _______________\n";
                    case "remove":
                        return $"{cRef, -20}- PC File = _______________ Database = {existingPartNumber}\n";
                    case "replace":
                        return $"{cRef, -20}- PC File = {filePartNumber,-15} Database = {existingPartNumber}\n";
                    default:
                        return null;
                }
            }

            switch (messageType)
            {
                case "add":
                    return $"Adding {filePartNumber} at {cRef}. \n";
                case "replace":
                    return $"Replacing {existingPartNumber} with {filePartNumber} at {cRef}. \n";
                case "remove":
                    return $"Removing {existingPartNumber} from {cRef}. \n";
                default:
                    return null;
            }
        }

        private CircuitBoard GetCircuitBoard(string boardCode)
        {
            var board = this.boardRepository.FindById(boardCode);
            if (board == null)
            {
                throw new ItemNotFoundException($"Could not find board {boardCode}");
            }

            return board;
        }

        private ChangeRequest GetChangeRequestAndMaybePopulatePcasChange(PcasChange pcasChange, int changeRequestId)
        {
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

            return changeRequest;
        }

        private void AddComponent(
            CircuitBoard board,
            BoardRevision revision,
            BoardComponent boardComponent,
            Part part,
            PcasChange pcasChange)
        {
            if (string.IsNullOrWhiteSpace(boardComponent.PartNumber) || string.IsNullOrWhiteSpace(boardComponent.CRef)
                                                                     || boardComponent.Quantity == 0)
            {
                throw new InvalidOptionException(
                    $"Component at line {boardComponent.BoardLine} cRef {boardComponent.CRef} is malformed");
            }

            boardComponent.Part = part;
            boardComponent.AddChangeId = pcasChange.ChangeId;
            boardComponent.AssemblyTechnology = part.AssemblyTechnology;
            boardComponent.FromLayoutVersion = revision.LayoutSequence;
            boardComponent.FromRevisionVersion = revision.VersionNumber;
            boardComponent.ChangeState = pcasChange.ChangeState;
            if (boardComponent.BoardLine == 0 && board.Components.Count > 0)
            {
                boardComponent.BoardLine = board.Components.Max(a => a.BoardLine) + 1;
            }

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

        private void RemoveComponent(
            CircuitBoard board,
            BoardRevision revision,
            BoardComponent boardComponent,
            PcasChange pcasChange)
        {
            var component = board.Components.FirstOrDefault(a => a.BoardLine == boardComponent.BoardLine);
            if (component == null)
            {
                throw new ItemNotFoundException(
                    $"Could not find component with board line {boardComponent.BoardLine} to remove");
            }

            if (component.DeleteChangeId.HasValue && component.DeleteChangeId.Value != pcasChange.ChangeId)
            {
                throw new InvalidActionException(
                    $"Cannot remove board line {component.BoardLine} from {board.BoardCode} as it is already being removed on change {component.DeleteChangeId}");
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
                                             Quantity = component.Quantity,
                                             Part = component.Part,
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
                                                 Quantity = component.Quantity,
                                                 Part = component.Part,
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
