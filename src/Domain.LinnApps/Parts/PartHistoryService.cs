namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Purchasing.Domain.LinnApps.Keys;

    public class PartHistoryService : IPartHistoryService
    {
        private readonly IRepository<PartHistoryEntry, PartHistoryEntryKey> partHistory;

        public PartHistoryService(IRepository<PartHistoryEntry, PartHistoryEntryKey> partHistory)
        {
            this.partHistory = partHistory;
        }

        public void AddPartHistory(Part prevPart, Part part, string changeType, int changedBy, string remarks, string priceChangeReason)
        {
            // update Part History
            var history = this.partHistory.FilterBy(x => x.PartNumber == part.PartNumber);
            var maxSeqForPart = history.Any() ? history.Max(x => x.Seq) : 0;
            var entry = new PartHistoryEntry
                            {
                                PartNumber = part.PartNumber,
                                Seq = maxSeqForPart + 1,
                                OldMaterialPrice = prevPart.MaterialPrice,
                                OldLabourPrice = prevPart.LabourPrice,
                                NewMaterialPrice = part.MaterialPrice,
                                NewLabourPrice = part.LabourPrice,
                                OldPreferredSupplierId = prevPart.PreferredSupplier?.SupplierId,
                                NewPreferredSupplierId = part.PreferredSupplier?.SupplierId,
                                OldBomType = prevPart.BomType,
                                NewBomType = part.BomType,
                                ChangedBy = changedBy,
                                ChangeType = changeType,
                                Remarks = remarks,
                                PriceChangeReason = priceChangeReason,
                                OldCurrency = prevPart.Currency?.Code,
                                NewCurrency = part.Currency?.Code,
                                OldCurrencyUnitPrice = prevPart.CurrencyUnitPrice,
                                NewCurrencyUnitPrice = part.CurrencyUnitPrice,
                                OldBaseUnitPrice = prevPart.BaseUnitPrice,
                                NewBaseUnitPrice = part.BaseUnitPrice,
                                DateChanged = DateTime.UtcNow
                            };
            this.partHistory.Add(entry);
        }
    }
}
