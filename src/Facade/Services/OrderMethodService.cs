namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class OrderMethodService : FacadeResourceService<OrderMethod, string, OrderMethodResource, OrderMethodResource>
    {
        public OrderMethodService(IRepository<OrderMethod, string> repository, ITransactionManager transactionManager, IBuilder<OrderMethod> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override OrderMethod CreateFromResource(OrderMethodResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            OrderMethod entity,
            OrderMethodResource resource,
            OrderMethodResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(OrderMethod entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(OrderMethod entity, OrderMethodResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<OrderMethod, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
