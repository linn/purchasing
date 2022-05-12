namespace Linn.Purchasing.Resources.RequestResources
{
    public class PatchRequestResource<T>
    {
        public T From { get; set; }

        public T To { get; set; }
    }
}
