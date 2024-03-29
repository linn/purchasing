﻿namespace Linn.Purchasing.Resources
{
    using System.Collections.Generic;

    public class BatchUpdateProcessResultResource : ProcessResultResource
    {
        public IEnumerable<ErrorResource> Errors { get; set; }

        public IEnumerable<string> Notes { get; set; }
    }
}
