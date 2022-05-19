namespace Linn.Purchasing.Facade.Services
{
    using System.Collections;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IPatchFacadeResourceService<T, in TKey, TResource, in TUpdateResource> 
        : IFacadeResourceService<T, TKey, TResource, TUpdateResource>
    {
        IResult<T> Patch(
            TKey key,
            PatchRequestResource<TResource> patchResource, 
            IEnumerable<string> privileges);
    }
}
