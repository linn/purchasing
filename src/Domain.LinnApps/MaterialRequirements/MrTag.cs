namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    public class MrTag
    {
        public MrTag()
        {
        }

        public MrTag(string title, string tag)
        {
            this.Title = title;
            this.Tag = tag;
        }

        public string Title { get; set; }

        public string Tag { get; set; }
    }
}
