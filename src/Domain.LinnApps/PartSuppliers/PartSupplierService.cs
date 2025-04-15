namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    public class PartSupplierService : IPartSupplierService
    {
        private readonly IAuthorisationService authService;

        private readonly IRepository<Currency, string> currencyRepository;

        private readonly IRepository<OrderMethod, string> orderMethodRepository;

        private readonly IRepository<FullAddress, int> addressRepository;

        private readonly IRepository<Employee, int> employeeRepository;

        private readonly IRepository<Manufacturer, string> manufacturerRepository;

        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<Supplier, int> supplierRepository;

        private readonly IRepository<PartSupplier, PartSupplierKey> partSupplierRepository;

        private readonly IPartHistoryService partHistoryService;

        private readonly IRepository<PriceChangeReason, string> changeReasonsRepository;

        private readonly IRepository<PreferredSupplierChange, PreferredSupplierChangeKey>
            preferredSupplierChangeRepository;

        private readonly IQueryRepository<StockLocator> stockLocatorRepository;

        public PartSupplierService(
            IAuthorisationService authService,
            IRepository<Currency, string> currencyRepository,
            IRepository<OrderMethod, string> orderMethodRepository,
            IRepository<FullAddress, int> addressRepository,
            IRepository<Employee, int> employeeRepository,
            IRepository<Manufacturer, string> manufacturerRepository,
            IQueryRepository<Part> partRepository,
            IRepository<Supplier, int> supplierRepository,
            IRepository<PartSupplier, PartSupplierKey> partSupplierRepository,
            IPartHistoryService partHistoryService,
            IRepository<PriceChangeReason, string> changeReasonsRepository,
            IRepository<PreferredSupplierChange, PreferredSupplierChangeKey> preferredSupplierChangeRepository,
            IQueryRepository<StockLocator> stockLocatorRepository)
        {
            this.authService = authService;
            this.currencyRepository = currencyRepository;
            this.orderMethodRepository = orderMethodRepository;
            this.addressRepository = addressRepository;
            this.employeeRepository = employeeRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.partRepository = partRepository;
            this.supplierRepository = supplierRepository;
            this.partSupplierRepository = partSupplierRepository;
            this.partHistoryService = partHistoryService;
            this.changeReasonsRepository = changeReasonsRepository;
            this.preferredSupplierChangeRepository = preferredSupplierChangeRepository;
            this.stockLocatorRepository = stockLocatorRepository;
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

            updated.CreatedBy = current.CreatedBy;
            ValidateFields(updated);

            if (current.OrderMethod.Name != updated.OrderMethod.Name)
            {
                current.OrderMethod = this.orderMethodRepository.FindById(updated.OrderMethod.Name);
            }

            if (current.Currency.Code != updated.Currency.Code)
            {
                current.Currency = this.currencyRepository.FindById(updated.Currency.Code);
            }

            if (current.DeliveryFullAddress?.Id != updated.DeliveryFullAddress?.Id)
            {
                current.DeliveryFullAddress = updated.DeliveryFullAddress == null 
                                              ? null 
                                              : this.addressRepository.FindById(updated.DeliveryFullAddress.Id);
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
            current.DamagesPercent = updated.DamagesPercent;
            current.DeliveryInstructions = updated.DeliveryInstructions;
            current.NotesForBuyer = updated.NotesForBuyer;
            current.ManufacturerPartNumber = updated.ManufacturerPartNumber;
            current.VendorPartNumber = updated.VendorPartNumber;
            current.UnitOfMeasure = updated.UnitOfMeasure;

            // ticket 20413 in limited circumstances the part price can change
            var part = this.partRepository.FindBy(x => x.PartNumber == current.PartNumber.ToUpper());
            if (part == null)
            {
                throw new PartSupplierException("Part Not Found");
            }

            if (part.PreferredSupplier?.SupplierId == updated.SupplierId && part.BomType == "C")
            {
                // if part is preferred supplier and stock locator is null
                var prevPart = part.ClonePricingFields();
                var stockLocators = this.stockLocatorRepository.FilterBy(s => s.PartNumber == prevPart.PartNumber && s.Qty > 0).ToList();
                var stockQty = stockLocators.Any() ? stockLocators.Sum(l => l.Qty) : 0;

                // if this is the first time a preferred supplier is chosen for this part
                if (prevPart.ShouldChangeStandardPrice(stockQty))
                {
                    part.MaterialPrice = updated.BaseOurUnitPrice;
                    var currency = this.currencyRepository.FindById(updated.Currency.Code);
                    part.Currency = currency; // if go straight to updated.currency EF complains
                    part.CurrencyUnitPrice = updated.CurrencyUnitPrice;
                    part.BaseUnitPrice = updated.BaseOurUnitPrice;
                    part.LabourPrice = 0m;
                    this.partHistoryService.AddPartHistory(prevPart, part, "PARTSUP PRICE", 100, "zero stock price chg", null);
                }
            }
        }

        public PartSupplier CreatePartSupplier(PartSupplier candidate, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierCreate, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to update Part Supplier records");
            }

            ValidateFields(candidate);

            var existingPartSupplier = this.partSupplierRepository.FindById(
                new PartSupplierKey { PartNumber = candidate.PartNumber, SupplierId = candidate.SupplierId });
            if (existingPartSupplier != null)
            {
                throw new PartSupplierException("This part and supplier ID combination has already been created");
            }

            candidate.CreatedBy = this.employeeRepository.FindById(candidate.CreatedBy.Id);
            var part = this.partRepository.FindBy(x => x.PartNumber == candidate.PartNumber);
            if (part == null)
            {
                throw new PartSupplierException("Part Not Found");
            }

            candidate.Part = part;
            
            if (string.IsNullOrEmpty(candidate.SupplierDesignation))
            {
                candidate.SupplierDesignation = part.Description;
            }

            candidate.Supplier = this.supplierRepository.FindById(candidate.SupplierId);
            if (candidate.Supplier == null)
            {
                throw new PartSupplierException("Supplier Not Found");
            }

            if (!string.IsNullOrEmpty(candidate.OrderMethod?.Name))
            {
                candidate.OrderMethod = this.orderMethodRepository.FindById(candidate.OrderMethod.Name);
            }

            if (!string.IsNullOrEmpty(candidate.Currency?.Code))
            {
                candidate.Currency = this.currencyRepository.FindById(candidate.Currency.Code);
            }

            if (candidate.DeliveryFullAddress?.Id != null)
            {
                candidate.DeliveryFullAddress = this.addressRepository.FindById(candidate.DeliveryFullAddress.Id);
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

            candidate.DateCreated = DateTime.Now;
            candidate.OverbookingAllowed = "Y";
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

            var prevPart = part.ClonePricingFields();
            
            if (part.BomType.Equals("P") || part.BomType.Equals("S"))
            {
                throw new PartSupplierException("You cannot set a preferred supplier for phantoms");
            }

            if (part.BomType == "C" && candidate.NewSupplier.SupplierId == 4415)
            {
                throw new PartSupplierException("Linn Cannot Supply Components");
            }

            if (candidate.OldSupplier != null)
            {
                if (candidate.NewSupplier.SupplierId == part.PreferredSupplier?.SupplierId)
                {
                    throw new PartSupplierException(
                        "Selected  supplier is already the preferred supplier for this part.");
                }

                var oldPartSupplier = this.partSupplierRepository.FindById(
                    new PartSupplierKey { PartNumber = part.PartNumber, SupplierId = candidate.OldSupplier.SupplierId });

                if (oldPartSupplier != null)
                {
                    oldPartSupplier.SupplierRanking = 2;
                }
            }
            
            var newPartSupplier = this.partSupplierRepository.FindById(
                new PartSupplierKey { PartNumber = part.PartNumber, SupplierId = candidate.NewSupplier.SupplierId });

            if (newPartSupplier.Supplier.Planner == null
                || newPartSupplier.Supplier.VendorManager == null)
            {
                throw new PartSupplierException(
                    "Selected supplier is missing planner or vendor manager");
            }

            newPartSupplier.SupplierRanking = 1;


            candidate.OldSupplier = prevPart.PreferredSupplier;
            candidate.OldPrice = prevPart.CurrencyUnitPrice;
            candidate.BaseOldPrice = prevPart.BaseUnitPrice;
            candidate.OldCurrency = prevPart.Currency;

            candidate.NewSupplier = this.supplierRepository.FindById(candidate.NewSupplier.SupplierId);
            candidate.ChangedBy = this.employeeRepository.FindById(candidate.ChangedBy.Id);
            candidate.ChangeReason = this.changeReasonsRepository.FindById(candidate.ChangeReason.ReasonCode);
            candidate.DateChanged = DateTime.Now;

            var entriesForThisPart =
                this.preferredSupplierChangeRepository.FilterBy(x => x.PartNumber == candidate.PartNumber);

            candidate.Seq = entriesForThisPart.Any() ? entriesForThisPart.Max(x => x.Seq) + 1 : 1;

            decimal? labourPrice;

            // update Part
            if (part.BomType.Equals("A") && newPartSupplier.SupplierId != 4415)
            {
                labourPrice = part.LabourPrice ?? 0m;
            }
            else
            {
                labourPrice = 0m;
            }

            part.PreferredSupplier = newPartSupplier.Supplier;

            var stockLocators = this.stockLocatorRepository.FilterBy(s => s.PartNumber == prevPart.PartNumber && s.Qty > 0).ToList();

            var stockQty = stockLocators.Any() ? stockLocators.Sum(l => l.Qty) : 0;

            // if this is the first time a preferred supplier is chosen for this part
            if (prevPart.ShouldChangeStandardPrice(stockQty))
            {
                var newCurrency = this.currencyRepository.FindById(candidate.NewCurrency.Code);

                // set prices
                part.MaterialPrice = candidate.BaseNewPrice;
                part.Currency = newCurrency;
                part.CurrencyUnitPrice = candidate.NewPrice;
                part.BaseUnitPrice = candidate.BaseNewPrice;
                part.LabourPrice = labourPrice;
                candidate.NewCurrency = newCurrency;
            }
            else
            {
                // otherwise the prices don't change
                candidate.NewPrice = prevPart.CurrencyUnitPrice;
                candidate.BaseNewPrice = prevPart.BaseUnitPrice;
                candidate.NewCurrency = prevPart.Currency;
            }

            // update Part History
            this.partHistoryService.AddPartHistory(prevPart, part, "PREFSUP", candidate.ChangedBy.Id, candidate.Remarks, candidate.ChangeReason?.ReasonCode);

            return candidate;
        }

        public BatchUpdateProcessResult BulkUpdateLeadTimes(
            int supplierId,
            IEnumerable<LeadTimeUpdateModel> changes,
            IEnumerable<string> privileges,
            int? supplierGroupId = null)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.PartSupplierUpdate, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to update Part Supplier records");
            }

            var successCount = 0;
            var errors = new List<string>();
            var leadTimeUpdateModels = changes.ToList();

            foreach (var change in leadTimeUpdateModels)
            {
                IQueryable<PartSupplier> records;

                if (supplierGroupId.GetValueOrDefault() != 0)
                {
                    records = this.partSupplierRepository.FilterBy(
                        x => x.PartNumber == change.PartNumber.ToUpper().Trim()
                             && x.Supplier.Group != null 
                             && x.Supplier.Group.Id == supplierGroupId);
                }
                else
                {
                    records = this.partSupplierRepository.FilterBy(
                        x => x.PartNumber == change.PartNumber.ToUpper().Trim()
                             && x.SupplierId == supplierId);
                }

                if (int.TryParse(change.LeadTimeWeeks, out var newLeadTime) && records.Any())
                {
                    foreach (var record in records)
                    {
                        record.LeadTimeWeeks = newLeadTime;
                    }

                    successCount++;
                }
                else
                {
                    errors.Add(change.PartNumber);
                }
            }

            if (!errors.Any())
            {
                return new BatchUpdateProcessResult
                           {
                               Success = true, 
                               Message = $"{successCount} records updated successfully",
                           };
            }

            var errorMessage = 
                    "Updates for the following parts could not be processed: ";

            return new BatchUpdateProcessResult 
                       {
                           Success = false,
                           Message = $"{successCount} out of {leadTimeUpdateModels.Count} records updated successfully. {errorMessage}",
                           Errors = errors.Select(e => new Error(e, "No record found."))
                       };
        }

        private static void ValidateFields(PartSupplier candidate)
        {
            var errors = new List<string>();

            if (candidate.SupplierId == 0)
            {
                errors.Add("Supplier");
            }

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
