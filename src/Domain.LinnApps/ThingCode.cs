namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    public class ThingCode
    {
        public int Code { get; set; }
        
        public string CodeName { get; set; }

        public IEnumerable<Thing> Things { get; set; }
    }
}
