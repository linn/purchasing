namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PurchaseOrderReqFacadeService : FacadeFilterResourceService<PurchaseOrderReq, int,
        PurchaseOrderReqResource, PurchaseOrderReqResource, PurchaseOrderReqSearchResource>
    {
        private readonly IPurchaseOrderReqService domainService;

        public PurchaseOrderReqFacadeService(
            IRepository<PurchaseOrderReq, int> repository,
            ITransactionManager transactionManager,
            IBuilder<PurchaseOrderReq> resourceBuilder,
            IPurchaseOrderReqService domainService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
        }

        protected override PurchaseOrderReq CreateFromResource(
            PurchaseOrderReqResource resource,
            IEnumerable<string> privileges = null)
        {
            var newReq = this.BuildEntityFromResourceHelper(resource);
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
           this.domainService.Update(entity, updateEntity);
        }

        private PurchaseOrderReq BuildEntityFromResourceHelper(PurchaseOrderReqResource resource)
        {
            return new PurchaseOrderReq
                       {
                           ReqNumber = resource.ReqNumber,
                           State = resource.State,
                           ReqDate = DateTime.Parse(resource.ReqDate),
                           OrderNumber = resource.OrderNumber,
                           PartNumber = resource.PartNumber,
                           PartDescription = resource.PartDescription,
                           Qty = resource.Qty,
                           UnitPrice = resource.UnitPrice,
                           Carriage = resource.Carriage,
                           TotalReqPrice = resource.TotalReqPrice,
                           CurrencyCode = resource.CurrencyCode,
                           SupplierId = resource.SupplierId,
                           SupplierName = resource.SupplierName,
                           SupplierContact = resource.SupplierContact,
                           AddressLine1 = resource.AddressLine1,
                           AddressLine2 = resource.AddressLine2,
                           AddressLine3 = resource.AddressLine3,
                           AddressLine4 = resource.AddressLine4,
                           PostCode = resource.PostCode,
                           CountryCode = resource.Country.CountryCode,
                           PhoneNumber = resource.PhoneNumber,
                           QuoteRef = resource.QuoteRef,
                           Email = resource.Email,
                           DateRequired =
                               !string.IsNullOrEmpty(resource.DateRequired)
                                   ? DateTime.Parse(resource.DateRequired)
                                   : null,
                           RequestedBy = resource.RequestedBy.Id,
                           AuthorisedBy = resource.AuthorisedBy.Id,
                           SecondAuthBy = resource.SecondAuthBy.Id,
                           FinanceCheckBy = resource.FinanceCheckBy.Id,
                           TurnedIntoOrderBy = resource.TurnedIntoOrderBy.Id,
                           Nominal = resource.Nominal,
                           RemarksForOrder = resource.RemarksForOrder,
                           InternalNotes = resource.InternalNotes,
                           Department = resource.Department
                       };
        }
    }
}
