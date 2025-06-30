namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class ProcessResultResourceWithLinks : HypermediaResource
    {
        public ProcessResultResourceWithLinks()
        {
        }

        public ProcessResultResourceWithLinks(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
