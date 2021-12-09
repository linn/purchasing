namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;

    public interface IApplicationStateService<T, TKey, TResource, TUpdateResource, TSearchResource> 
        : IFacadeResourceFilterService<T, TKey, TResource, TUpdateResource, TSearchResource>
    {
        IResult<TResource> GetApplicationState(IEnumerable<string> claims);
    }
}
