namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;

    public class SupplierService : ISupplierService
    {
        private readonly IAuthorisationService authService;

        public SupplierService(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public void UpdateSupplier(Supplier current, Supplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Suppliers");
            }

            current.Name = updated.Name;
            current.Currency = updated.Currency;
            current.WebAddress = updated.WebAddress;
            current.VendorManager = updated.VendorManager;
            current.Planner = updated.Planner;
            current.InvoiceContactMethod = updated.InvoiceContactMethod;
            current.PhoneNumber = updated.PhoneNumber;
            current.OrderContactMethod = updated.OrderContactMethod;
            current.SuppliersReference = updated.SuppliersReference;
            current.LiveOnOracle = updated.LiveOnOracle;
            current.LedgerStream = updated.LedgerStream;
        }

        public Supplier CreateSupplier(Supplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create Suppliers");
            }

            return candidate;
        }
    }
}
