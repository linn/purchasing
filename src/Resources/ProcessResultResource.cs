﻿namespace Linn.Purchasing.Resources
{
    using Linn.Common.Resources;

    public class ProcessResultResource : HypermediaResource
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
    }
}
