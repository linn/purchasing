namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrders
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PurchaseOrderReqService : IPurchaseOrderReqService
    {
        private readonly IAuthorisationService authService;

        private readonly IPurchaseOrderReqsPack purchaseOrderReqsPack;

        public PurchaseOrderReqService(
            IAuthorisationService authService,
            IPurchaseOrderReqsPack purchaseOrderReqsPack)
        {
            this.authService = authService;
            this.purchaseOrderReqsPack = purchaseOrderReqsPack;
        }

        public void Authorise(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqAuthorise, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to authorise PO Reqs");
            }

            if (entity.State != "AUTHORISE WAIT")
            {
                throw new UnauthorisedActionException("Cannot authorise a req that is not in state 'AUTHORISE WAIT'");
            }

            if (!entity.TotalReqPrice.HasValue)
            {
                throw new UnauthorisedActionException("Cannot authorise a req that has no value");
            }

            var stage = !entity.AuthorisedById.HasValue ? "AUTH1" : "AUTH2";

            // todo check if totalReqPrice is used on the old form and if any currency conversion is done
            var allowedToAuthoriseResult = this.purchaseOrderReqsPack.AllowedToAuthorise(
                stage,
                currentUserId,
                entity.TotalReqPrice.Value,
                entity.DepartmentCode,
                entity.State);

            if (!allowedToAuthoriseResult.Success)
            {
                throw new UnauthorisedActionException(allowedToAuthoriseResult.Message);
            }

            entity.State = allowedToAuthoriseResult.NewState;
            entity.AuthorisedById = currentUserId;
        }

        public void Cancel(PurchaseOrderReq entity, IEnumerable<string> privileges)
        {
            // todo in facade use delete or obselete resource? just return nothing from save to log
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to cancel PO Reqs");
            }

            var stateChangeAllowed = this.purchaseOrderReqsPack.StateChangeAllowed(entity.State, "CANCELLED");
            if (!stateChangeAllowed)
            {
                throw new IllegalPoReqStateChangeException($"Cannot cancel req from state '{entity.State}'");
            }

            entity.State = "CANCELLED";
        }

        public PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqCreate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create PO Reqs");
            }

            if (entity.State != "DRAFT" && entity.State != "AUTHORISE WAIT")
            {
                throw new UnauthorisedActionException(
                    "Cannot create new PO req into state other than Draft or Authorise Wait");
            }

            return entity;
        }

        public void FinanceApprove(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqFinanceCheck, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to perform finance sign off on PO Reqs");
            }

            if (entity.State != "FINANCE WAIT")
            {
                throw new UnauthorisedActionException("Cannot authorise a req that is not in state 'AUTHORISE WAIT'");
            }

            return;
            // todo find finance check equivalent of allowed to authorise checks
            //entity.FinanceCheckById = currentUserId;
        }

        public void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update PO Reqs");
            }

            var stateChangeAllowed = this.purchaseOrderReqsPack.StateChangeAllowed(entity.State, updatedEntity.State);
            if (!stateChangeAllowed)
            {
                throw new IllegalPoReqStateChangeException(
                    $"Cannot change directly from state '{entity.State}' to '{updatedEntity.State}'");
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
            entity.CountryCode = updatedEntity.CountryCode;
            entity.PhoneNumber = updatedEntity.PhoneNumber;
            entity.QuoteRef = updatedEntity.QuoteRef;
            entity.Email = updatedEntity.Email;
            entity.DateRequired = updatedEntity.DateRequired;
            entity.RequestedBy = updatedEntity.RequestedBy;
            entity.AuthorisedBy = updatedEntity.AuthorisedBy;
            entity.SecondAuthBy = updatedEntity.SecondAuthBy;
            entity.FinanceCheckBy = updatedEntity.FinanceCheckBy;
            entity.TurnedIntoOrderBy = updatedEntity.TurnedIntoOrderBy;
            entity.NominalCode = updatedEntity.NominalCode;
            entity.RemarksForOrder = updatedEntity.RemarksForOrder;
            entity.InternalNotes = updatedEntity.InternalNotes;
            entity.DepartmentCode = updatedEntity.DepartmentCode;
        }
    }
}
