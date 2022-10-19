namespace Linn.Purchasing.Domain.LinnApps.Finance.Models
{
    public class ImmediateLiability
    {
        public int OrderNumber { get; set; }

        public int OrderLine { get; set; }

        public decimal Quantity { get; set; }

        public decimal Liability { get; set; }
    }
}
