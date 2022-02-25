namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
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

        private readonly IRepository<FullAddress, int> addressRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<VendorManager, string> vendorManagerRepository;

        private readonly IRepository<Planner, int> plannerRepository;
        
        private readonly IRepository<Contact, int> contactRepository;
        
        private readonly IRepository<Person, int> personRepository;
        
        private readonly IRepository<SupplierContact, int> supplierContactRepository;

        public SupplierService(
            IAuthorisationService authService,
            IRepository<Supplier, int> supplierRepository,
            IRepository<Currency, string> currencyRepository,
            IRepository<PartCategory, string> partCategoryRepository,
            IRepository<SupplierOrderHoldHistoryEntry, int> supplierHoldHistories,
            IRepository<FullAddress, int> addressRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<VendorManager, string> vendorManagerRepository,
            IRepository<Planner, int> plannerRepository,
            IRepository<Contact, int> contactRepository,
            IRepository<Person, int> personRepository,
            IRepository<SupplierContact, int> supplierContactRepository)
        {
            this.authService = authService;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.partCategoryRepository = partCategoryRepository;
            this.supplierHoldHistories = supplierHoldHistories;
            this.addressRepository = addressRepository;
            this.employeeRepository = employeeRepository;
            this.vendorManagerRepository = vendorManagerRepository;
            this.plannerRepository = plannerRepository;
            this.contactRepository = contactRepository;
            this.personRepository = personRepository;
            this.supplierContactRepository = supplierContactRepository;
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
            current.DateClosed = updated.DateClosed;
            current.ReasonClosed = updated.ReasonClosed;
            current.Notes = updated.Notes;
            current.OrganisationId = updated.OrganisationId;


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

            current.OrderFullAddress = updated.OrderFullAddress != null
                                           ? this.addressRepository.FindById(updated.OrderFullAddress.Id)
                                           : null;

            current.InvoiceFullAddress = updated.InvoiceFullAddress != null
                                           ? this.addressRepository.FindById(updated.InvoiceFullAddress.Id)
                                           : null;

            current.Planner = updated.Planner != null
                                  ? this.plannerRepository.FindById(updated.Planner.Id)
                                  : null;

            current.VendorManager = updated.VendorManager != null
                                        ? this.vendorManagerRepository.FindById(updated.VendorManager.Id)
                                        : null;

            current.AccountController = updated.AccountController != null
                                            ? this.employeeRepository.FindById(updated.AccountController.Id)
                                            : null;

            current.ClosedBy = updated.ClosedBy != null
                                            ? this.employeeRepository.FindById(updated.ClosedBy.Id)
                                            : null;

            current.Contacts = this.UpdateContacts(updated.Contacts);
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

            candidate.OrderFullAddress = candidate.OrderFullAddress != null
                                             ? this.addressRepository.FindById(candidate.OrderFullAddress.Id)
                                             : null;

            candidate.InvoiceFullAddress = candidate.InvoiceFullAddress != null
                                               ? this.addressRepository.FindById(candidate.InvoiceFullAddress.Id)
                                               : null;

            candidate.Planner = candidate.Planner != null
                                               ? this.plannerRepository.FindById(candidate.Planner.Id)
                                               : null;

            candidate.VendorManager = candidate.VendorManager != null
                                    ? this.vendorManagerRepository.FindById(candidate.VendorManager.Id)
                                    : null;

            candidate.AccountController = candidate.AccountController != null
                                    ? this.employeeRepository.FindById(candidate.AccountController.Id)
                                    : null;
            candidate.OpenedBy = candidate.OpenedBy != null
                                              ? this.employeeRepository.FindById(candidate.OpenedBy.Id)
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

            if (candidate.AccountController == null)
            {
                errors.Add("Account Controller");
            }

            if (candidate.OrderFullAddress == null)
            {
                errors.Add("Order Addressee");
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

        private IEnumerable<SupplierContact> UpdateContacts(IEnumerable<SupplierContact> supplierContacts)
        {
            if (supplierContacts == null)
            {
                return null;
            }

            var result = new List<SupplierContact>();

            foreach (var supplierContact in supplierContacts)
            {
                var existingSupplierContact = this.supplierContactRepository.FindById(supplierContact.ContactId);

                if (existingSupplierContact != null)
                {
                    existingSupplierContact.IsMainInvoiceContact = supplierContact.IsMainInvoiceContact;
                    existingSupplierContact.IsMainOrderContact = supplierContact.IsMainOrderContact;

                    var contact = this.contactRepository.FindById(supplierContact.ContactId);
                    contact.PhoneNumber = supplierContact.Contact.PhoneNumber;
                    contact.EmailAddress = supplierContact.Contact.EmailAddress;
                   

                    var person = this.personRepository.FindById(supplierContact.Contact.Person.Id);

                    person.FirstName = supplierContact.Contact.Person.FirstName;
                    person.LastName = supplierContact.Contact.Person.LastName;

                    contact.Person = person;
                    existingSupplierContact.Contact = contact;

                    result.Add(existingSupplierContact);
                }
                else
                {
                    result.Add(supplierContact);
                }
            }

            return result;
        }
    }
}
