namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using System.Collections.Generic;

    public interface IChangeRequestFacadeService : IFacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>
    {
        IResult<ChangeRequestResource> ApproveChangeRequest(int documentNumber, IEnumerable<string> privileges = null);
    }
}
