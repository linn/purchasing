namespace Linn.Purchasing.Domain.LinnApps.Edi
{
    public class EdiOrder
    {
        public int Id { get; set; }

        public int OrderNumber { get; set; }

        public int SupplierId { get; set; }

        public int? SequenceNumber { get; set; }
    }
}
