namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Resources;

    public class ManufacturerFacadeService : FacadeResourceService<Manufacturer, string, ManufacturerResource, ManufacturerResource>
    {
        public ManufacturerFacadeService(IRepository<Manufacturer, string> repository, ITransactionManager transactionManager, IBuilder<Manufacturer> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override Manufacturer CreateFromResource(ManufacturerResource resource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Manufacturer entity)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Manufacturer entity, ManufacturerResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Manufacturer, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
