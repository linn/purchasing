namespace Linn.Purchasing.Domain.LinnApps.Boms.Models
{
    public class PcasChangeComponent
    {
        public string BoardCode { get; set; }

        public string RevisionCode { get; set; }

        public int DocumentNumber { get; set; }

        public string Cref { get; set; }

        public string OldPartNumber { get; set; }

        public string NewPartNumber { get; set; }

        public string OldAssemblyTechnology { get; set; }

        public string NewAssemblyTechnology { get; set; }

        public decimal? OldQty { get; set; }

        public decimal? NewQty { get; set; }
    }
}
