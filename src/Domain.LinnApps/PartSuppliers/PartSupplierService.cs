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

        private IRepository<Tariff, int> tariffRepository;

        private IRepository<PackagingGroup, int> packagingGroupRepository;

        public PartSupplierService(
            IAuthorisationService authService,
            IRepository<Currency, string> currencyRepository,
            IRepository<OrderMethod, string> orderMethodRepository,
            IRepository<Address, int> addressRepository,
            IRepository<Tariff, int> tariffRepository,
            IRepository<PackagingGroup, int> packagingGroupRepository)
        {
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.orderMethodRepository = orderMethodRepository;
            this.addressRepository = addressRepository;
            this.tariffRepository = tariffRepository;
            this.packagingGroupRepository = packagingGroupRepository;
        }

        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            // what if any nulls?
            if (current.OrderMethod.Name != updated.OrderMethod.Name)
            {
                current.OrderMethod = this.orderMethodRepository.FindById(updated.OrderMethod.Name);
            }

            if (current.Currency.Code != updated.Currency.Code)
            {
                current.Currency = this.currencyRepository.FindById(updated.Currency.Code);
            }

            if (current.DeliveryAddress?.Id != updated.DeliveryAddress?.Id)
            {
                current.DeliveryAddress = this.addressRepository.FindById(updated.DeliveryAddress.Id);
            }

            if (current.Tariff?.Id != updated.Tariff?.Id)
            {
                current.Tariff = updated.Tariff == null ? null : this.tariffRepository.FindById(updated.Tariff.Id);
            }

            if (current.PackagingGroup?.Id != updated.PackagingGroup?.Id)
            {
                current.PackagingGroup = updated.PackagingGroup == null 
                                             ? null : this.packagingGroupRepository.FindById(updated.PackagingGroup.Id);
            }

            current.SupplierDesignation = updated.SupplierDesignation;
            current.CurrencyUnitPrice = updated.CurrencyUnitPrice;
            current.OurCurrencyPriceToShowOnOrder = updated.OurCurrencyPriceToShowOnOrder;
            current.BaseOurUnitPrice = updated.BaseOurUnitPrice;
            current.MinimumOrderQty = updated.MinimumOrderQty;
            current.MinimumDeliveryQty = updated.MinimumDeliveryQty;
            current.OrderIncrement = updated.OrderIncrement;
            current.ReelOrBoxQty = updated.ReelOrBoxQty;
            current.LeadTimeWeeks = updated.LeadTimeWeeks;
            current.ContractLeadTimeWeeks = updated.ContractLeadTimeWeeks;
            current.OverbookingAllowed = updated.OverbookingAllowed;
            current.DamagesPercent = updated.DamagesPercent;
            current.WebAddress = updated.WebAddress;
            current.DeliveryInstructions = updated.DeliveryInstructions;
            updated.NotesForBuyer = updated.NotesForBuyer;
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
