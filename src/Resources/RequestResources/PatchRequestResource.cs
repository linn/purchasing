namespace Linn.Purchasing.Resources.RequestResources
{
    public class PatchRequestResource<TResource>
    {
        public TResource From { get; set; }

        public TResource To { get; set; }
    }
}
