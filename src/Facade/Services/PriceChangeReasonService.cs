namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class PriceChangeReasonService 
        : FacadeResourceService<PriceChangeReason, string, PriceChangeReasonResource, PriceChangeReasonResource>
    {
        public PriceChangeReasonService(IRepository<PriceChangeReason, string> repository, ITransactionManager transactionManager, IBuilder<PriceChangeReason> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override PriceChangeReason CreateFromResource(PriceChangeReasonResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            PriceChangeReason entity,
            PriceChangeReasonResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PriceChangeReason, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PriceChangeReason entity,
            PriceChangeReasonResource resource,
            PriceChangeReasonResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(PriceChangeReason entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
