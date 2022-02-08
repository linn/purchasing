namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
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

        protected override Manufacturer CreateFromResource(ManufacturerResource resource, IEnumerable<string> privileges = null)        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Manufacturer entity,
            ManufacturerResource resource,
            ManufacturerResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(Manufacturer entity, IEnumerable<string> privileges = null)        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(Manufacturer entity, ManufacturerResource updateResource, IEnumerable<string> privileges = null)        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Manufacturer, bool>> SearchExpression(string searchTerm)
        {
            return x => x.Code.Contains(searchTerm.ToUpper()) || x.Name.Contains(searchTerm.ToUpper());
        }
    }
}
