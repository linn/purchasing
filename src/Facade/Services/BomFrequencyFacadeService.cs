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

    public class BomFrequencyFacadeService : FacadeResourceService<BomFrequencyWeeks, string, BomFrequencyWeeksResource, BomFrequencyWeeksResource>
    {
        private readonly IDatabaseService databaseService;

        public BomFrequencyFacadeService(
            IRepository<BomFrequencyWeeks, string> repository,
            ITransactionManager transactionManager,
            IBuilder<BomFrequencyWeeks> resourceBuilder,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.databaseService = databaseService;
        }

        protected override BomFrequencyWeeks CreateFromResource(BomFrequencyWeeksResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(BomFrequencyWeeks entity, BomFrequencyWeeksResource updateResource, IEnumerable<string> privileges = null)
        {
            entity.FreqWeeks = updateResource.FreqWeeks;
        }

        protected override Expression<Func<BomFrequencyWeeks, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            BomFrequencyWeeks entity,
            BomFrequencyWeeksResource resource,
            BomFrequencyWeeksResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(BomFrequencyWeeks entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}
