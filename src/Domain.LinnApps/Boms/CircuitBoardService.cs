namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class CircuitBoardService : ICircuitBoardService
    {
        private readonly IRepository<ChangeRequest, int> changeRequestRepository;

        private readonly IRepository<CircuitBoard, string> boardRepository;

        public CircuitBoardService(
            IRepository<ChangeRequest, int> changeRequestRepository,
            IRepository<CircuitBoard, string> boardRepository)
        {
            this.changeRequestRepository = changeRequestRepository;
            this.boardRepository = boardRepository;
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
                board.Components.Add(boardComponent);
                // TODO set from things
            }

            return board;
        }
    }
}
