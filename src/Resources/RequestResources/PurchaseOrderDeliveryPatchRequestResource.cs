namespace Linn.Purchasing.Resources.RequestResources
{
    public class PurchaseOrderDeliveryPatchRequestResource
    {
        public string AdvisedDate { get; set; }

        public string Reason { get; set; }

        public string ConfirmationComment { get; set; }

        public string AvailableAtSupplier { get; set; }
    }
}
