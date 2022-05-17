namespace Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Linn.Common.Authorisation;
    using Linn.Common.Domain.Exceptions;
    using Linn.Common.Email;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PurchaseOrderReqService : IPurchaseOrderReqService
    {
        private readonly string appRoot;

        private readonly IAuthorisationService authService;

        private readonly IEmailService emailService;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IPurchaseOrderAutoOrderPack purchaseOrderAutoOrderPack;

        private readonly IPurchaseOrderReqsPack purchaseOrderReqsPack;

        private readonly IPurchaseOrdersPack purchaseOrdersPack;

        private readonly ICurrencyPack currencyPack;

        private readonly IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey> reqsStateChangeRepository;

        public PurchaseOrderReqService(
            string appRoot,
            IAuthorisationService authService,
            IPurchaseOrderReqsPack purchaseOrderReqsPack,
            IRepository<Employee, int> employeeRepository,
            IEmailService emailService,
            IRepository<PurchaseOrderReqStateChange, PurchaseOrderReqStateChangeKey> reqsStateChangeRepository,
            IPurchaseOrderAutoOrderPack purchaseOrderAutoOrderPack,
            IPurchaseOrdersPack purchaseOrdersPack,
            ICurrencyPack currencyPack)
        {
            this.authService = authService;
            this.purchaseOrderReqsPack = purchaseOrderReqsPack;
            this.employeeRepository = employeeRepository;
            this.emailService = emailService;
            this.reqsStateChangeRepository = reqsStateChangeRepository;
            this.appRoot = appRoot;
            this.purchaseOrderAutoOrderPack = purchaseOrderAutoOrderPack;
            this.purchaseOrdersPack = purchaseOrdersPack;
            this.currencyPack = currencyPack;
        }

        public void Authorise(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId)
        {
            var stage = !entity.AuthorisedById.HasValue ? "AUTH1" : "AUTH2";

            if (stage == "AUTH1" && entity.State != "AUTHORISE WAIT")
            {
                throw new UnauthorisedActionException(
                    "Cannot authorise a req that is not in state 'AUTHORISE WAIT'. Please make sure the req is saved in this state and try again");
            }

            if (stage == "AUTH2" && entity.State != "AUTHORISE 2ND WAIT")
            {
                throw new UnauthorisedActionException(
                    "Cannot 2nd authorise a req that is not in state 'AUTHORISE 2ND WAIT'. Please make sure the req is saved in this state and try again");
            }

            if (!entity.TotalReqPrice.HasValue)
            {
                throw new UnauthorisedActionException("Cannot authorise a req that has no value");
            }

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

            if (stage == "AUTH1")
            {
                entity.State = allowedToAuthoriseResult.NewState;
                entity.AuthorisedById = currentUserId;
            }
            else
            {
                entity.State = allowedToAuthoriseResult.NewState;
                entity.SecondAuthById = currentUserId;
            }
        }

        public void Cancel(PurchaseOrderReq entity, IEnumerable<string> privileges)
        {
            var stateChangeAllowed = this.StateChangeAllowed(entity.State, "CANCELLED");
            if (!stateChangeAllowed)
            {
                throw new IllegalPoReqStateChangeException($"Cannot cancel req from state '{entity.State}'");
            }

            entity.State = "CANCELLED";
        }

        public PurchaseOrderReq Create(PurchaseOrderReq entity, IEnumerable<string> privileges)
        {
            if (entity.State != "DRAFT" && entity.State != "AUTHORISE WAIT")
            {
                throw new IllegalPoReqStateChangeException(
                    "Cannot create new PO req into state other than Draft or Authorise Wait");
            }

            return entity;
        }

        public void CreateOrderFromReq(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId)
        {
            if (entity.State != "ORDER WAIT")
            {
                throw new UnauthorisedActionException(
                    "Cannot create order from a req that is not in state 'ORDER WAIT'. Please make sure the req is saved in this state and try again");
            }

            if (!entity.AuthorisedById.HasValue)
            {
                throw new UnauthorisedActionException("Cannot create order from a req that has not been authorised");
            }

            if (!entity.TotalReqPrice.HasValue)
            {
                throw new UnauthorisedActionException("Cannot create order from a req without value for price");
            }

            var totalInBaseCurr = this.currencyPack.CalculateBaseValueFromCurrencyValue(entity.CurrencyCode, entity.TotalReqPrice.Value);

            var authAllowed = this.purchaseOrdersPack.OrderCanBeAuthorisedBy(
                null,
                null,
                currentUserId,
                totalInBaseCurr,
                entity.PartNumber,
                "PO");

            var createMiniOrderResult = this.purchaseOrderAutoOrderPack.CreateMiniOrderFromReq(
                entity.NominalCode,
                entity.DepartmentCode,
                entity.RequestedById,
                currentUserId,
                entity.Description,
                entity.QuoteRef,
                entity.RemarksForOrder,
                entity.PartNumber,
                entity.SupplierId,
                entity.Qty,
                entity.DateRequired,
                entity.UnitPrice,
                authAllowed);

            if (createMiniOrderResult.Success)
            {
                entity.TurnedIntoOrderById = currentUserId;
                entity.OrderNumber = createMiniOrderResult.OrderNumber;
                entity.State = this.GetNextState(entity.State, true);
            }
            else
            {
                throw new DomainException(createMiniOrderResult.Message);
            }
        }

        public ProcessResult CheckIfSigningLimitCanAuthorisePurchaseOrder(PurchaseOrderReq entity, int currentUserId)
        {
            if (!entity.TotalReqPrice.HasValue)
            {
                throw new DomainException("No req value set so cannot check if signing limit is enough");
            }

            var totalInBaseCurr = this.currencyPack.CalculateBaseValueFromCurrencyValue(entity.CurrencyCode, entity.TotalReqPrice.Value);

            var authAllowed = this.purchaseOrdersPack.OrderCanBeAuthorisedBy(
                null,
                null,
                currentUserId,
                totalInBaseCurr,
                entity.PartNumber,
                "PO");

            var message = authAllowed
                              ? "You can authorise this with your signing limit"
                              : $"Your signing limit will not cover this req (£{totalInBaseCurr}). The order will be created unauthorised";

            return new ProcessResult(authAllowed, message);
        }

        public void FinanceApprove(PurchaseOrderReq entity, IEnumerable<string> privileges, int currentUserId)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PurchaseOrderReqFinanceCheck, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to perform finance sign off on PO Reqs");
            }

            if (entity.State != "FINANCE WAIT")
            {
                throw new UnauthorisedActionException(
                    "Cannot authorise a req that is not in state 'FINANCE WAIT'. Please make sure the req is saved in this state and try again");
            }

            entity.FinanceCheckById = currentUserId;
            entity.State = this.GetNextState(entity.State, true);
        }

        public ProcessResult SendAuthorisationRequestEmail(int currentUser, int toEmp, PurchaseOrderReq req)
        {
            var from = this.employeeRepository.FindById(currentUser);
            var to = this.employeeRepository.FindById(toEmp);
            var reqUrl = $"{this.appRoot}/purchasing/purchase-orders/reqs/{req.ReqNumber}";
            var body = $"{req.RequestedBy.FullName} has placed a request to purchase {req.Description}.\n"
                       + $"Please could you look at req number {req.ReqNumber} and authorise as appropriate at \n"
                       + $"{reqUrl}.\n\nThank you";
            try
            {
                this.emailService.SendEmail(
                    to.PhoneListEntry.EmailAddress.Trim(),
                    to.FullName,
                    new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                                {
                                    { "name", from.FullName }, { "address", from.PhoneListEntry.EmailAddress.Trim() }
                                }
                        },
                    null,
                    from.PhoneListEntry.EmailAddress.Trim(),
                    from.FullName,
                    $"Purchase Order Req {req.ReqNumber} requires authorisation",
                    body,
                    null,
                    string.Empty);

                return new ProcessResult(true, "Email Sent");
            }
            catch (Exception e)
            {
                return new ProcessResult
                           {
                               Success = false, Message = $"Error sending email. Error Message: {e.Message}"
                           };
            }
        }

        public ProcessResult SendEmails(int sender, string to, int reqNumber, Stream pdfAttachment)
        {
            var from = this.employeeRepository.FindById(sender);
            try
            {
                this.emailService.SendEmail(
                    to.Trim(),
                    to.Trim(),
                    new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                                {
                                    { "name", from.FullName }, { "address", from.PhoneListEntry.EmailAddress.Trim() }
                                }
                        },
                    null,
                    from.PhoneListEntry.EmailAddress.Trim(),
                    from.FullName,
                    $"Purchase Order Req {reqNumber}",
                    $"Attached is a copy of Purchase Order Req {reqNumber}",
                    pdfAttachment,
                    $"Purchase Order Req {reqNumber}");

                return new ProcessResult(true, "Email Sent");
            }
            catch (Exception e)
            {
                return new ProcessResult
                           {
                               Success = false, Message = $"Error sending email. Error Message: {e.Message}"
                           };
            }
        }

        public ProcessResult SendFinanceCheckRequestEmail(int currentUser, int toEmp, PurchaseOrderReq req)
        {
            var from = this.employeeRepository.FindById(currentUser);
            var to = this.employeeRepository.FindById(toEmp);
            var reqUrl = $"{this.appRoot}/purchasing/purchase-orders/reqs/{req.ReqNumber}";
            var body = $"{req.RequestedBy.FullName} has placed a request to purchase {req.Description}.\n"
                       + $"Please could you look at req number {req.ReqNumber} and authorise for finance as appropriate at \n"
                       + $"{reqUrl}.\n\nThank you";
            try
            {
                this.emailService.SendEmail(
                    to.PhoneListEntry.EmailAddress.Trim(),
                    to.FullName,
                    new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                                {
                                    { "name", from.FullName }, { "address", from.PhoneListEntry.EmailAddress.Trim() }
                                }
                        },
                    null,
                    from.PhoneListEntry.EmailAddress.Trim(),
                    from.FullName,
                    $"Purchase Order Req {req.ReqNumber} requires finance authorisation",
                    body,
                    null,
                    string.Empty);

                return new ProcessResult(true, "Email Sent");
            }
            catch (Exception e)
            {
                return new ProcessResult
                           {
                               Success = false, Message = $"Error sending email. Error Message: {e.Message}"
                           };
            }
        }

        public void Update(PurchaseOrderReq entity, PurchaseOrderReq updatedEntity, IEnumerable<string> privileges)
        {
            if (entity.State != updatedEntity.State)
            {
                var stateChangeAllowed = this.StateChangeAllowed(entity.State, updatedEntity.State);
                if (!stateChangeAllowed)
                {
                    throw new IllegalPoReqStateChangeException(
                        $"Cannot change directly from state '{entity.State}' to '{updatedEntity.State}'");
                }
            }

            entity.State = updatedEntity.State;
            entity.ReqDate = updatedEntity.ReqDate;
            entity.OrderNumber = updatedEntity.OrderNumber;
            entity.PartNumber = updatedEntity.PartNumber;
            entity.Description = updatedEntity.Description;
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
            entity.NominalCode = updatedEntity.NominalCode;
            entity.RemarksForOrder = updatedEntity.RemarksForOrder;
            entity.InternalNotes = updatedEntity.InternalNotes;
            entity.DepartmentCode = updatedEntity.DepartmentCode;
        }

        private string GetNextState(string from, bool changeIsFromFunction = false)
        {
            var stateChange = this.reqsStateChangeRepository.FindBy(
                x => x.FromState == from && ((!changeIsFromFunction && x.UserAllowed == "Y")
                                             || (changeIsFromFunction && x.ComputerAllowed == "Y")));
            return stateChange.ToState;
        }

        private bool StateChangeAllowed(string from, string to, bool changeIsFromFunction = false)
        {
            var stateChange = this.reqsStateChangeRepository.FindBy(
                x => x.FromState == from && x.ToState == to
                                         && (x.UserAllowed == "Y" || (changeIsFromFunction && x.ComputerAllowed == "Y")));
            return stateChange != null;
        }
    }
}
