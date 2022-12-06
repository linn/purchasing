namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Facade;
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

        public ChangeRequestFacadeService(
            IRepository<ChangeRequest, int> repository,
            ITransactionManager transactionManager,
            IBuilder<ChangeRequest> resourceBuilder,
            IChangeRequestService changeRequestService,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.resourceBuilder = resourceBuilder;
            this.changeRequestService = changeRequestService;
            this.databaseService = databaseService;
            this.transactionManager = transactionManager;
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

        public IResult<ChangeRequestResource> CancelChangeRequest(int documentNumber, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        public IResult<ChangeRequestResource> ChangeStatus(ChangeRequestStatusChangeResource request, IEnumerable<string> privileges = null)
        {
            if (request?.Status == "ACCEPT")
            {
                return this.ApproveChangeRequest(request.Id, privileges);
            }

            if (request?.Status == "CANCEL")
            {
                return this.CancelChangeRequest(request.Id, privileges);
            }

            return new BadRequestResult<ChangeRequestResource>($"Cannot change status to {request?.Status}");
        }

        protected override ChangeRequest CreateFromResource(ChangeRequestResource resource, IEnumerable<string> privileges = null)
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
                           EnteredById = (int) resource.EnteredBy.Id,
                           ProposedById = (int) resource.ProposedBy.Id,
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(ChangeRequest entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
