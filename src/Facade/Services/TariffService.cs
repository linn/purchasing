namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class TariffService : FacadeResourceService<Tariff, int, TariffResource, TariffResource>
    {
        public TariffService(IRepository<Tariff, int> repository, ITransactionManager transactionManager, IBuilder<Tariff> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Tariff CreateFromResource(TariffResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Tariff entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Tariff entity, TariffResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Tariff, bool>> SearchExpression(string searchTerm)
        {
            return x => x.Code.Contains(searchTerm.ToUpper()) || x.Description.Contains(searchTerm.ToUpper());
        }
    }
}
