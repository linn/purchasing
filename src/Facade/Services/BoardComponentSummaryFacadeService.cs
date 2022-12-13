namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Models;
    using Linn.Purchasing.Resources.Boms;

    public class BoardComponentSummaryFacadeService : QueryFacadeResourceService<BoardComponentSummary,
        BoardComponentSummaryResource, BoardComponentSummaryResource>
    {
        public BoardComponentSummaryFacadeService(IQueryRepository<BoardComponentSummary> repository, IBuilder<BoardComponentSummary> resourceBuilder)
            : base(repository, resourceBuilder)
        {
        }

        protected override Expression<Func<BoardComponentSummary, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<BoardComponentSummary, bool>> FilterExpression(BoardComponentSummaryResource searchResource)
        {
            return a => (
                            string.IsNullOrEmpty(searchResource.BoardCode) 
                            || a.BoardCode == searchResource.BoardCode)
                        && (string.IsNullOrEmpty(searchResource.RevisionCode)
                            || a.RevisionCode == searchResource.RevisionCode)
                        && (string.IsNullOrEmpty(searchResource.Cref)
                            || a.Cref == searchResource.Cref)
                        && (string.IsNullOrEmpty(searchResource.PartNumber)
                            || a.PartNumber == searchResource.PartNumber);
        }

        protected override Expression<Func<BoardComponentSummary, bool>> FindExpression(BoardComponentSummaryResource searchResource)
        {
            throw new NotImplementedException();
        }
    }
}
