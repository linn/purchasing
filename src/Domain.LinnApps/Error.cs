namespace Linn.Purchasing.Domain.LinnApps
{
    public class Error
    {
        public Error(string descriptor, string message)
        {
            this.Descriptor = descriptor;
            this.Message = message;
        }

        public string Descriptor { get; set; }

        public string Message { get; set; }
    }
}

