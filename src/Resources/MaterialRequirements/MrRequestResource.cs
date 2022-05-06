namespace Linn.Purchasing.Resources.MaterialRequirements
{
    using System.Collections.Generic;

    public class MrRequestResource
    {
        public string JobRef { get; set; }

        public IEnumerable<string> PartNumbers { get; set; }
    }
}
