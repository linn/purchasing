namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Linq.Expressions;

    using Linn.Purchasing.Domain.LinnApps.Boms.Models;

    public interface ICircuitBoardSummaryService
    {
        Expression<Func<BoardComponentSummary, bool>> GetFilterExpression(
            string boardCodeSearch,
            string revisionCodeSearch,
            string crefSearch,
            string partNumberSearch);
    }
}
