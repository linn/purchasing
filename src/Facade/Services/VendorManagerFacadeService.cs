namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class VendorManagerFacadeService : FacadeResourceService<VendorManager, string, VendorManagerResource, VendorManagerResource>
    {
        private readonly IRepository<VendorManager, string> vendorManagerRepository;

        public VendorManagerFacadeService(
            IRepository<VendorManager, string> repository,
            ITransactionManager transactionManager,
            IBuilder<VendorManager> resourceBuilder)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.vendorManagerRepository = repository;
        }

        protected override VendorManager CreateFromResource(
            VendorManagerResource resource,
            IEnumerable<string> privileges = null)
        {
            var sameNameOrIdVendorManager  = this.vendorManagerRepository.FindBy(vm => vm.Id == resource.VmId || vm.UserNumber == resource.UserNumber);

            if (sameNameOrIdVendorManager == null)
            {
                var newVendorManager = new VendorManager
                                           {
                                               Id = resource.VmId,
                                               PmMeasured = resource.PmMeasured,
                                               UserNumber = resource.UserNumber
                                           };

                return newVendorManager;
            }

            throw new VendorManagerException("Check that Employee or Id are not used on other Vendor Managers");

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
            var sameNameVendorManager = this.vendorManagerRepository.FindBy(
                vm => vm.Id != updateResource.VmId && vm.UserNumber == updateResource.UserNumber);

            if (sameNameVendorManager == null)
            {
                entity.PmMeasured = updateResource.PmMeasured;
                entity.UserNumber = updateResource.UserNumber;
            }
            else
            {
                throw new VendorManagerException("Check that Employee or Id are not used on other Vendor Managers");
            }
        }
    }
}
