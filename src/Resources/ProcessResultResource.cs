namespace Linn.Purchasing.Resources
{
    public class ProcessResultResource
    {
        public ProcessResultResource()
        {
        }

        public ProcessResultResource(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public string ProcessHref { get; set; }
    }
}
