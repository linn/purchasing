namespace Linn.Purchasing.Domain.LinnApps.Suppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers.Exceptions;

    using MoreLinq;

    public class SupplierService : ISupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<PartCategory, string> partCategoryRepository;

        private readonly IRepository<SupplierOrderHoldHistoryEntry, int> supplierHoldHistories;

        private readonly IRepository<FullAddress, int> fullAddressRepository;

        private readonly IRepository<Address, int> addressRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<SupplierGroup, int> groupRepository;

        private readonly IRepository<VendorManager, string> vendorManagerRepository;

        private readonly IRepository<Planner, int> plannerRepository;
        
        private readonly IRepository<Person, int> personRepository;
        
        private readonly IRepository<SupplierContact, int> supplierContactRepository;

        private readonly IRepository<Organisation, int> orgRepository;

        private readonly ISupplierPack supplierPack;

        public SupplierService(
            IAuthorisationService authService,
            IRepository<Supplier, int> supplierRepository,
            IRepository<Currency, string> currencyRepository,
            IRepository<PartCategory, string> partCategoryRepository,
            IRepository<SupplierOrderHoldHistoryEntry, int> supplierHoldHistories,
            IRepository<FullAddress, int> fullAddressRepository,
            IRepository<Address, int> addressRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<VendorManager, string> vendorManagerRepository,
            IRepository<Planner, int> plannerRepository,
            IRepository<Person, int> personRepository,
            IRepository<SupplierContact, int> supplierContactRepository,
            IRepository<SupplierGroup, int> groupRepository,
            IRepository<Organisation, int> orgRepository,
            ISupplierPack supplierPack)
        {
            this.authService = authService;
            this.supplierRepository = supplierRepository;
            this.currencyRepository = currencyRepository;
            this.partCategoryRepository = partCategoryRepository;
            this.supplierHoldHistories = supplierHoldHistories;
            this.fullAddressRepository = fullAddressRepository;
            this.addressRepository = addressRepository;
            this.employeeRepository = employeeRepository;
            this.vendorManagerRepository = vendorManagerRepository;
            this.plannerRepository = plannerRepository;
            this.personRepository = personRepository;
            this.supplierContactRepository = supplierContactRepository;
            this.groupRepository = groupRepository;
            this.orgRepository = orgRepository;
            this.supplierPack = supplierPack;
        }

        public void UpdateSupplier(Supplier current, Supplier updated, IEnumerable<string> privileges)
        {
            var privilegesList = privileges.ToList();
            if (!string.IsNullOrEmpty(updated.ReasonClosed))
            {
                if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierClose, privilegesList))
                {
                    throw new UnauthorisedActionException("You are not authorised to close a supplier");
                }

                current.DateClosed = DateTime.Today;
                current.ReasonClosed = updated.ReasonClosed;
                current.ClosedBy = updated.ClosedBy;
            }


            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privilegesList))
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
            current.Country = updated.Country;
            current.ApprovedCarrier = updated.ApprovedCarrier;
            current.AccountingCompany = updated.AccountingCompany;
            current.VatNumber = updated.VatNumber;
            current.OrderHold = updated.OrderHold;
            current.NotesForBuyer = updated.NotesForBuyer;
            current.DeliveryDay = updated.DeliveryDay;
            current.PmDeliveryDaysGrace = updated.PmDeliveryDaysGrace;
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

            current.OrderAddress = updated.OrderAddress != null
                                           ? this.addressRepository.FindById(updated.OrderAddress.AddressId)
                                           : null;

            current.InvoiceFullAddress = updated.InvoiceFullAddress != null
                                           ? this.fullAddressRepository.FindById(updated.InvoiceFullAddress.Id)
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

            current.Group = updated.Group != null
                                   ? this.groupRepository.FindById(updated.Group.Id)
                                   : null;

            current.SupplierContacts = this.UpdateContacts(updated.SupplierContacts);
        }

        public Supplier CreateSupplier(Supplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to create Suppliers");
            }

            candidate.SupplierId = this.supplierPack.GetNextSupplierKey();

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

            candidate.OrderAddress = candidate.OrderAddress != null
                                             ? this.addressRepository.FindById(candidate.OrderAddress.AddressId)
                                             : null;

            candidate.InvoiceFullAddress = candidate.InvoiceFullAddress != null
                                               ? this.fullAddressRepository.FindById(candidate.InvoiceFullAddress.Id)
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

            candidate.Group = candidate.Group != null
                                     ? this.groupRepository.FindById(candidate.Group.Id)
                                     : null;

            if (candidate.SupplierContacts != null)
            {

                var contacts = candidate.SupplierContacts.Select(c => new SupplierContact
                                                           {
                                                               Comments = c.Comments,
                                                               ContactId = c.ContactId,
                                                               EmailAddress = c.EmailAddress,
                                                               SupplierId = candidate.SupplierId,
                                                               IsMainInvoiceContact = c.IsMainInvoiceContact,
                                                               IsMainOrderContact = c.IsMainOrderContact,
                                                               JobTitle = c.JobTitle,
                                                               DateCreated = DateTime.Today,
                                                               MobileNumber = c.MobileNumber,
                                                               PhoneNumber = c.PhoneNumber,
                                                               Person = c.Person
                                                           });

                candidate.SupplierContacts = this.UpdateContacts(contacts);
            }
            
            ValidateFields(candidate);
            candidate.Name = candidate.Name.ToUpper();
            this.orgRepository.Add(new Organisation
                                       {
                                            OrgId = candidate.OrganisationId,
                                            AddressId = candidate.OrderAddress.AddressId,
                                            DateCreated = DateTime.Today,
                                            PhoneNumber = candidate.PhoneNumber,
                                            WebAddress = candidate.WebAddress,
                                            Title = candidate.Name.ToUpper()
                                       });
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

            if (candidate.OrderAddress == null)
            {
                errors.Add("Order Addressee");
            }

            if (candidate.SupplierContacts == null)
            {
                errors.Add("Supplier Contacts is empty");
            }
            else
            {
                if (!candidate.SupplierContacts.Any(c => 
                        !string.IsNullOrEmpty(c.IsMainInvoiceContact) && c.IsMainInvoiceContact.Equals("Y")))
                {
                    errors.Add("Main Invoice Contact");
                }
                else if (candidate.SupplierContacts.Count(x => x.IsMainInvoiceContact == "Y") > 1)
                {
                    errors.Add("Cannot have more than one Main Invoice Contact");
                }

                if (!candidate.SupplierContacts.Any(c =>
                        !string.IsNullOrEmpty(c.IsMainOrderContact) && c.IsMainOrderContact.Equals("Y")))
                {
                    errors.Add("Main Order Contact");
                }
                else if (candidate.SupplierContacts.Count(x => x.IsMainOrderContact == "Y") > 1)
                {
                    errors.Add("Cannot have more than one Main Order Contact");
                }
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

                    existingSupplierContact.PhoneNumber = supplierContact.PhoneNumber;
                    existingSupplierContact.EmailAddress = supplierContact.EmailAddress;
                    existingSupplierContact.JobTitle = supplierContact.JobTitle;
                    existingSupplierContact.Comments = supplierContact.Comments;
                    var person = this.personRepository.FindById(supplierContact.Person.Id);

                    person.FirstName = supplierContact.Person.FirstName;
                    person.LastName = supplierContact.Person.LastName;

                    existingSupplierContact.Person = person;
                    result.Add(existingSupplierContact);
                }
                else
                {
                    supplierContact.Person.DateCreated = DateTime.Today;
                    this.personRepository.Add(supplierContact.Person);
                    supplierContact.SupplierId = supplierContact.SupplierId;
                    supplierContact.DateCreated = DateTime.Today;
                    supplierContact.Comments = supplierContact.Comments;
                    this.supplierContactRepository.Add(supplierContact);
                    result.Add(supplierContact);
                }
            }

            return result;
        }
    }
}
