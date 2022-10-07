namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    public interface IPartHistoryService
    {
        public void AddPartHistory(Part prevPart, Part part, string changeType, Employee changedBy, string remarks, string priceChangeReason);
    }
}
