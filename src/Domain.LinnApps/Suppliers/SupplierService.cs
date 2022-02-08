namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    public class SupplierService : ISupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<PartCategory, string> partCategoryRepository;

        private readonly IRepository<SupplierOrderHoldHistoryEntry, int> supplierHoldHistories;

        public SupplierService(
            IAuthorisationService authService,
            IRepository<Supplier, int> supplierRepository,
            IRepository<Currency, string> currencyRepository,
            IRepository<PartCategory, string> partCategoryRepository,
            IRepository<SupplierOrderHoldHistoryEntry, int> supplierHoldHistories)
        {
            this.authService = authService;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.partCategoryRepository = partCategoryRepository;
            this.supplierHoldHistories = supplierHoldHistories;
        }

        public void UpdateSupplier(Supplier current, Supplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Suppliers");
            }

            ValidateFields(updated);

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
            current.ExpenseAccount = updated.ExpenseAccount;
            current.PaymentDays = updated.PaymentDays;
            current.PaymentMethod = updated.PaymentMethod;
            current.PaysInFc = updated.PaysInFc;
           
            current.ApprovedCarrier = updated.ApprovedCarrier;
            current.AccountingCompany = updated.AccountingCompany;
            current.VatNumber = updated.VatNumber;
            current.OrderHold = updated.OrderHold;
            current.NotesForBuyer = updated.NotesForBuyer;
            current.DeliveryDay = updated.DeliveryDay;
            current.PmDeliveryDaysGrace = updated.PmDeliveryDaysGrace;


            current.InvoiceGoesTo = updated.InvoiceGoesTo != null
                                        ? this.supplierRepository.FindById(updated.InvoiceGoesTo.SupplierId)
                                        : null;
            current.Currency = updated.Currency != null
                                   ? this.currencyRepository.FindById(updated.Currency.Code)
                                   : null;

            current.PartCategory = updated.PartCategory != null
                                       ? this.partCategoryRepository.FindById(updated.PartCategory.Category)
                                       : null;
            current.RefersToFc = updated.RefersToFc != null
                                     ? this.supplierRepository.FindById(updated.RefersToFc.SupplierId)
                                     : null;
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
            candidate.PartCategory = candidate.PartCategory != null
                                         ? this.partCategoryRepository.FindById(candidate.PartCategory.Category)
                                         : null;

            candidate.RefersToFc = candidate.RefersToFc != null
                                       ? this.supplierRepository.FindById(candidate.RefersToFc.SupplierId)
                                       : null;
            ValidateFields(candidate);

            return candidate;
        }

        public Supplier ChangeSupplierHoldStatus(
            SupplierOrderHoldHistoryEntry data, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierHoldChange, privileges))
            {
                throw new UnauthorisedActionException("You do not have permission to change Supplier Hold Status");
            }

            Supplier supplier = this.supplierRepository.FindById(data.SupplierId);

            if (!string.IsNullOrEmpty(data.ReasonOnHold))
            {
                data.DateOnHold = DateTime.Today;
                this.supplierHoldHistories.Add(data);

                supplier.OrderHold = "Y";
            }
            else if (!string.IsNullOrEmpty(data.ReasonOffHold))
            {
                var entry = this.supplierHoldHistories.FilterBy(x => x.SupplierId == data.SupplierId && x.DateOffHold == null)
                    .OrderByDescending(x => x.Id).First();
                entry.DateOffHold = DateTime.Today;
                entry.ReasonOffHold = data.ReasonOffHold;
                entry.TakenOffHoldBy = data.TakenOffHoldBy;
                supplier.OrderHold = "N";
            }

            return supplier;
        }

        private static void ValidateFields(Supplier candidate)
        {
            var errors = new List<string>();

            if (candidate.SupplierId == 0)
            {
                errors.Add("Supplier Id");
            }

            if (string.IsNullOrEmpty(candidate.Name))
            {
                errors.Add("Supplier Name");
            }

            if (string.IsNullOrEmpty(candidate.InvoiceContactMethod))
            {
                errors.Add("Invoice Contact Method");
            }

            if (candidate.PaymentDays == 0)
            {
                errors.Add("Payment Days");
            }

            if (string.IsNullOrEmpty(candidate.PaymentMethod))
            {
                errors.Add("Payment Method");
            }

            if (errors.Any())
            {
                var msg = errors
                    .Aggregate(
                        "The inputs for the following fields are empty/invalid: ",
                        (current, error) => current + $"{error}, ");

                throw new SupplierException(msg);
            }
        }
    }
}
