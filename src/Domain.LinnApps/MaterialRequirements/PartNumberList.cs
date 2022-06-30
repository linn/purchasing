namespace Linn.Purchasing.Domain.LinnApps.MaterialRequirements
{
    using System;
    using System.Collections.Generic;

    public class PartNumberList
    {
        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }
        
        public string TypeOfList { get; set; }

        public string Temporary { get; set; }

        public IEnumerable<PartNumberListElement> Elements { get; set; }
    }
}
