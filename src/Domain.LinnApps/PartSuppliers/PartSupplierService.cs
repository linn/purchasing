namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System.Collections.Generic;
    using System.Linq;

    using Amazon.SimpleEmail.Model;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;

    using Microsoft.VisualBasic;

    public class PartSupplierService : IPartSupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<OrderMethod, string> orderMethodRepository;

        private readonly IRepository<Address, int> addressRepository;

        private readonly IRepository<Tariff, int> tariffRepository;

        private readonly IRepository<PackagingGroup, int> packagingGroupRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Manufacturer, string> manufacturerRepository;

        public PartSupplierService(
            IAuthorisationService authService,
            IRepository<Currency, string> currencyRepository,
            IRepository<OrderMethod, string> orderMethodRepository,
            IRepository<Address, int> addressRepository,
            IRepository<Tariff, int> tariffRepository,
            IRepository<PackagingGroup, int> packagingGroupRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Manufacturer, string> manufacturerRepository)
        {
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.orderMethodRepository = orderMethodRepository;
            this.addressRepository = addressRepository;
            this.tariffRepository = tariffRepository;
            this.packagingGroupRepository = packagingGroupRepository;
            this.employeeRepository = employeeRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public void UpdatePartSupplier(PartSupplier current, PartSupplier updated, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

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
                current.DeliveryAddress = updated.DeliveryAddress == null ? null : this.addressRepository.FindById(updated.DeliveryAddress.Id);
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

            if (current.MadeInvalidBy?.Id != updated.MadeInvalidBy?.Id)
            {
                current.MadeInvalidBy = updated.MadeInvalidBy == null
                                             ? null : this.employeeRepository.FindById(updated.MadeInvalidBy.Id);
            }

            if (current.Manufacturer?.Code != updated.Manufacturer?.Code)
            {
                current.Manufacturer = updated.Manufacturer == null
                                            ? null : this.manufacturerRepository.FindById(updated.Manufacturer.Code);
            }

            current.DateInvalid = updated.DateInvalid;
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
            current.NotesForBuyer = updated.NotesForBuyer;
            current.ManufacturerPartNumber = updated.ManufacturerPartNumber;
            current.VendorPartNumber = updated.VendorPartNumber;
            current.RohsCategory = updated.RohsCategory;
            current.DateRohsCompliant = updated.DateRohsCompliant;
            current.RohsCompliant = updated.RohsCompliant;
            current.RohsComments = updated.RohsComments;
        }

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException("You are not authorised to update Part Supplier records");
            }

            var errors = this.ValidateFields(candidate);

            if (errors.Any())
            {
                var msg = errors
                        .Aggregate(
                            "The inputs for the following fields are empty/invalid: ", 
                            (current, error) => current + $"{error}, ");

                throw new PartSupplierException(msg);
            }

            if (!string.IsNullOrEmpty(candidate.OrderMethod?.Name))
            {
                candidate.OrderMethod = this.orderMethodRepository.FindById(candidate.OrderMethod.Name);
            }

            if (!string.IsNullOrEmpty(candidate.Currency?.Code))
            {
                candidate.Currency = this.currencyRepository.FindById(candidate.Currency.Code);
            }

            if (candidate.DeliveryAddress?.Id != null)
            {
                candidate.DeliveryAddress = this.addressRepository.FindById(candidate.DeliveryAddress.Id);
            }

            if (candidate.Tariff?.Id != null)
            {
                candidate.Tariff = this.tariffRepository.FindById(candidate.Tariff.Id);
            }

            if (candidate.PackagingGroup?.Id != null)
            {
                candidate.PackagingGroup = this.packagingGroupRepository.FindById(candidate.PackagingGroup.Id);
            }

            if (candidate.MadeInvalidBy?.Id != null)
            {
                candidate.MadeInvalidBy = this.employeeRepository.FindById(candidate.MadeInvalidBy.Id);
            }

            if (!string.IsNullOrEmpty(candidate.Manufacturer?.Code))
            {
                candidate.Manufacturer = this.manufacturerRepository.FindById(candidate.Manufacturer.Code);
            }

            return candidate;
        }

        private List<string> ValidateFields(PartSupplier candidate)
        {
            var errors = new List<string>();

            if (candidate.MinimumOrderQty == 0)
            {
                errors.Add("Minimum Order Qty");
            }

            if (candidate.CreatedBy == null)
            {
                errors.Add("Created By");
            }

            if (candidate.OrderIncrement == 0)
            {
                errors.Add("Order Increment");
            }

            if (candidate.LeadTimeWeeks == 0)
            {
                errors.Add("Lead Time Weeks");
            }

            if (string.IsNullOrEmpty(candidate.RohsCompliant))
            {
                errors.Add("Rohs Compliant");
            }

            if (string.IsNullOrEmpty(candidate.RohsCategory))
            {
                errors.Add("Rohs Category");
            }

            return errors;
        }
    }
}
