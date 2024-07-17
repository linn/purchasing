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

        private readonly IBomChangeService bomChangeService;

        private readonly IRepository<CircuitBoard, string> boardRepository;

        private readonly IRepository<BomDetail, int> bomDetailRepository;

        public ChangeRequestService(
            IAuthorisationService authService,
            IRepository<ChangeRequest, int> repository,
            IQueryRepository<Part> partRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<LinnWeek, int> weekRepository,
            IBomPack bomPack,
            IPcasPack pcasPack,
            IBomChangeService bomChangeService,
            IRepository<CircuitBoard, string> boardRepository,
            IRepository<BomDetail, int> bomDetailRepository)
        {
            this.authService = authService;
            this.repository = repository;
            this.partRepository = partRepository;
            this.employeeRepository = employeeRepository;
            this.weekRepository = weekRepository;
            this.bomPack = bomPack;
            this.pcasPack = pcasPack;
            this.bomChangeService = bomChangeService;
            this.boardRepository = boardRepository;
            this.bomDetailRepository = bomDetailRepository;
        }

        public Part ValidPartNumber(string partNumber)
        {
            if (string.IsNullOrEmpty(partNumber))
            {
                return null;
            }

            var part = this.partRepository.FindBy(
                p => p.PartNumber == partNumber.Trim().ToUpper());
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

            if (request.ChangeState == "ACCEPT" 
                && !this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
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

            if (request.BomChanges != null)
            {
                var hasSpecifiedChanges = (selectedBomChangeIds != null && selectedBomChangeIds.Any())
                                          || (selectedPcasChangeIds != null && selectedPcasChangeIds.Any());

                var changesToCheck = hasSpecifiedChanges
                                         ? request.BomChanges.Where(
                                             x => selectedBomChangeIds != null && selectedBomChangeIds.Contains(x.ChangeId)) : request.BomChanges;
                
                    foreach (var c in changesToCheck)
                    {
                        var currentLiveDetails
                            = this.bomDetailRepository
                                .FilterBy(x => x.BomId == c.BomId && c.ChangeState == "LIVE")?.Select(
                                    d => d.PartNumber).ToList();

                        if (c.AddedBomDetails != null)
                        {
                            foreach (var d in c.AddedBomDetails)
                            {
                                var bomPart = this.partRepository.FindBy(p => p.BomId == d.BomId);
                                if (currentLiveDetails != null && currentLiveDetails.Contains(d.PartNumber))
                                {
                                    throw new InvalidBomChangeException(
                                        $"{d.PartNumber} is already live on {c.BomName}!!");
                                }

                                if (bomPart.DateLive.HasValue && !d.Part.DateLive.HasValue)
                                {
                                    throw new InvalidBomChangeException(
                                        $"Cannot add NON-LIVE {d.PartNumber} onto BOM of {bomPart.PartNumber}, which IS LIVE!!");
                                }
                            }
                        }
                    }
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

        public ChangeRequest PhaseInChanges(
            int documentNumber, 
            int? linnWeekNumber, 
            DateTime? linnWeekStartDate, 
            IEnumerable<int> selectedBomChangeIds, 
            IEnumerable<string> privileges = null)
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
                var weekDate = ((DateTime)linnWeekStartDate).Date;
                
                // if you don't do weekNumber > 0 then for this week you also get the Now week
                // and Jacki doesn't want that
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

        public ChangeRequest Replace(
            int documentNumber,
            int replacedBy,
            bool globalReplace,
            bool hasPcasLines,
            decimal? newQty,
            IEnumerable<int> selectedDetailIds,
            IEnumerable<string> selectedPcasComponents,
            IEnumerable<(string bomName, decimal qty)> addToBoms,
            IEnumerable<string> privileges = null)
        {
            var request = this.repository.FindById(documentNumber);
            if (request == null)
            {
                throw new ItemNotFoundException("Change Request not found");
            }

            if (request.ChangeState == "ACCEPT" 
                && !this.authService.HasPermissionFor(AuthorisedAction.AdminChangeRequest, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to do change requests replace");
            }

            if (globalReplace)
            {
                request.GlobalReplace = "Y";
                this.bomChangeService.ReplaceAllBomDetails(request, replacedBy, null);

                if (hasPcasLines && (request.OldPartNumber != request.NewPartNumber))
                {
                    this.pcasPack.ReplaceAll(
                        request.OldPartNumber,
                        request.DocumentNumber,
                        request.ChangeState,
                        replacedBy,
                        request.NewPartNumber);
                }

                return request;
            }
            
            if (selectedDetailIds != null && selectedDetailIds.Any())
            {
                foreach (var detailId in selectedDetailIds)
                {
                    this.bomChangeService.ReplaceBomDetail(detailId, request, replacedBy, newQty);
                }
            }

            if (selectedPcasComponents != null && selectedPcasComponents.Any())
            {
                foreach (var componentKeys in selectedPcasComponents)
                {
                    // expecting board/revision/cref/qty/asst/...
                    // basically a way of passing parameters for this call
                    var keys = componentKeys.Split("/");
                    if (keys.Length < 5)
                    {
                        throw new DomainException($"invalid selected component {componentKeys}");
                    }

                    var board = this.boardRepository.FindById(keys[0]);
                    if (board == null)
                    {
                        throw new ItemNotFoundException($"no board found for {keys[0]}");
                    }

                    var revision = board.Layouts.SelectMany(a => a.Revisions)
                        .FirstOrDefault(a => a.RevisionCode == keys[1]);
                    if (revision == null)
                    {
                        throw new ItemNotFoundException($"no revision found for {keys[1]}");
                    }

                    var qty = newQty ?? Convert.ToDecimal(keys[3]);

                    this.pcasPack.ReplacePartWith(
                        board.BoardCode, 
                        revision.RevisionCode, 
                        revision.LayoutSequence, 
                        revision.VersionNumber, 
                        request.DocumentNumber, 
                        request.ChangeState, 
                        replacedBy, 
                        keys[2], 
                        request.OldPartNumber, 
                        request.NewPartNumber, qty, keys[4]);
                }
            }

            if (addToBoms != null && addToBoms.Any())
            {
                foreach (var addBom in addToBoms)
                {
                    this.bomChangeService.AddBomDetail(addBom.bomName, request, replacedBy, addBom.qty);
                }
            }

            return request;
        }

        public Expression<Func<ChangeRequest, bool>> SearchExpression(
            string searchTerm, bool? outstanding, int? lastMonths, bool? cancelled)
        {
            DateTime fromDate;

            if (lastMonths == 0)
            {
                fromDate = DateTime.MinValue;
            }
            else
            {
                fromDate = lastMonths == null
                               ? DateTime.Now.AddMonths(-120)
                               : DateTime.Now.AddMonths(-1 * (int)lastMonths);
            }

            var inclLive = (outstanding == false) ? "LIVE" : "JUSTOUTSTANDING";
            var inclCancelled = (cancelled == true) ? "CANCEL" : "NOTCANCELLED";
            var exclCancelled = (cancelled == false) ? "CANCEL" : "NOTCANCELLED";
            var searchPartNumber = searchTerm.Trim().ToUpper();
            var partSearch = searchPartNumber.Split('*');

            // this big if is just because Linq/EF/Oracle doesn't do a LIKE
            // supports IC*, *3, *LEWIS*, PCAS*L1R1 but not multiple * e.g. PCAS */L1*
            if (string.IsNullOrEmpty(searchPartNumber))
            {
                return r 
                    => (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive || r.ChangeState == inclCancelled)
                       && r.ChangeState != "CANCEL" && r.DateEntered >= fromDate;
            }
            
            if (!searchPartNumber.Contains("*"))
            {
                return r 
                    => (r.NewPartNumber.Equals(searchPartNumber) 
                        || r.OldPartNumber.Equals(searchPartNumber)) 
                       && (r.ChangeState == "PROPOS" || r.ChangeState == "ACCEPT" || r.ChangeState == inclLive || r.ChangeState == inclCancelled) 
                       && r.ChangeState != exclCancelled && r.DateEntered >= fromDate;
            }
            
            if (searchPartNumber.EndsWith("*"))
            {
                // supporting *LEWIS*
                if (searchPartNumber.StartsWith("*"))
                {
                    return r => (r.NewPartNumber.Contains(partSearch[1]) 
                                 || r.OldPartNumber.Contains(partSearch[1])) 
                                && (r.ChangeState == "PROPOS" 
                                    || r.ChangeState == "ACCEPT" 
                                    || r.ChangeState == inclLive
                                    || r.ChangeState == inclCancelled) 
                                && r.ChangeState != exclCancelled && r.DateEntered >= fromDate;
                }
                else
                {
                    return r => (r.NewPartNumber.StartsWith(partSearch.First()) 
                                 || r.OldPartNumber.StartsWith(partSearch.First())) 
                                && (r.ChangeState == "PROPOS" 
                                    || r.ChangeState == "ACCEPT" 
                                    || r.ChangeState == inclLive
                                    || r.ChangeState == inclCancelled) 
                                && r.ChangeState != exclCancelled && r.DateEntered >= fromDate;
                }
            }
            
            if (searchPartNumber.StartsWith("*"))
            {
                return r => (r.NewPartNumber.EndsWith(partSearch.Last()) 
                             || r.OldPartNumber.EndsWith(partSearch.Last())) 
                            && (r.ChangeState == "PROPOS" 
                                || r.ChangeState == "ACCEPT" 
                                || r.ChangeState == inclLive
                                || r.ChangeState == inclCancelled) 
                            && r.ChangeState != exclCancelled && r.DateEntered >= fromDate;
            }

            return r => ((r.NewPartNumber.StartsWith(partSearch.First()) 
                          && r.NewPartNumber.EndsWith(partSearch.Last())) 
                         || (r.OldPartNumber.StartsWith(partSearch.First()) 
                             && r.OldPartNumber.EndsWith(partSearch.Last()))) 
                        && (r.ChangeState == "PROPOS"
                            || r.ChangeState == "ACCEPT" 
                            || r.ChangeState == inclLive
                            || r.ChangeState == inclCancelled) 
                        && r.ChangeState != exclCancelled && r.DateEntered >= fromDate;
        }
    }
}
