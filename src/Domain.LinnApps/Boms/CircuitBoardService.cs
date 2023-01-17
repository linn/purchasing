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

            var changeRequest = this.changeRequestRepository.FindBy(a => a.DocumentNumber == changeRequestId);
            if (changeRequest == null)
            {
                throw new ItemNotFoundException($"Could not find change request {changeRequestId}");
            }

            if (pcasChange.ChangeRequest is null)
            {
                pcasChange.ChangeRequest = changeRequest;
                pcasChange.ChangeState = changeRequest.ChangeState;
                pcasChange.DocumentNumber = changeRequestId;
                pcasChange.DocumentType = changeRequest.DocumentType;
            }

            var revision = board.Layouts.SelectMany(a => a.Revisions).First(r => r.RevisionCode == changeRequest.RevisionCode);

            foreach (var boardComponent in componentsToRemove)
            {
                if (boardComponent.AddChangeId == pcasChange.ChangeId && boardComponent.ChangeState != "LIVE")
                {
                    var component = board.Components.First(a => a.BoardLine == boardComponent.BoardLine);
                    board.Components.Remove(component);
                }
                else
                {
                    boardComponent.DeleteChangeId = pcasChange.ChangeId;
                }
            }

            foreach (var boardComponent in componentsToAdd)
            {
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

            return board;
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
