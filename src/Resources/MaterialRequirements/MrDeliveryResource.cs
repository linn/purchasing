namespace Linn.Purchasing.Resources.MaterialRequirements
{
    public class MrDeliveryResource
    {
        public string JobRef { get; set; }

        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public int DeliverySequence { get; set; }

        public decimal DeliveryQuantity { get; set; }

        public decimal? QuantityReceived { get; set; }

        public string RequestedDeliveryDate { get; set; }

        public string AdvisedDeliveryDate { get; set; }

        public string Reference { get; set; }
    }
}
