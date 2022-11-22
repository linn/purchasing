namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Domain.LinnApps.Boms.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Resources;

    public class ChangeRequestFacadeService :  FacadeResourceService<ChangeRequest, int, ChangeRequestResource, ChangeRequestResource>, IChangeRequestFacadeService
    {
        private readonly IRepository<ChangeRequest, int> repository;

        private readonly IChangeRequestService changeRequestService;

        private readonly IBuilder<ChangeRequest> resourceBuilder;

        public ChangeRequestFacadeService(IRepository<ChangeRequest, int> repository, ITransactionManager transactionManager, IBuilder<ChangeRequest> resourceBuilder, IChangeRequestService changeRequestService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.repository = repository;
            this.resourceBuilder = resourceBuilder;
            this.changeRequestService = changeRequestService;
        }

        public IResult<ChangeRequestResource> ApproveChangeRequest(int documentNumber, IEnumerable<string> privileges = null)
        {
            try
            {
                var request = this.changeRequestService.Approve(documentNumber, privileges);
                var resource = (ChangeRequestResource) this.resourceBuilder.Build(request, new List<string>());
                return new SuccessResult<ChangeRequestResource>(resource);
            }
            catch (ItemNotFoundException e)
            {
                return new NotFoundResult<ChangeRequestResource>("Change Request not found");
            }
            catch (InvalidStateChangeException e)
            {
                return new BadRequestResult<ChangeRequestResource>("Cannot approve this change request");
            }
        }

        protected override ChangeRequest CreateFromResource(ChangeRequestResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(ChangeRequest entity, ChangeRequestResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<ChangeRequest, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
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
