namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;

    using PagedList.Core;

    public class CurrencyFacadeService : FacadeResourceService<Currency, string, CurrencyResource, CurrencyResource>
    {
        public CurrencyFacadeService(IRepository<Currency, string> repository, ITransactionManager transactionManager, IBuilder<Currency> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Currency CreateFromResource(CurrencyResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Currency entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Currency entity, CurrencyResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Currency, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
