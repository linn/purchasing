namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    public class PartSupplierService : IPartSupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<OrderMethod, string> orderMethodRepository;

        private readonly IRepository<Address, int> addressRepository;

        public PartSupplierService(
            IAuthorisationService authService,
            IRepository<Currency, string> currencyRepository,
            IRepository<OrderMethod, string> orderMethodRepository,
            IRepository<Address, int> addressRepository)
        {
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.orderMethodRepository = orderMethodRepository;
            this.addressRepository = addressRepository;
        }

        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            // only do db lookups for these fields if they have actually been updated to save time
            if (current.OrderMethod.Name != updated.OrderMethod.Name)
            {
                current.OrderMethod = this.orderMethodRepository.FindById(updated.OrderMethod.Name);
            }

            if (current.Currency.Code != updated.Currency.Code)
            {
                current.Currency = this.currencyRepository.FindById(updated.Currency.Code);
            }

            if (current.DeliveryAddress.Id != updated.DeliveryAddress.Id)
            {
                current.DeliveryAddress = this.addressRepository.FindById(updated.DeliveryAddress.Id);
            }

            current.SupplierDesignation = updated.SupplierDesignation;
            current.CurrencyUnitPrice = updated.CurrencyUnitPrice;
            current.OurCurrencyPriceToShowOnOrder = updated.OurCurrencyPriceToShowOnOrder;
            current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            current.MinimumOrderQty = updated.MinimumOrderQty;
            current.MinimumDeliveryQty = updated.MinimumDeliveryQty;
            current.OrderIncrement = updated.OrderIncrement;
            current.ReelOrBoxQty = updated.ReelOrBoxQty;
        }

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            return candidate;
        }
    }
}
