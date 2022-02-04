namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class SupplierService : ISupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<Currency, string> currencyRepository;

        public SupplierService(
            IAuthorisationService authService,
            IRepository<Supplier, int> supplierRepository,
            IRepository<Currency, string> currencyRepository)
        {
            this.authService = authService;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
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
            current.InvoiceGoesTo = updated.InvoiceGoesTo != null
                                        ? this.supplierRepository.FindById(updated.InvoiceGoesTo.SupplierId)
                                        : null;
            current.ExpenseAccount = updated.ExpenseAccount;
            current.PaymentDays = updated.PaymentDays;
            current.PaymentMethod = updated.PaymentMethod;
            current.PaysInFc = updated.PaysInFc;
            current.Currency = updated.Currency != null
                                        ? this.currencyRepository.FindById(updated.Currency.Code)
                                        : null;
            current.ApprovedCarrier = updated.ApprovedCarrier;

        }

        public Supplier CreateSupplier(Supplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create Suppliers");
            }

            candidate.InvoiceGoesTo = candidate.InvoiceGoesTo != null
                                           ? this.supplierRepository.FindById(candidate.InvoiceGoesTo.SupplierId)
                                           : null;
            candidate.Currency = candidate.Currency != null
                                     ? this.currencyRepository.FindById(candidate.Currency.Code)
                                     : null;
            return candidate;
        }
    }
}
