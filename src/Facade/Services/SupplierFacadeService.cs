namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;
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
            var candidate = this.BuildEntityFromResourceHelper(resource);
            candidate.SupplierId = this.databaseService.GetNextVal("SUPPLIER_SEQ");
            candidate.OrganisationId = this.databaseService.GetNextVal("ORG_SEQ");
            candidate.OpenedBy = resource.OpenedById.HasValue
                ? new Employee { Id = (int)resource.OpenedById } : null;
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
            var updated = this.BuildEntityFromResourceHelper(updateResource);

            updated.SupplierId = entity.SupplierId;

            this.domainService.UpdateSupplier(entity, updated, privileges);
        }

        protected override Expression<Func<Supplier, bool>> SearchExpression(string searchTerm)
        {
            return s => s.SupplierId.ToString().Contains(searchTerm) || s.Name.Contains(searchTerm.ToUpper());
        }

        private Supplier BuildEntityFromResourceHelper(SupplierResource resource)
        {
            var supplier = new Supplier
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
                           OrderAddress = resource.OrderAddressId.HasValue
                                                  ? new Address { AddressId = (int)resource.OrderAddressId } : null,
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
                           SupplierContacts = resource.SupplierContacts?.Select(c => new SupplierContact
                                                                          {
                                                                              SupplierId = resource.Id,
                                                                              ContactId = c.Id > 0 ? c.Id : this.databaseService.GetIdSequence("CONT_SEQ"),
                                                                              IsMainInvoiceContact = c.IsMainInvoiceContact,
                                                                              IsMainOrderContact = c.IsMainOrderContact,
                                                                              EmailAddress = c.EmailAddress,
                                                                              Comments = c.Comments,
                                                                              JobTitle = c.JobTitle,
                                                                              MobileNumber = c.MobileNumber,
                                                                              PhoneNumber = c.PhoneNumber,
                                                                              Person =
                                                                                  new Person
                                                                                      {
                                                                                          Id = c.PersonId.GetValueOrDefault() > 0 
                                                                                              ? c.PersonId.GetValueOrDefault() 
                                                                                              : this.databaseService.GetNextVal("PERS_SEQ"), 
                                                                                          FirstName = c.FirstName, 
                                                                                          LastName = c.LastName
                                                                                      }
                                                                          }),
                           Group = resource.GroupId.HasValue 
                                       ? new SupplierGroup { Id = (int)resource.GroupId } : null
                        };
         
            return supplier;
        }
    }
}
