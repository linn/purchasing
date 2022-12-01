namespace Linn.Purchasing.Facade.Services
{
    using System;
    using System.Collections.Generic;

    using Linn.Common.Facade;
    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Exceptions;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.Parts.Exceptions;
    using Linn.Purchasing.Resources;

    public class PartFacadeService : IPartFacadeService
    {
        private readonly IQueryRepository<Part> partRepository;

        private readonly IAutocostPack autocostPack;

        private readonly ICurrencyPack currencyPack;

        private readonly IPartService partService;

        private readonly IBuilder<BomTypeChange> bomTypeChangeBuilder;

        private readonly ITransactionManager transactionManager;

        public PartFacadeService(
            IQueryRepository<Part> partRepository, 
            IAutocostPack autocostPack, 
            ICurrencyPack currencyPack,
            IPartService partService,
            IBuilder<BomTypeChange> bomTypeChangeBuilder,
            ITransactionManager transactionManager)
        {
            this.partRepository = partRepository;
            this.autocostPack = autocostPack;
            this.currencyPack = currencyPack;
            this.partService = partService;
            this.bomTypeChangeBuilder = bomTypeChangeBuilder;
            this.transactionManager = transactionManager;
        }

        public string GetPartNumberFromId(int id)
        {
            return this.partRepository.FindBy(x => x.Id == id)?.PartNumber;
        }

        public IResult<PartPriceConversionsResource> GetPrices(
            string partNumber,
            string newCurrency,
            decimal newPrice,
            string ledger,
            string round)
        {
            try
            {
                return new SuccessResult<PartPriceConversionsResource>(
                    new PartPriceConversionsResource
                        {
                            NewPrice = string.IsNullOrEmpty(partNumber) ? null : this.autocostPack.CalculateNewMaterialPrice(partNumber, newCurrency, newPrice),
                            BaseNewPrice = this.currencyPack.CalculateBaseValueFromCurrencyValue(newCurrency, newPrice, ledger, round)
                        });
            }
            catch (Exception e)
            {
                return new BadRequestResult<PartPriceConversionsResource>(e.Message);
            }
        }

        public IResult<BomTypeChangeResource> GetBomType(int partNumberId, IEnumerable<string> privileges = null)
        {
            var part = this.partRepository.FindBy(p => p.Id == partNumberId);
            if (part == null)
            {
                return new NotFoundResult<BomTypeChangeResource>("Part not found");
            }

            var model = new BomTypeChange { PartNumber = part.PartNumber, Part = part, OldBomType = part.BomType, OldSupplierId = part.PreferredSupplier?.SupplierId };

            var resource = (BomTypeChangeResource)this.bomTypeChangeBuilder.Build(model, privileges);

            return new SuccessResult<BomTypeChangeResource>(resource);
        }
        
        public IResult<BomTypeChangeResource> ChangeBomType(BomTypeChangeResource request, IEnumerable<string> privileges = null)
        {
            if (request == null)
            {
                return new BadRequestResult<BomTypeChangeResource>("No request body");
            }

            var bomTypeChange = new BomTypeChange
                                    {
                                        PartNumber = request.PartNumber,
                                        OldBomType = request.OldBomType,
                                        NewBomType = request.NewBomType,
                                        OldSupplierId = request.OldSupplierId,
                                        NewSupplierId = request.NewSupplierId,
                                        ChangedBy = request.ChangedBy
                                    };
            try
            {
                var part = this.partService.ChangeBomType(bomTypeChange, privileges);
                bomTypeChange.Part = part;
                this.transactionManager.Commit();
            }
            catch (ItemNotFoundException e)
            {
                return new NotFoundResult<BomTypeChangeResource>(e.Message);
            }
            catch (InvalidBomTypeChangeException e)
            {
                return new BadRequestResult<BomTypeChangeResource>(e.Message);
            }

            var resource = (BomTypeChangeResource) this.bomTypeChangeBuilder.Build(bomTypeChange, null);
            return new SuccessResult<BomTypeChangeResource>(resource);
        }
    }
}
