namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PurchaseOrderReqFacadeService : FacadeFilterResourceService<PurchaseOrderReq, int,
        PurchaseOrderReqResource, PurchaseOrderReqResource, PurchaseOrderReqSearchResource>
    {
        private readonly IPurchaseOrderReqService domainService;

        private IDatabaseService databaseService;

        public PurchaseOrderReqFacadeService(
            IRepository<PurchaseOrderReq, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrderReq> resourceBuilder,
            IPurchaseOrderReqService domainService,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.databaseService = databaseService;
        }

        protected override PurchaseOrderReq CreateFromResource(
            PurchaseOrderReqResource resource,
            IEnumerable<string> privileges = null)
        {
            var newReq = this.BuildEntityFromResourceHelper(resource);

            newReq.ReqNumber = this.databaseService.GetNextVal("BLUE_REQ_SEQ");

            return this.domainService.Create(newReq, privileges);
        }

        protected override void DeleteOrObsoleteResource(PurchaseOrderReq entity, IEnumerable<string> privileges = null)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PurchaseOrderReq, bool>> FilterExpression(
            PurchaseOrderReqSearchResource searchResource)
        {
            throw new NotImplementedException();
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrderReq entity,
            PurchaseOrderReqResource resource,
            PurchaseOrderReqResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<PurchaseOrderReq, bool>> SearchExpression(string searchTerm)
        {
            return x => x.OrderNumber.ToString().Contains(searchTerm);
        }

        protected override void UpdateFromResource(
            PurchaseOrderReq entity,
            PurchaseOrderReqResource updateResource,
            IEnumerable<string> privileges = null)
        {
           var updateEntity = this.BuildEntityFromResourceHelper(updateResource);
           updateEntity.ReqNumber = updateResource.ReqNumber;

           this.domainService.Update(entity, updateEntity, privileges);
        }

        private PurchaseOrderReq BuildEntityFromResourceHelper(PurchaseOrderReqResource resource)
        {
            return new PurchaseOrderReq
                       {
                           State = resource.State,
                           ReqDate = DateTime.Parse(resource.ReqDate),
                           OrderNumber = resource.OrderNumber,
                           PartNumber = resource.PartNumber,
                           PartDescription = resource.PartDescription,
                           Qty = resource.Qty,
                           UnitPrice = resource.UnitPrice,
                           Carriage = resource.Carriage,
                           TotalReqPrice = resource.TotalReqPrice,
                           Currency = new Currency { Code = resource.Currency.Code },
                           Supplier = new Supplier { SupplierId = resource.Supplier.Id, Name = resource.Supplier.Name },
                           SupplierName = resource.Supplier.Name,
                           SupplierContact = resource.SupplierContact,
                           AddressLine1 = resource.AddressLine1,
                           AddressLine2 = resource.AddressLine2,
                           AddressLine3 = resource.AddressLine3,
                           AddressLine4 = resource.AddressLine4,
                           PostCode = resource.PostCode,
                           Country = new Country { CountryCode = resource.Country.CountryCode },
                           PhoneNumber = resource.PhoneNumber,
                           QuoteRef = resource.QuoteRef,
                           Email = resource.Email,
                           DateRequired =
                               !string.IsNullOrEmpty(resource.DateRequired)
                                   ? DateTime.Parse(resource.DateRequired)
                                   : null,
                           RequestedBy = new Employee { Id = resource.RequestedBy.Id },
                           AuthorisedBy =
                               resource.AuthorisedBy != null ? new Employee { Id = resource.AuthorisedBy.Id } : null,
                           SecondAuthBy =
                               resource.SecondAuthBy != null ? new Employee { Id = resource.SecondAuthBy.Id } : null,
                           FinanceCheckBy =
                               resource.FinanceCheckBy != null
                                   ? new Employee { Id = resource.FinanceCheckBy.Id }
                                   : null,
                           TurnedIntoOrderBy =
                               resource.TurnedIntoOrderBy != null
                                   ? new Employee { Id = resource.TurnedIntoOrderBy.Id }
                                   : null,
                           Nominal =
                               resource.Nominal != null
                                   ? new Nominal
                                         {
                                             NominalCode = resource.Nominal.NominalCode,
                                             Description = resource.Nominal.Description
                                         }
                                   : null,
                           RemarksForOrder = resource.RemarksForOrder,
                           InternalNotes = resource.InternalNotes,
                           Department = resource.Department != null
                                            ? new Department
                                                  {
                                                      DepartmentCode = resource.Department.DepartmentCode,
                                                      Description = resource.Department.Description
                                                  }
                                            : null
                       };
        }
    }
}
