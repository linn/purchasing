namespace Linn.Purchasing.Resources.MaterialRequirements
{
    public class MrDetailResource
    {
        public string Title { get; set; }

        public int Segment { get; set; }

        public int DisplaySequence { get; set; }

        public string Value { get; set; }

        public string Tag { get; set; }

        public MrDetailItemResource Item { get; set; }
    }
}
