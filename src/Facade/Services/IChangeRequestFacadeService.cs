namespace Linn.Purchasing.Facade.Services
{
    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using System.Collections.Generic;

    using Linn.Purchasing.Resources.RequestResources;

    public interface IChangeRequestFacadeService : IFacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>
    {
        IResult<ChangeRequestResource> ApproveChangeRequest(int documentNumber, IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> ChangeStatus(ChangeRequestStatusChangeResource request, IEnumerable<string> privileges = null);
    }
}
