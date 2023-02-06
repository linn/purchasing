namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;

    public class ChangeRequestService : IChangeRequestService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<ChangeRequest, int> repository;

        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<LinnWeek, int> weekRepository;

        private readonly IBomPack bomPack;

        private readonly IPcasPack pcasPack;

        public ChangeRequestService(
            IAuthorisationService authService,
            IRepository<ChangeRequest, int> repository,
            IQueryRepository<Part> partRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<LinnWeek, int> weekRepository,
            IBomPack bomPack,
            IPcasPack pcasPack)
        {
            this.authService = authService;
            this.repository = repository;
            this.partRepository = partRepository;
            this.employeeRepository = employeeRepository;
            this.weekRepository = weekRepository;
            this.bomPack = bomPack;
            this.pcasPack = pcasPack;
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

        public bool ChangeRequestAdmin(IEnumerable<string> privileges)
        {
            return this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges);
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

        public ChangeRequest MakeLive(
            int documentNumber,
            int appliedById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            var employee = this.employeeRepository.FindById(appliedById);
            if (employee == null)
            {
                throw new ItemNotFoundException("Employee not found");
            }

            if (!this.authService.HasPermissionFor(AuthorisedAction.MakeLiveChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to make live change requests");
            }

            if (request.CanMakeLive())
            {
                request.MakeLive(employee, selectedBomChangeIds, selectedPcasChangeIds);
            }
            else
            {
                throw new InvalidStateChangeException("Cannot make live this change request");
            }

            return request;
        }

        public ChangeRequest PhaseInChanges(int documentNumber, int? linnWeekNumber, DateTime? linnWeekStartDate, IEnumerable<int> selectedBomChangeIds, IEnumerable<string> privileges = null)
        {
            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            LinnWeek week = null;
            if (linnWeekNumber != null)
            {
                week = this.weekRepository.FindById((int) linnWeekNumber);
            }
            else if (linnWeekStartDate != null)
            {
                var weekDate = ((DateTime) linnWeekStartDate).Date;
                // if you don't do weekNumber > 0 then for this week you also get the Now week and Jacki doesn't want that
                week = this.weekRepository.FindBy(
                    d => d.StartsOn <= weekDate && d.EndsOn >= weekDate && d.WeekNumber > 0);
            }

            if (week == null)
            {
                throw new ItemNotFoundException("Linn Week not found");
            }

            if (week.EndsOn < DateTime.Now.Date)
            {
                throw new InvalidPhaseInWeekException("Phase in week is in the past");
            }

            if (!this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to phase in change requests");
            }

            request.PhaseIn(week, selectedBomChangeIds);

            return request;
        }

        public ChangeRequest UndoChanges(
            int documentNumber,
            int undoneById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            var changesUndone = false;

            if (!this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to undo change requests");
            }

            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            var employee = this.employeeRepository.FindById(undoneById);
            if (employee == null)
            {
                throw new ItemNotFoundException("Employee not found");
            }

            changesUndone = request.UndoChanges(
                employee,
                selectedBomChangeIds,
                selectedPcasChangeIds,
                this.bomPack,
                this.pcasPack);

            // undo change
            if (changesUndone)
            {
                var updatedRequest = this.repository.FindById(documentNumber);
                if (updatedRequest == null)
                {
                    throw new ItemNotFoundException("Change Request not found");
                }

                return updatedRequest;
            }

            return request;
        }

        public Expression<Func<ChangeRequest, bool>> SearchExpression(string searchTerm, bool? outstanding, int? lastMonths)
        {
            var fromDate = (lastMonths == null)
                               ? DateTime.Now.AddMonths(-120)
                               : DateTime.Now.AddMonths(-1 * (int)lastMonths);
            var inclLive = (outstanding == false) ? "LIVE" : "JUSTOUTSTANDING";

            var newPartNumber = searchTerm.Trim().ToUpper();
            var partSearch = newPartNumber.Split('*');

            // this big if is just because Linq/EF/Oracle doesn't do a LIKE
            // supports IC*, *3, *LEWIS*, PCAS*L1R1 but not multiple * e.g. PCAS */L1*
            if (string.IsNullOrEmpty(newPartNumber))
            {
                return r => (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive)
                         && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
            }
            
            if (!newPartNumber.Contains("*"))
            {
                return r => r.NewPartNumber.Equals(newPartNumber) && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive) && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
            }
            
            if (newPartNumber.EndsWith("*"))
            {
                // supporting *LEWIS*
                if (newPartNumber.StartsWith("*"))
                {
                    return r => r.NewPartNumber.Contains(partSearch[1]) && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive) && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
                }
                else
                {
                    return r => r.NewPartNumber.StartsWith(partSearch.First()) && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive) && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
                }
            }
            
            if (newPartNumber.StartsWith("*"))
            {
                return r => r.NewPartNumber.EndsWith(partSearch.Last()) && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive) && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
            }

            return r => r.NewPartNumber.StartsWith(partSearch.First()) && r.NewPartNumber.EndsWith(partSearch.Last()) && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive) && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
        }
    }
}
