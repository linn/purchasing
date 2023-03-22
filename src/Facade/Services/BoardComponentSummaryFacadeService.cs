namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BoardComponentSummaryFacadeService : QueryFacadeResourceService<BoardComponentSummary,
        BoardComponentSummaryResource, BoardComponentSummaryResource>
    {
        private readonly ICircuitBoardSummaryService circuitBoardSummaryService;

        public BoardComponentSummaryFacadeService(
            IQueryRepository<BoardComponentSummary> repository,
            IBuilder<BoardComponentSummary> resourceBuilder,
            ICircuitBoardSummaryService circuitBoardSummaryService)
            : base(repository, resourceBuilder)
        {
            this.circuitBoardSummaryService = circuitBoardSummaryService;
        }

        protected override Expression<Func<BoardComponentSummary, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<BoardComponentSummary, bool>> FilterExpression(BoardComponentSummaryResource searchResource)
        {
            return this.circuitBoardSummaryService.GetFilterExpression(
                searchResource.BoardCode,
                searchResource.RevisionCode,
                searchResource.Cref,
                searchResource.PartNumber);
        }
        
        protected override Expression<Func<BoardComponentSummary, bool>> FindExpression(BoardComponentSummaryResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
