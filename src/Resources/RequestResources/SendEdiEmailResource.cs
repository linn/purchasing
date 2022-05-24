namespace Linn.Purchasing.Resources.RequestResources
{
    public class SendEdiEmailResource
    {
        public int supplierId { get; set; }

        public string altEmail { get; set; }

        public string additionalEmail { get; set; }

        public string additionalText { get; set; }

        public bool test { get; set; }
    }
}

