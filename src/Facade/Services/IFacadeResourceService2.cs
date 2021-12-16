namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;

    using PagedList.Core;

    public interface IFacadeResourceService2<T, in TKey, TResource, in TUpdateResource>
    {
        IResult<TResource> GetById(TKey id, IEnumerable<string> privileges = null);

        IResult<IEnumerable<TResource>> GetAll(IEnumerable<string> privileges = null);

        IResult<IEnumerable<TResource>> Search(string searchTerm, IEnumerable<string> privileges = null);

        IResult<IPagedList<TResource>> GetAll(int pageNumber, int pageSize, IEnumerable<string> privileges = null);

        IResult<IPagedList<TResource>> GetAll<TKeySort>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TKeySort>> keySelectorForSort,
            bool asc,
            IEnumerable<string> privileges = null);

        IResult<TResource> Add(TResource resource, IEnumerable<string> privileges = null, int? userNumber = null);

        IResult<TResource> Update(
            TKey id,
            TUpdateResource updateResource,
            IEnumerable<string> privileges = null,
            int? userNumber = null);

        IResult<TResource> GetApplicationState(IEnumerable<string> privileges = null);

        IResult<TResource> DeleteOrObsolete(TKey id, IEnumerable<string> privileges = null, int? userNumber = null);
    }
}