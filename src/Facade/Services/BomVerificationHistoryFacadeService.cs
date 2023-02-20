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

    public class BomVerificationHistoryFacadeService : FacadeResourceService<BomVerificationHistory, int, BomVerificationHistoryResource, BomVerificationHistoryResource>
    {
        private readonly IDatabaseService databaseService;

        public BomVerificationHistoryFacadeService(
            IRepository<BomVerificationHistory, int> repository,
            ITransactionManager transactionManager,
            IBuilder<BomVerificationHistory> resourceBuilder,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.databaseService = databaseService;
        }

        protected override BomVerificationHistory CreateFromResource(BomVerificationHistoryResource resource, IEnumerable<string> privileges = null)
        {
            if (resource == null)
            {
                throw new DomainException("Bom Verification History not present");
            }

            return new BomVerificationHistory
            {
                TRef = this.databaseService.GetNextVal("BVH_SEQ"),
                PartNumber = resource.PartNumber,
                VerifiedBy = resource.VerifiedBy,
                DateVerified = DateTime.Now,
                DocumentType = resource.DocumentType,
                DocumentNumber = resource.DocumentNumber,
                Remarks = resource.Remarks
            };
        }

        protected override void UpdateFromResource(BomVerificationHistory entity, BomVerificationHistoryResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<BomVerificationHistory, bool>> SearchExpression(string searchTerm)
        {
            return x => x.PartNumber.Contains(searchTerm.ToUpper()) || x.TRef.ToString().Contains(searchTerm.ToUpper());
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
