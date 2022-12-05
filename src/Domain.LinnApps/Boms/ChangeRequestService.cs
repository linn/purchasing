namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class ChangeRequestService : IChangeRequestService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<ChangeRequest, int> repository;

        public ChangeRequestService(IAuthorisationService authService, IRepository<ChangeRequest, int> repository)
        {
            this.authService = authService;
            this.repository = repository;
        }

        public ChangeRequest Approve(int documentNumber, IEnumerable<string> privileges = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.ApproveChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to approve change requests");
            }

            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            if (request.CanApprove())
            {
                request.Approve();
            }
            else
            {
                throw new InvalidStateChangeException("Cannot approve this change request");
            }

            return request;
        }
    }
}
