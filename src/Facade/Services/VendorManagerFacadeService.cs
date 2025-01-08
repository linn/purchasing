namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class VendorManagerFacadeService : FacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource>
    {
        public VendorManagerFacadeService(
            IRepository<VendorManager, string> repository,
            ITransactionManager transactionManager,
            IBuilder<VendorManager> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
        }

        protected override VendorManager CreateFromResource(
            VendorManagerResource resource,
            IEnumerable<string> privileges = null)
        {
            var newVendorManager = new VendorManager
                                       {
                                           Id = resource.VmId,
                                           PmMeasured = resource.PmMeasured,
                                           UserNumber = resource.UserNumber,
                                           Employee = new Employee
                                                          {
                                                              FullName = resource.Name, Id = resource.UserNumber
                                                          }
                                       };

            return newVendorManager;
        }

        protected override void DeleteOrObsoleteResource(VendorManager entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            VendorManager entity,
            VendorManagerResource resource,
            VendorManagerResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<VendorManager, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            VendorManager entity,
            VendorManagerResource updateResource,
            IEnumerable<string> privileges = null)
        {
            entity.PmMeasured = updateResource.PmMeasured;
            entity.UserNumber = updateResource.UserNumber;
            entity.Employee = new Employee { FullName = updateResource.Name, Id = updateResource.UserNumber };
        }
    }
}
