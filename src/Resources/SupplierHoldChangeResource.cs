namespace Linn.Purchasing.Resources
{
    public class SupplierHoldChangeResource
    {
        public int SupplierId { get; set; }

        public string ReasonOnHold { get; set; }

        public string ReasonOffHold { get; set; }

        public int PutOnHoldBy { get; set; }

        public int? TakenOffHoldBy { get; set; }
    }
}
