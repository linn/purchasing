﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
    using Linn.Common.Logging;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.RequestResources;

    public class ChangeRequestFacadeService :
        FacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>,
        IChangeRequestFacadeService
    {
        private readonly IChangeRequestService changeRequestService;

        private readonly IBuilder<ChangeRequest> resourceBuilder;

        private readonly ITransactionManager transactionManager;

        private readonly IDatabaseService databaseService;

        private readonly ILog log;

        private readonly IBomTreeService bomTreeService;

        private readonly IRepository<ChangeRequest, int> repository;

        public ChangeRequestFacadeService(
            IRepository<ChangeRequest, int> repository,
            ITransactionManager transactionManager,
            IBuilder<ChangeRequest> resourceBuilder,
            IChangeRequestService changeRequestService,
            IDatabaseService databaseService,
            IBomTreeService bomTreeService,
            ILog log)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.resourceBuilder = resourceBuilder;
            this.changeRequestService = changeRequestService;
            this.databaseService = databaseService;
            this.log = log;
            this.transactionManager = transactionManager;
            this.bomTreeService = bomTreeService;
            this.repository = repository;
        }

        public IResult<ChangeRequestResource> ApproveChangeRequest(int documentNumber, IEnumerable<string> privileges = null)
        {
            try
            {
                var request = this.changeRequestService.Approve(documentNumber, privileges);
                this.transactionManager.Commit();
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(request, new List<string>());
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot approve this change request");
            }
        }

        public IResult<ChangeRequestResource> CancelChangeRequest(
            int documentNumber,
            int cancelledById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            try
            {
                var request = this.changeRequestService.Cancel(documentNumber, cancelledById, selectedBomChangeIds, selectedPcasChangeIds, privileges);
                this.transactionManager.Commit();
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(request, privileges);
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot cancel this change request");
            }
        }

        public IResult<ChangeRequestResource> MakeLiveChangeRequest(
            int documentNumber,
            int appliedById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            try
            {
                var request = this.changeRequestService.MakeLive(documentNumber, appliedById, selectedBomChangeIds, selectedPcasChangeIds, privileges);
                this.transactionManager.Commit();
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(request, privileges);
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException e)
            {
                return new BadRequestResult<ChangeRequestResource>(e.Message ?? "Cannot make this change request live");
            }
        }

        public IResult<ChangeRequestResource> UndoChangeRequest(
            int documentNumber,
            int undoneById,
            IEnumerable<int> selectedBomChangeIds,
            IEnumerable<int> selectedPcasChangeIds,
            IEnumerable<string> privileges = null)
        {
            try
            {
                var request = this.changeRequestService.UndoChanges(documentNumber, undoneById, selectedBomChangeIds, selectedPcasChangeIds, privileges);
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(request, privileges);
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot undo this Change Request");
            }
        }

        public IResult<ChangeRequestResource> ChangeStatus(ChangeRequestStatusChangeResource request, int changedById, IEnumerable<string> privileges = null)
        {
            if (request?.Status == "ACCEPT")
            {
                return this.ApproveChangeRequest(request.Id, privileges);
            }

            if (request?.Status == "CANCEL")
            {
                return this.CancelChangeRequest(request.Id, changedById, request.SelectedBomChangeIds, request.SelectedPcasChangeIds, privileges);
            }

            if (request?.Status == "LIVE")
            {
                return this.MakeLiveChangeRequest(request.Id, changedById, request.SelectedBomChangeIds, request.SelectedPcasChangeIds, privileges);
            }

            if (request?.Status == "UNDO")
            {
                return this.UndoChangeRequest(request.Id, changedById, request.SelectedBomChangeIds, request.SelectedPcasChangeIds, privileges);
            }

            return new BadRequestResult<ChangeRequestResource>($"Cannot change status to {request?.Status}");
        }

        public IResult<ChangeRequestResource> PhaseInChangeRequest(
            ChangeRequestPhaseInsResource request,
            IEnumerable<string> privileges = null)
        {
            if (request == null)
            {
                return new BadRequestResult<ChangeRequestResource>("No parameters supplied");
            }
            else if (request.PhaseInWeek == null && request.PhaseInWeekStart == null)
            {
                return new BadRequestResult<ChangeRequestResource>("No phase in week supplied");
            }

            try
            {
                var changeRequest = this.changeRequestService.PhaseInChanges(request.DocumentNumber, request.PhaseInWeek, request.PhaseInWeekStart, request.SelectedBomChangeIds, privileges);
                this.transactionManager.Commit();
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(changeRequest, privileges);
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot phase in this change request");
            }
        }

        public IResult<ChangeRequestResource> ChangeRequestReplace(ChangeRequestReplaceResource request, int replacedBy, IEnumerable<string> privileges = null)
        {
            if (request == null)
            {
                return new BadRequestResult<ChangeRequestResource>("No parameters supplied");
            }
            else if (request.GlobalReplace == false && request.SelectedDetailIds == null && request.SelectedPcasComponents == null && request.AddToBoms == null)
            {
                return new BadRequestResult<ChangeRequestResource>("No details selected to replace");
            }

            try
            {
                var addBoms = request.AddToBoms?.Select(a => (bomName: a.BomName, qty: a.Qty)).ToList();
                var changeRequest = this.changeRequestService.Replace(request.DocumentNumber, replacedBy, request.GlobalReplace, request.HasPcasLines, request.NewQty, request.SelectedDetailIds, request.SelectedPcasComponents, addBoms, privileges);
                this.transactionManager.Commit();
                var resource = (ChangeRequestResource)this.resourceBuilder.Build(changeRequest, privileges);
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
        }

        public IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBom(
            string bomName, IEnumerable<string> privileges = null)
        {
            var assemblies = this.bomTreeService
                .FlattenBomTree(bomName.ToUpper().Trim(), null, false, true)
                .Where(x => x.Type != "C").Select(a => a.Name).ToList();


            var changeRequests = assemblies.Count == 0 ? this.repository.FilterBy(x => x.NewPartNumber == bomName).ToList() : this.repository.FilterBy(
                x => assemblies.Contains(x.NewPartNumber) 
                     && (x.ChangeState == "ACCEPT" || x.ChangeState == "PROPOS")).ToList();

            return new SuccessResult<IEnumerable<ChangeRequestResource>>(
                changeRequests.Select(x => (ChangeRequestResource)this.resourceBuilder.Build(x, privileges)));
        }

        public IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBoard(string boardCode, IEnumerable<string> privileges = null)
        {
            var changeRequests = this.repository.FilterBy(x => x.BoardCode == boardCode.ToUpper()
                     && (x.ChangeState == "ACCEPT" || x.ChangeState == "PROPOS")).ToList();

            return new SuccessResult<IEnumerable<ChangeRequestResource>>(
                changeRequests.Select(x => (ChangeRequestResource)this.resourceBuilder.Build(x, privileges)));
        }

        public IResult<IEnumerable<ChangeRequestResource>> SearchChangeRequests(
            string searchTerm,
            bool? outstanding, 
            int? lastMonths,
            bool? cancelled,
            IEnumerable<string> privileges = null)
        {
            var expression = this.changeRequestService.SearchExpression(searchTerm, outstanding, lastMonths, cancelled);
            var changeRequests = this.repository.FindAll().Where(expression).OrderByDescending(r => r.DocumentNumber).ToList();

            return new SuccessResult<IEnumerable<ChangeRequestResource>>(changeRequests.Select(x => (ChangeRequestResource)this.resourceBuilder.Build(x, privileges)));
        }

        public IResult<ChangeRequestResource> AddAndReplace(ChangeRequestResource resource, int createdBy, IEnumerable<string> privileges = null)
        {
            /* on original form this was done with Post-Database-Commit trigger so have to find way to do two stage operation since
               replace relies on there being an existing change request */
            var createresult = this.Add(resource, privileges, createdBy);
            if (resource.GlobalReplace)
            {
                if (createresult is CreatedResult<ChangeRequestResource>)
                {
                    var createdRequest = ((CreatedResult<ChangeRequestResource>) createresult).Data;
                    var replaceRequest = new ChangeRequestReplaceResource
                                             {
                                                 DocumentNumber = createdRequest.DocumentNumber,
                                                 GlobalReplace = createdRequest.GlobalReplace,
                                                 HasPcasLines =
                                                     createdRequest.ChangeType
                                                     == "REPLACE" /* BOARDEDIT won't but not easy to know with REPLACE so say true in case */
                                             };
                    var replaceresult = this.ChangeRequestReplace(replaceRequest, createdBy, privileges);
                    if (replaceresult is not SuccessResult<ChangeRequestResource>)
                    {
                        return replaceresult;
                    }

                    ((CreatedResult<ChangeRequestResource>) createresult).Data =
                        ((SuccessResult<ChangeRequestResource>) replaceresult).Data;
                }
            }

            return createresult;
        }

        protected override ChangeRequest CreateFromResource(
            ChangeRequestResource resource, IEnumerable<string> privileges = null)
        {
            if (resource == null)
            {
                throw new DomainException("Change Request not present");
            }

            var requestId = this.databaseService.GetNextVal("CRF_SEQ");
            var newPart = this.changeRequestService.ValidPartNumber(resource.NewPartNumber);

            return new ChangeRequest
                       {
                           DocumentType = "CRF",
                           DocumentNumber = requestId,
                           ChangeRequestType = resource.ChangeType,
                           ChangeState = "PROPOS",
                           DateEntered = DateTime.Now,
                           EnteredById = (int)resource.EnteredBy.Id,
                           ProposedById = (int)resource.ProposedBy.Id,
                           OldPartNumber = resource.OldPartNumber,
                           NewPartNumber = newPart.PartNumber,
                           BoardCode = resource.BoardCode,
                           RevisionCode = resource.RevisionCode,
                           ReasonForChange = resource.ReasonForChange,
                           DescriptionOfChange = resource.DescriptionOfChange,
                           GlobalReplace = resource.GlobalReplace ? "Y" : "N",
                           RequiresVerification = "N",
                           RequiresStartingSernos = "N"
                       };
        }

        protected override void UpdateFromResource(ChangeRequest entity, ChangeRequestResource updateResource, IEnumerable<string> privileges = null)
        {
            if (entity.CanEdit(this.changeRequestService.ChangeRequestAdmin(privileges)))
            {
                entity.ReasonForChange = updateResource.ReasonForChange;
                entity.DescriptionOfChange = updateResource.DescriptionOfChange;
            }
        }

        protected override Expression<Func<ChangeRequest, bool>> SearchExpression(string searchTerm)
        {
           return cr => searchTerm.Trim().ToUpper().Equals(cr.NewPartNumber) 
                         && cr.ChangeState != "LIVE" && cr.ChangeState != "CANCEL";
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            ChangeRequest entity,
            ChangeRequestResource resource,
            ChangeRequestResource updateResource)
        {
            this.log.Info($"updated {entity.DocumentNumber}");
        }

        protected override void DeleteOrObsoleteResource(ChangeRequest entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
