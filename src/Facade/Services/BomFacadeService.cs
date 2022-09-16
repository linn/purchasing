namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Boms;
    using Linn.Purchasing.Resources;

    public class BomFacadeService : FacadeResourceService<Bom, int, BomResource, BomResource>
    {
        public BomFacadeService(IRepository<Bom, int> repository, ITransactionManager transactionManager, IBuilder<Bom> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Bom CreateFromResource(BomResource resource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Bom entity, BomResource updateResource, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Bom, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(string actionType, int userNumber, Bom entity, BomResource resource, BomResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Bom entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }
    }
}

