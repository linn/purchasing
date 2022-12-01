namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System.Collections.Generic;

    using Linn.Common.Authorisation;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.Keys;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;

    public class PartService : IPartService
    {
        private readonly IQueryRepository<Part> partRepository;

        private readonly IRepository<PartSupplier, PartSupplierKey> partSupplierRepository;

        private readonly IPartHistoryService partHistoryService;

        private readonly IAuthorisationService authService;

        public PartService(
            IQueryRepository<Part> partRepository,
            IRepository<PartSupplier, PartSupplierKey> partSupplierRepository,
            IPartHistoryService partHistoryService,
            IAuthorisationService authService)
        {
            this.partRepository = partRepository;
            this.partSupplierRepository = partSupplierRepository;
            this.partHistoryService = partHistoryService;
            this.authService = authService;
        }

        public Part ChangeBomType(BomTypeChange bomTypeChange, IEnumerable<string> privileges)
        {
            if (!this.authService.HasPermissionFor(AuthorisedAction.ChangeBomType, privileges))
            {
                throw new UnauthorisedActionException(
                    "You are not authorised to change bom type");
            }

            var part = this.partRepository.FindBy(p => p.PartNumber == bomTypeChange.PartNumber);
            if (part == null)
            {
                throw new ItemNotFoundException("Part not found");
            }

            if (part.BomType != bomTypeChange.OldBomType)
            {
                throw new InvalidBomTypeChangeException(
                    $"Inconsistent old bom type should be {part.BomType} was {bomTypeChange.OldBomType}");
            }

            if (!part.ValidBomTypeChange(bomTypeChange.NewBomType))
            {
                throw new InvalidBomTypeChangeException(
                    $"Invalid bom type change from {part.BomType} to {bomTypeChange.NewBomType}");
            }

            if (bomTypeChange.OldSupplierId == null)
            {
                if (part.PreferredSupplier != null)
                {
                    throw new InvalidBomTypeChangeException(
                        $"Inconsistent old supplier id should be {part.PreferredSupplier.SupplierId} was blank");
                }
            }
            else if (part.PreferredSupplier.SupplierId != bomTypeChange.OldSupplierId)
            {
                throw new InvalidBomTypeChangeException(
                    $"Inconsistent old supplier id should be {part.PreferredSupplier.SupplierId} was {bomTypeChange.OldSupplierId}");
            }

            var newPartSupplier = bomTypeChange.NewSupplierId.HasValue
                                      ? this.partSupplierRepository.FindById(
                                          new PartSupplierKey
                                              {
                                                  PartNumber = part.PartNumber,
                                                  SupplierId = (int) bomTypeChange.NewSupplierId
                                              })
                                      : null;
            if ((newPartSupplier == null) && bomTypeChange.NewSupplierId.HasValue)
            {
                throw new InvalidBomTypeChangeException(
                    $"New supplier id {bomTypeChange.NewSupplierId} is not a supplier of part {part.PartNumber}");
            }

            var prevPart = part.ClonePricingFields();

            part.BomType = bomTypeChange.NewBomType;
            part.PreferredSupplier = newPartSupplier?.Supplier;
            // TODO work out if under new standards we want to autocost a new price with possible material variance req

            this.partHistoryService.AddPartHistory(
                prevPart,
                part,
                "BOM TYPE",
                bomTypeChange.ChangedBy ?? 100,
                null,
                null);

            return part;
        }
    }
}
