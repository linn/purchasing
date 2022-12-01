namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    public class BomDetailComponent
    {
        public int DetailId { get; set; }

        public string Component { get; set; }

        public string CircuitRef { get; set; }

        public string Bom { get; set; }

        public BomDetail Detail { get; set; }
    }
}
