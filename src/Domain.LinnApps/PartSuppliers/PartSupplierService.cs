namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

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

        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<PartSupplier, PartSupplierKey> partSupplierRepository;

        private readonly IRepository<PartHistoryEntry, PartHistoryEntryKey> partHistory;

        private readonly IRepository<PriceChangeReason, string> changeReasonsRepository;

        private readonly IRepository<PreferredSupplierChange, PreferredSupplierChangeKey>
            preferredSupplierChangeRepository;

        public PartSupplierService(
            IAuthorisationService authService,
            IRepository<Currency, string> currencyRepository,
            IRepository<OrderMethod, string> orderMethodRepository,
            IRepository<Address, int> addressRepository,
            IRepository<Tariff, int> tariffRepository,
            IRepository<PackagingGroup, int> packagingGroupRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Manufacturer, string> manufacturerRepository,
            IQueryRepository<Part> partRepository,
            IRepository<Supplier, int> supplierRepository,
            IRepository<PartSupplier, PartSupplierKey> partSupplierRepository,
            IRepository<PartHistoryEntry, PartHistoryEntryKey> partHistory,
            IRepository<PriceChangeReason, string> changeReasonsRepository,
            IRepository<PreferredSupplierChange, PreferredSupplierChangeKey> preferredSupplierChangeRepository)
        {
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.orderMethodRepository = orderMethodRepository;
            this.addressRepository = addressRepository;
            this.tariffRepository = tariffRepository;
            this.packagingGroupRepository = packagingGroupRepository;
            this.employeeRepository = employeeRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.partRepository = partRepository;
            this.supplierRepository = supplierRepository;
            this.partSupplierRepository = partSupplierRepository;
            this.partHistory = partHistory;
            this.changeReasonsRepository = changeReasonsRepository;
            this.preferredSupplierChangeRepository = preferredSupplierChangeRepository;
        }

        public void UpdatePartSupplier(
            PartSupplier current, 
            PartSupplier updated, 
            IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to update Part Supplier records");
            }

            this.ValidateFields(updated);

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
                current.DeliveryAddress = updated.DeliveryAddress == null 
                                              ? null 
                                              : this.addressRepository.FindById(updated.DeliveryAddress.Id);
            }

            if (current.Tariff?.Id != updated.Tariff?.Id)
            {
                current.Tariff = updated.Tariff == null 
                                     ? null 
                                     : this.tariffRepository.FindById(updated.Tariff.Id);
            }

            if (current.PackagingGroup?.Id != updated.PackagingGroup?.Id)
            {
                current.PackagingGroup = updated.PackagingGroup == null 
                                             ? null 
                                             : this.packagingGroupRepository
                                                 .FindById(updated.PackagingGroup.Id);
            }

            if (current.MadeInvalidBy?.Id != updated.MadeInvalidBy?.Id)
            {
                current.MadeInvalidBy = updated.MadeInvalidBy == null
                                             ? null 
                                             : this.employeeRepository.FindById(updated.MadeInvalidBy.Id);
            }

            if (current.Manufacturer?.Code != updated.Manufacturer?.Code)
            {
                current.Manufacturer = updated.Manufacturer == null
                                            ? null 
                                            : this.manufacturerRepository.FindById(updated.Manufacturer.Code);
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
                throw new UnauthorisedActionException(
                    "You are not authorised to update Part Supplier records");
            }

            this.ValidateFields(candidate);

            candidate.CreatedBy = this.employeeRepository.FindById(candidate.CreatedBy.Id);
            var part = this.partRepository.FindBy(x => x.PartNumber == candidate.PartNumber);
            candidate.Part = part;
            
            if (string.IsNullOrEmpty(candidate.SupplierDesignation))
            {
                candidate.SupplierDesignation = part.Description;
            }

            candidate.Supplier = this.supplierRepository.FindById(candidate.SupplierId);

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
                candidate.PackagingGroup = this.packagingGroupRepository
                    .FindById(candidate.PackagingGroup.Id);
            }

            if (candidate.MadeInvalidBy?.Id != null)
            {
                candidate.MadeInvalidBy = this.employeeRepository
                    .FindById(candidate.MadeInvalidBy.Id);
            }

            if (!string.IsNullOrEmpty(candidate.Manufacturer?.Code))
            {
                candidate.Manufacturer = this.manufacturerRepository.FindById(candidate.Manufacturer.Code);
            }

            return candidate;
        }

        public PreferredSupplierChange CreatePreferredSupplierChange(PreferredSupplierChange candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to update Part Supplier records");
            }

            var part = this.partRepository.FindBy(x => x.PartNumber == candidate.PartNumber.ToUpper());

            var prevPart = new Part
                               {
                                   MaterialPrice = part.MaterialPrice, 
                                   PreferredSupplier = part.PreferredSupplier,
                                   Currency = part.Currency,
                                   LabourPrice = part.LabourPrice,
                                   BaseUnitPrice = part.BaseUnitPrice,
                                   BomType = part.BomType,
                                   CurrencyUnitPrice = part.CurrencyUnitPrice
                               };
            
            if (part.BomType.Equals("P") || part.BomType.Equals("S"))
            {
                throw new PartSupplierException("You cannot set a preferred supplier for phantoms");
            }

            var oldPartSupplier = this.partSupplierRepository.FindById(
                new PartSupplierKey { PartNumber = part.PartNumber, SupplierId = candidate.OldSupplier.SupplierId });
            oldPartSupplier.SupplierRanking = 2;

            var newPartSupplier = this.partSupplierRepository.FindById(
                new PartSupplierKey { PartNumber = part.PartNumber, SupplierId = candidate.NewSupplier.SupplierId });
            newPartSupplier.SupplierRanking = 1;

            candidate.OldSupplier = prevPart.PreferredSupplier;
            candidate.OldPrice = prevPart.CurrencyUnitPrice;
            candidate.BaseOldPrice = prevPart.BaseUnitPrice;
            candidate.OldCurrency = prevPart.Currency;

            // todo - case where new part and no prices exist yet
            
            // otherwise the prices don't change
            candidate.NewPrice = prevPart.CurrencyUnitPrice;
            candidate.BaseNewPrice = prevPart.BaseUnitPrice;
            candidate.NewCurrency = prevPart.Currency;

            candidate.NewSupplier = this.supplierRepository.FindById(candidate.NewSupplier.SupplierId);
            candidate.ChangedBy = this.employeeRepository.FindById(candidate.ChangedBy.Id);
            candidate.ChangeReason = this.changeReasonsRepository.FindById(candidate.ChangeReason.ReasonCode);
            candidate.DateChanged = DateTime.Now;

            var entriesForThisPart =
                this.preferredSupplierChangeRepository.FilterBy(x => x.PartNumber == candidate.PartNumber);

            candidate.Seq = entriesForThisPart.Any() ? entriesForThisPart.Max(x => x.Seq) + 1 : 1;

            // update Part
            if (!(part.BomType.Equals("A") && newPartSupplier.SupplierId == 4415))
            {
                part.LabourPrice = 0;
            }

            part.PreferredSupplier = newPartSupplier.Supplier;
            
            if (prevPart.BaseUnitPrice.GetValueOrDefault() == 0)
            {
                // todo - find out if standard price should still change here
                part.PreferredSupplier = newPartSupplier.Supplier;
                part.MaterialPrice = candidate.BaseNewPrice;
                part.Currency = candidate.NewCurrency;
                part.CurrencyUnitPrice = candidate.NewPrice;
            }

            var history = this.partHistory.FilterBy(x => x.PartNumber == candidate.PartNumber);
            
            var maxSeqForPart = history.Any() ? history.Max(x => x.Seq) : 0;

            // update Part History
            this.partHistory.Add(new PartHistoryEntry
                                     {
                                        PartNumber = candidate.PartNumber,
                                        Seq = maxSeqForPart + 1,
                                        OldMaterialPrice = prevPart.MaterialPrice,
                                        OldLabourPrice = prevPart.LabourPrice,
                                        NewMaterialPrice = part.MaterialPrice,
                                        NewLabourPrice = part.LabourPrice,
                                        OldPreferredSupplierId = prevPart.PreferredSupplier.SupplierId,
                                        NewPreferredSupplierId = part.PreferredSupplier.SupplierId,
                                        OldBomType = prevPart.BomType,
                                        NewBomType = part.BomType,
                                        ChangedBy = candidate.ChangedBy.Id,
                                        ChangeType = "PREFSUP",
                                        Remarks = candidate.Remarks,
                                        PriceChangeReason = candidate.ChangeReason.ReasonCode,
                                        OldCurrency = prevPart.Currency.Code,
                                        NewCurrency = part.Currency.Code,
                                        OldCurrencyUnitPrice = prevPart.CurrencyUnitPrice,
                                        NewCurrencyUnitPrice = part.CurrencyUnitPrice,
                                        OldBaseUnitPrice = prevPart.BaseUnitPrice,
                                        NewBaseUnitPrice = part.BaseUnitPrice
                                     });

            return candidate;
        }

        private void ValidateFields(PartSupplier candidate)
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
                candidate.RohsCompliant = "N";
            }

            if (string.IsNullOrEmpty(candidate.RohsCategory))
            {
                errors.Add("Rohs Category");
            }

            if (candidate.OrderMethod == null)
            {
                errors.Add("Order Method");
            }

            if (candidate.CurrencyUnitPrice.GetValueOrDefault() == 0)
            {
                errors.Add("Currency Unit Price");
            }

            if (candidate.MinimumDeliveryQty.GetValueOrDefault() == 0)
            {
                errors.Add("Minimum Delivery Quantity");
            }

            if (!candidate.DamagesPercent.HasValue)
            {
                errors.Add("Damages Percent");
            }

            if (candidate.Currency == null)
            {
                errors.Add("Currency");
            }

            if (errors.Any())
            {
                var msg = errors
                    .Aggregate(
                        "The inputs for the following fields are empty/invalid: ",
                        (current, error) => current + $"{error}, ");

                throw new PartSupplierException(msg);
            }
        }
    }
}
