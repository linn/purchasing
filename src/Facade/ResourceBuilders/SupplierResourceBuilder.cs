namespace Linn.Purchasing.Facade.ResourceBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Facade;
    using Linn.Common.Resources;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;
    using Linn.Purchasing.Resources;

    public class SupplierResourceBuilder : IBuilder<Supplier>
    {
        private readonly IAuthorisationService authService;

        public SupplierResourceBuilder(IAuthorisationService authService)
        {
            this.authService = authService;
        }

        public SupplierResource Build(Supplier entity, IEnumerable<string> claims)
        {
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
                VendorManager = entity.VendorManager,
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
            yield return new LinkResource { Rel = "self", Href = this.GetLocation(model) };

            if (this.authService.HasPermissionFor(AuthorisedAction.SupplierUpdate, claims))
            {
                yield return new LinkResource { Rel = "edit", Href = $"{this.GetLocation(model)}/edit" };
            }
        }
    }
}
