namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    public class Thing
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int? CodeId { get; set; }

        public ThingCode Code { get; set; }

        public IList<ThingDetail> Details { get; set; }

        public string RecipientName { get; set; }

        public string RecipientAddress { get; set; }
    }
}
