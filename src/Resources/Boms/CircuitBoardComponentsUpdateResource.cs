namespace Linn.Purchasing.Resources.Boms
{
    using System.Collections.Generic;

    public class CircuitBoardComponentsUpdateResource : CircuitBoardBaseResource
    {
        public int ChangeRequestId { get; set; }
        
        public string ChangeRequestRevisionCode { get; set; }

        public int UserNumber { get; set; }

        public IEnumerable<BoardComponentUpdateResource> Components { get; set; }
    }
}
