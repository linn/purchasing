namespace Linn.Purchasing.Domain.LinnApps
{
    public class ProcessResult
    {
        public ProcessResult()
        {
        }

        public ProcessResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; set; }

        public string Message { get; set; }

        public string ProcessHref { get; set; }
    }
}
