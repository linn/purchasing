namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;

    public class PurchaseOrderReqService : IPurchaseOrderReqService
    {
        private readonly IAuthorisationService authService;

        public PurchaseOrderReqService(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create PO Reqs");
            }

            return entity;
        }

        public void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update PO Reqs");
            }

            entity.ReqNumber = updatedEntity.ReqNumber;
            entity.State = updatedEntity.State;
            entity.ReqDate = updatedEntity.ReqDate;
            entity.OrderNumber = updatedEntity.OrderNumber;
            entity.PartNumber = updatedEntity.PartNumber;
            entity.PartDescription = updatedEntity.PartDescription;
            entity.Qty = updatedEntity.Qty;
            entity.UnitPrice = updatedEntity.UnitPrice;
            entity.Carriage = updatedEntity.Carriage;
            entity.TotalReqPrice = updatedEntity.TotalReqPrice;
            entity.CurrencyCode = updatedEntity.CurrencyCode;
            entity.SupplierId = updatedEntity.SupplierId;
            entity.SupplierName = updatedEntity.SupplierName;
            entity.SupplierContact = updatedEntity.SupplierContact;
            entity.AddressLine1 = updatedEntity.AddressLine1;
            entity.AddressLine2 = updatedEntity.AddressLine2;
            entity.AddressLine3 = updatedEntity.AddressLine3;
            entity.AddressLine4 = updatedEntity.AddressLine4;
            entity.PostCode = updatedEntity.PostCode;
            entity.CountryCode = updatedEntity.Country.CountryCode;
            entity.PhoneNumber = updatedEntity.PhoneNumber;
            entity.QuoteRef = updatedEntity.QuoteRef;
            entity.Email = updatedEntity.Email;
            entity.DateRequired = updatedEntity.DateRequired;
            entity.RequestedBy = updatedEntity.RequestedBy;
            entity.AuthorisedBy = updatedEntity.AuthorisedBy;
            entity.SecondAuthBy = updatedEntity.SecondAuthBy;
            entity.FinanceCheckBy = updatedEntity.FinanceCheckBy;
            entity.TurnedIntoOrderBy = updatedEntity.TurnedIntoOrderBy;
            entity.Nominal = updatedEntity.Nominal;
            entity.RemarksForOrder = updatedEntity.RemarksForOrder;
            entity.InternalNotes = updatedEntity.InternalNotes;
            entity.Department = updatedEntity.Department;
        }
    }
}
