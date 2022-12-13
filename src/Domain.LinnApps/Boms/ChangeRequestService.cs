namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class ChangeRequestService : IChangeRequestService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<ChangeRequest, int> repository;

        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        public ChangeRequestService(
            IAuthorisationService authService,
            IRepository<ChangeRequest, int> repository,
            IQueryRepository<Part> partRepository,
            IRepository<Employee, int> employeeRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.partRepository = partRepository;
            this.employeeRepository = employeeRepository;
        }

        public Part ValidPartNumber(string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return null;
            }

            var part = this.partRepository.FindBy(p => p.PartNumber == partNumber);
            if (part == null)
            {
                throw new DomainException("invalid part number");
            }

            return part;
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

        public ChangeRequest Cancel(
            int documentNumber,
            int cancelledById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            var employee = this.employeeRepository.FindById(cancelledById);
            if (employee == null)
            {
                throw new ItemNotFoundException("Employee not found");
            }

            if (request.ChangeState == "ACCEPT" && !this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to cancel change requests");
            }

            if (request.CanCancel(true))
            {
                request.Cancel(employee, selectedBomChangeIds, selectedPcasChangeIds);
            }
            else
            {
                throw new InvalidStateChangeException("Cannot cancel this change request");
            }

            return request;
        }
    }
}
