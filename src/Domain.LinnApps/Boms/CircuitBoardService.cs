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
            IEnumerable<BoardComponent> componentsToAdd,
            IEnumerable<BoardComponent> componentsToRemove)
        {
            var board = this.boardRepository.FindById(boardCode);
            if (board == null)
            {
                throw new ItemNotFoundException($"Could not find board {boardCode}");
            }

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
                    // TODO set to things
                }
            }

            foreach (var boardComponent in componentsToAdd)
            {
                var part = this.partRepository.FindBy(a => a.PartNumber == boardComponent.PartNumber.ToUpper());
                boardComponent.AddChangeId = pcasChange.ChangeId;
                boardComponent.AssemblyTechnology = part.AssemblyTechnology;
                board.Components.Add(boardComponent);
                // TODO set from things
            }

            return board;
        }
    }
}
