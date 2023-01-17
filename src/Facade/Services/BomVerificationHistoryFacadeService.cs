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
    using Linn.Purchasing.Resources.Boms;

    public class BomVerificationHistoryFacadeService : FacadeResourceService<BomVerificationHistory, string, BomVerificationHistoryResource, BomVerificationHistoryResource>
    {
        private readonly IBuilder<BomVerificationHistory> resourceBuilder;

        private readonly ITransactionManager transactionManager;

        private readonly IDatabaseService databaseService;

        private readonly IBomVerificationHistoryService bomVerificationHistoryService;

        public BomVerificationHistoryFacadeService(
            IRepository<BomVerificationHistory, string> repository,
            ITransactionManager transactionManager,
            IBuilder<BomVerificationHistory> resourceBuilder,
            IBomVerificationHistoryService bomVerificationHistoryService,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.resourceBuilder = resourceBuilder;
            this.databaseService = databaseService;
            this.bomVerificationHistoryService = bomVerificationHistoryService;
            this.transactionManager = transactionManager;
        }

        protected override BomVerificationHistory CreateFromResource(BomVerificationHistoryResource resource, IEnumerable<string> privileges = null)
        {
            if (resource == null)
            {
                throw new DomainException("Bom Verification History not present");
            }

            return new BomVerificationHistory
            {
                TRef = resource.TRef,
                PartNumber = resource.PartNumber,
                VerifiedBy = resource.VerifiedBy,
                DocumentType = resource.DocumentType,
                DocumentNumber = resource.DocumentNumber,
                Remarks= resource.Remarks
            };
        }

        protected override void UpdateFromResource(BomVerificationHistory entity, BomVerificationHistoryResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<BomVerificationHistory, bool>> SearchExpression(string searchTerm)
        {
            return bomVerificationHistory => searchTerm.Trim().ToUpper().Equals(bomVerificationHistory.DocumentNumber);
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            BomVerificationHistory entity,
            BomVerificationHistoryResource resource,
            BomVerificationHistoryResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(BomVerificationHistory entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
