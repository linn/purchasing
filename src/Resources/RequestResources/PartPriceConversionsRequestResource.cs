namespace Linn.Purchasing.Resources.RequestResources
{
    public class PartPriceConversionsRequestResource
    {
        public string PartNumber { get; set; }

        public string NewCurrency { get; set; }

        public decimal NewPrice { get; set; }
    }
}
