namespace Linn.Purchasing.Facade.Services
{
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public interface IChangeRequestFacadeService 
        : IFacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>
    {
        IResult<ChangeRequestResource> ChangeStatus(
            ChangeRequestStatusChangeResource request, int changedById, IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> PhaseInChangeRequest(
            ChangeRequestPhaseInsResource request,
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> ChangeRequestReplace(
            ChangeRequestReplaceResource request,
            int replacedBy,
            IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBom(
            string bomName, IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBoard(
            string bomName, IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> SearchChangeRequests(
            string searchTerm,
            bool? outstanding, 
            int? lastMonths, 
            bool? cancelled, 
            string boardCode = null,
            string rootProduct = null,
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> AddAndReplace(
            ChangeRequestResource resource, int createdBy, IEnumerable<string> privileges = null);
    } 
}
