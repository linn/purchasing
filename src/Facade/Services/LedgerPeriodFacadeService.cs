namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Resources;

    public class LedgerPeriodFacadeService : FacadeResourceService<LedgerPeriod, int, LedgerPeriodResource, LedgerPeriodResource>
    {
        public LedgerPeriodFacadeService(
            IRepository<LedgerPeriod, int> repository,
            ITransactionManager transactionManager,
            IBuilder<LedgerPeriod> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override LedgerPeriod CreateFromResource(
            LedgerPeriodResource resource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(LedgerPeriod entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            LedgerPeriod entity,
            LedgerPeriodResource resource,
            LedgerPeriodResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<LedgerPeriod, bool>> SearchExpression(string searchTerm)
        {
            return lp => lp.MonthName.Trim().Contains(searchTerm) || lp.PeriodNumber.ToString().Contains(searchTerm);
        }

        protected override void UpdateFromResource(
            LedgerPeriod entity,
            LedgerPeriodResource updateResource,
            IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
