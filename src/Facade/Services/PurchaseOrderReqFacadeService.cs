namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Resources;
    using Linn.Purchasing.Resources.SearchResources;

    public class PurchaseOrderReqFacadeService : FacadeFilterResourceService<PurchaseOrderReq, int,
        PurchaseOrderReqResource, PurchaseOrderReqResource, PurchaseOrderReqSearchResource>
    {
        private readonly IPurchaseOrderReqService domainService;

        private readonly IDatabaseService databaseService;

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
            //todo this.domainService.cancel
        }

        protected override Expression<Func<PurchaseOrderReq, bool>> FilterExpression(
            PurchaseOrderReqSearchResource searchResource)
        {
            return x => (string.IsNullOrWhiteSpace(searchResource.ReqNumber) || x.ReqNumber.ToString().Contains(searchResource.ReqNumber))
                && (string.IsNullOrWhiteSpace(searchResource.Part) || x.PartNumber.ToUpper().Contains(searchResource.Part.ToUpper()))
                && (string.IsNullOrWhiteSpace(searchResource.Supplier) || x.SupplierId.ToString().Contains(searchResource.Supplier)
                    || x.SupplierName.ToUpper().ToString().Contains(searchResource.Supplier.ToUpper()));
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            PurchaseOrderReq entity,
            PurchaseOrderReqResource resource,
            PurchaseOrderReqResource updateResource)
        {
            return;
        }

        protected override Expression<Func<PurchaseOrderReq, bool>> SearchExpression(string searchTerm)
        {
            throw new NotImplementedException();
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
                           CurrencyCode = resource.Currency.Code,
                           SupplierId = resource.Supplier.Id,
                           SupplierName = resource.Supplier.Name,
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
                           RequestedById = resource.RequestedBy.Id,
                           AuthorisedById = resource.AuthorisedBy?.Id,
                           SecondAuthById = resource.SecondAuthBy?.Id,
                           FinanceCheckById = resource.FinanceCheckBy?.Id,
                           TurnedIntoOrderById = resource.TurnedIntoOrderBy?.Id,
                           NominalCode = resource.Nominal?.NominalCode,
                           RemarksForOrder = resource.RemarksForOrder,
                           InternalNotes = resource.InternalNotes,
                           DepartmentCode = resource.Department?.DepartmentCode
                       };
        }
    }
}
