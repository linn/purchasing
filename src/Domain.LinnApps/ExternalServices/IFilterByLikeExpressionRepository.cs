namespace Linn.Purchasing.Domain.LinnApps.ExternalServices
{
    using System.Linq;

    using Linn.Common.Persistence;

    public interface IFilterByLikeExpressionRepository<T, in TKey> : IRepository<T, TKey>
    {
        IQueryable<T> FilterByLikeExpression(string expression);
    }
}
