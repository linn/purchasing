namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Resources;

    public class UnitsOfMeasureService 
        : FacadeResourceService<UnitOfMeasure, string, UnitOfMeasureResource, UnitOfMeasureResource>
    {
        public UnitsOfMeasureService(IRepository<UnitOfMeasure, string> repository, ITransactionManager transactionManager, IBuilder<UnitOfMeasure> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override UnitOfMeasure CreateFromResource(UnitOfMeasureResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            UnitOfMeasure entity,
            UnitOfMeasureResource resource,
            UnitOfMeasureResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(UnitOfMeasure entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(UnitOfMeasure entity, UnitOfMeasureResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<UnitOfMeasure, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
