namespace Linn.Purchasing.Resources.RequestResources
{
    public class BomDifferenceReportRequestResource
    {
        public string BoardCode1 { get; set; }

        public string RevisionCode1 { get; set; }
        
        public string BoardCode2 { get; set; }
        
        public string RevisionCode2 { get; set; }

        public string PartNumber1 { get; set; }

        public string PartNumber2 { get; set; }

        public bool LiveOnly { get; set; }
    }
}
