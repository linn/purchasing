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
                var resource = (ChangeRequestResource) this.resourceBuilder.Build(request, new List<string>());
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
            catch (InvalidStateChangeException)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot make this change request live");
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

            return new BadRequestResult<ChangeRequestResource>($"Cannot change status to {request?.Status}");
        }

        public IResult<IEnumerable<ChangeRequestResource>> GetChangeRequestsRelevantToBom(
            string bomName, IEnumerable<string> privileges = null)
        {
            var assemblies = this.bomTreeService
                .FlattenBomTree(bomName.ToUpper().Trim(), null, false, true)
                .Where(x => x.Type != "C").Select(a => a.Name).ToList();

            var changeRequests = this.repository.FilterBy(
                x => assemblies.Contains(x.NewPartNumber) 
                     && (x.ChangeState == "ACCEPT" || x.ChangeState == "PROPOS")).ToList();

            return new SuccessResult<IEnumerable<ChangeRequestResource>>(
                changeRequests.Select(x => (ChangeRequestResource)this.resourceBuilder.Build(x, privileges)));
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
                           NewPartNumber = resource.NewPartNumber,
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
