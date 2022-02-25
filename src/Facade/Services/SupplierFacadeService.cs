﻿namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Proxy.LinnApps;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierFacadeService : FacadeResourceService<Supplier, int, SupplierResource, SupplierResource>
    {
        private readonly ISupplierService domainService;

        private readonly IDatabaseService databaseService;

        public SupplierFacadeService(
            IRepository<Supplier, int> repository, 
            ITransactionManager transactionManager, 
            IBuilder<Supplier> resourceBuilder,
            ISupplierService domainService,
            IDatabaseService databaseService)
            : base(repository, transactionManager, resourceBuilder)
        {
            this.domainService = domainService;
            this.databaseService = databaseService;
        }

        protected override Supplier CreateFromResource(
            SupplierResource resource, 
            IEnumerable<string> privileges = null)        
        {
            var candidate = BuildEntityFromResourceHelper(resource);
            candidate.SupplierId = this.databaseService.GetNextVal("SUPPLIER_SEQ");

            candidate.OpenedBy = resource.OpenedById.HasValue
                ? new Employee {Id = (int)resource.OpenedById } : null;
            candidate.DateOpened = DateTime.Today;
            return this.domainService.CreateSupplier(candidate, privileges);
        }

        protected override void SaveToLogTable(
            string actionType,
            int userNumber,
            Supplier entity,
            SupplierResource resource,
            SupplierResource updateResource)
        {
            throw new NotImplementedException();
        }

        protected override void DeleteOrObsoleteResource(
            Supplier entity, 
            IEnumerable<string> privileges = null)        
        {
            throw new NotImplementedException();
        }

        protected override void UpdateFromResource(
            Supplier entity, 
            SupplierResource updateResource, 
            IEnumerable<string> privileges = null)        
        {
            var updated = BuildEntityFromResourceHelper(updateResource);

            updated.SupplierId = entity.SupplierId;

            this.domainService.UpdateSupplier(entity, updated, privileges);
        }

        protected override Expression<Func<Supplier, bool>> SearchExpression(string searchTerm)
        {
            return s => s.SupplierId.ToString().Contains(searchTerm) || s.Name.Contains(searchTerm.ToUpper());
        }

        private static Supplier BuildEntityFromResourceHelper(SupplierResource resource)
        {
            return new Supplier
                       {
                           Name = resource.Name,
                           Currency = new Currency
                                          {
                                              Code = resource.CurrencyCode
                                          },
                           WebAddress = resource.WebAddress,
                           VendorManager = !string.IsNullOrEmpty(resource.VendorManagerId) 
                            ? new VendorManager { Id = resource.VendorManagerId } : null,
                           Planner = resource.PlannerId.HasValue 
                                         ? new Planner { Id = (int)resource.PlannerId } : null,
                           InvoiceContactMethod = resource.InvoiceContactMethod,
                           PhoneNumber = resource.PhoneNumber,
                           OrderContactMethod = resource.OrderContactMethod,
                           SuppliersReference = resource.SuppliersReference,
                           LiveOnOracle = resource.LiveOnOracle,
                           ExpenseAccount = resource.ExpenseAccount,
                           PaymentDays = resource.PaymentDays,
                           PaymentMethod = resource.PaymentMethod,
                           PaysInFc = resource.PaysInFc,
                           AccountingCompany = resource.AccountingCompany,
                           ApprovedCarrier = resource.ApprovedCarrier,
                           VatNumber = resource.VatNumber,
                           InvoiceGoesTo = resource.InvoiceGoesToId.HasValue 
                                               ? new Supplier { SupplierId = (int)resource.InvoiceGoesToId } : null,
                           PartCategory = !string.IsNullOrEmpty(resource.PartCategory)
                                          ? new PartCategory { Category = resource.PartCategory } : null,
                           OrderHold = resource.OrderHold,
                           NotesForBuyer = resource.NotesForBuyer,
                           DeliveryDay = resource.DeliveryDay,
                           RefersToFc = resource.RefersToFcId.HasValue
                            ? new Supplier { SupplierId = (int)resource.RefersToFcId } : null,
                           PmDeliveryDaysGrace = resource.PmDeliveryDaysGrace,
                           OrderFullAddress = resource.OrderAddressId.HasValue 
                                                  ? new FullAddress { Id = (int)resource.OrderAddressId } : null,
                           InvoiceFullAddress = resource.InvoiceAddressId.HasValue
                                                  ? new FullAddress { Id = (int)resource.InvoiceAddressId } : null,
                           AccountController = resource.AccountControllerId.HasValue
                               ? new Employee { Id = (int)resource.AccountControllerId } : null,
                           ClosedBy = resource.ClosedById.HasValue ? new Employee { Id = (int)resource.ClosedById }
                                        : null,
                           DateClosed = !string.IsNullOrEmpty(resource.DateClosed) 
                                        ? DateTime.Parse(resource.DateClosed) : null,
                           ReasonClosed = resource.ReasonClosed,
                           Notes = resource.Notes,
                           OrganisationId = resource.OrganisationId,
                           Contacts = resource.Contacts?.Select(c => new SupplierContact
                                                                          {
                                                                              Contact = new Contact
                                                                                  {
                                                                                      ContactId = c.Contact.ContactId,
                                                                                      Comments = c.Contact.Comments,
                                                                                      EmailAddress = c.Contact.EmailAddress,
                                                                                      JobTitle = c.Contact.JobTitle,
                                                                                      DateCreated = !string.IsNullOrEmpty(c.Contact.DateCreated)
                                                                                          ? DateTime.Parse(c.Contact.DateCreated) : null,
                                                                                      DateInvalid = !string.IsNullOrEmpty(c.Contact.DateInvalid)
                                                                                          ? DateTime.Parse(c.Contact.DateInvalid) : null,
                                                                                      MobileNumber = c.Contact.MobileNumber,
                                                                                      PhoneNumber = c.Contact.PhoneNumber,
                                                                                      Person = c.Contact.PersonId.HasValue 
                                                                                          ? new Person
                                                                                          {
                                                                                              FirstName = c.Contact.FirstName,
                                                                                              LastName = c.Contact.LastName,
                                                                                              Id = (int)c.Contact.PersonId
                                                                                          } 
                                                                                          : null
                                                                                  },
                                                                              ContactId = c.Contact.ContactId,
                                                                              IsMainInvoiceContact = c.IsMainInvoiceContact,
                                                                              IsMainOrderContact = c.IsMainOrderContact
                                                                          })
            };
        }
    }
}
