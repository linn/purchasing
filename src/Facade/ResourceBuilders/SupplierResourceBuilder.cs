namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierResourceBuilder : IBuilder<Supplier>
    {
        private readonly IAuthorisationService authService;

        private readonly AddressResourceBuilder addressResourceBuilder;

        private readonly SupplierContactResourceBuilder supplierContactResourceBuilder;


        private readonly IRepository<SupplierContact, int> supplierContactRepository;

        public SupplierResourceBuilder(IAuthorisationService authService, IRepository<SupplierContact, int> supplierContactRepository)
        {
            this.authService = authService;
            this.addressResourceBuilder = new AddressResourceBuilder();
            this.supplierContactResourceBuilder = new SupplierContactResourceBuilder();
            this.supplierContactRepository = supplierContactRepository;
        }

        public SupplierResource Build(Supplier entity, IEnumerable<string> claims)
        {
            if (entity == null)
            {
                return new SupplierResource
                {
                    Links = this.BuildLinks(null, claims).ToArray()
                };
            }

            var supplierContact = this.supplierContactRepository.FindBy(
                    c => c.SupplierId == entity.SupplierId && c.MainOrderContact == "Y");

            return new SupplierResource
            {
                Id = entity.SupplierId,
                Name = entity.Name,
                PhoneNumber = entity.PhoneNumber,
                InvoiceContactMethod = entity.InvoiceContactMethod,
                LiveOnOracle = entity.LiveOnOracle,
                SuppliersReference = entity.SuppliersReference,
                WebAddress = entity.WebAddress,
                OrderContactMethod = entity.OrderContactMethod,
                VendorManagerId = entity.VendorManager?.Id,
                ApprovedCarrier = entity.ApprovedCarrier,
                CurrencyCode = entity.Currency?.Code,
                CurrencyName = entity.Currency?.Name,
                ExpenseAccount = entity.ExpenseAccount,
                InvoiceGoesToId = entity.InvoiceGoesTo?.SupplierId,
                InvoiceGoesToName = entity.InvoiceGoesTo?.Name,
                PaymentDays = entity.PaymentDays,
                PaymentMethod = entity.PaymentMethod,
                PaysInFc = entity.PaysInFc,
                AccountingCompany = entity.AccountingCompany,
                VatNumber = entity.VatNumber,
                PartCategory = entity.PartCategory?.Category,
                PartCategoryDescription = entity.PartCategory?.Description,
                OrderHold = entity.OrderHold,
                NotesForBuyer = entity.NotesForBuyer,
                DeliveryDay = entity.DeliveryDay,
                RefersToFcId = entity.RefersToFc?.SupplierId,
                RefersToFcName = entity.RefersToFc?.Name,
                PmDeliveryDaysGrace = entity.PmDeliveryDaysGrace,
                InvoiceAddressId = entity.InvoiceFullAddress?.Id,
                InvoiceFullAddress = entity.InvoiceFullAddress?.AddressString,
                OrderAddressId = entity.OrderAddress?.AddressId,
                OrderFullAddress = entity.OrderAddress?.FullAddress?.AddressString,
                PlannerId = entity.Planner?.Id,
                AccountControllerId = entity.AccountController?.Id,
                AccountControllerName = entity.AccountController?.FullName,
                OpenedById = entity.OpenedBy?.Id,
                DateOpened = entity.DateOpened.ToString("o"),
                OpenedByName = entity.OpenedBy?.FullName,
                ClosedById = entity.ClosedBy?.Id,
                ClosedByName = entity.ClosedBy?.FullName,
                DateClosed = entity.DateClosed?.ToString("o"),
                ReasonClosed = entity.ReasonClosed,
                Notes = entity.Notes,
                OrganisationId = entity.OrganisationId,
                OrderAddress = entity.OrderAddress != null ? this.addressResourceBuilder.Build(entity.OrderAddress, new List<string>()) : null,
                SupplierContact = supplierContact != null ? this.supplierContactResourceBuilder.Build(supplierContact, new List<string>()) : null,
                Links = this.BuildLinks(entity, claims).ToArray()
            };
        }

        public string GetLocation(Supplier p)
        {
            return $"/purchasing/suppliers/{p.SupplierId}";
        }

        object IBuilder<Supplier>.Build(Supplier entity, IEnumerable<string> claims) => this.Build(entity, claims);

        private IEnumerable<LinkResource> BuildLinks(Supplier model, IEnumerable<string> claims)
        {
            if (model != null)
            {
                yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };
            }

            var privileges = claims?.ToList();

            if (model != null && this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, privileges))
            {
                yield return new LinkResource { Rel = "edit", Href = $"{this.GetLocation(model)}/edit" };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.SupplierCreate, privileges))
            {
                yield return new LinkResource { Rel = "create", Href = $"/purchasing/suppliers/create" };
            }

            if (this.authService.HasPermissionFor(AuthorisedAction.SupplierHoldChange, privileges))
            {
                yield return new LinkResource { Rel = "hold", Href = $"/purchasing/suppliers/hold" };
            }
        }
    }
}
