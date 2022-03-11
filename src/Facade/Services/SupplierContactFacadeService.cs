//namespace Linn.Purchasing.Facade.Services
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq.Expressions;

//    using Linn.Common.Facade;
//    using Linn.Common.Persistence;
//    using Linn.Purchasing.Domain.LinnApps.Suppliers;
//    using Linn.Purchasing.Resources;

//    public class SupplierContactFacadeService : ISupplierContactFacadeService
//    {
//        private readonly ISupplierContactService domainService;

//        private readonly IBuilder<SupplierContact> resourceBuilder;

//        public SupplierContactFacadeService(
//            IRepository<SupplierContact, int> repository,
//            IBuilder<SupplierContact> resourceBuilder,
//            ISupplierContactService domainService)
//        {
//            this.domainService = domainService;
//            this.resourceBuilder = resourceBuilder;
//        }

//        public IResult<SupplierContactResource> GetMainContactForSupplier(int supplierId, IEnumerable<string> privileges)
//        {
//            var contact = this.domainService.GetMainContactForSupplier(supplierId);

//            if (contact == null)
//            {
//                return new NotFoundResult<SupplierContactResource>();
//            }

//            return new SuccessResult<SupplierContactResource>(this.resourceBuilder.Build(contact, privileges));
//        }
//    }
//}
