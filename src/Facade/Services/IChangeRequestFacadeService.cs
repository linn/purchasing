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
        IResult<ChangeRequestResource> ApproveChangeRequest(
            int documentNumber, IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> CancelChangeRequest(
            int documentNumber, 
            int cancelledById,
            IEnumerable<int> selectedBomChangeIds, 
            IEnumerable<int> selectedPcasChangeIds, 
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> MakeLiveChangeRequest(
            int documentNumber, 
            int appliedById, 
            IEnumerable<int> selectedBomChangeIds, 
            IEnumerable<int> selectedPcasChangeIds, 
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> UndoChangeRequest(
            int documentNumber,
            int undoneById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> ChangeStatus(
            ChangeRequestStatusChangeResource request, int changedById, IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> PhaseInChangeRequest(
            ChangeRequestPhaseInsResource request,
            IEnumerable<string> privileges = null);

        IResult<ChangeRequestResource> ChangeRequestReplace(
            ChangeRequestReplaceResource request,
            IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBom(
            string bomName, IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBoard(
            string bomName, IEnumerable<string> privileges = null);

        IResult<IEnumerable<ChangeRequestResource>> SearchChangeRequests(
            string searchTerm, bool? outstanding, int? lastMonths, IEnumerable<string> privileges = null);
    }
}
