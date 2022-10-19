namespace Linn.Purchasing.Domain.LinnApps.Parts
{
    using System.Collections.Generic;

    public interface IPartService
    {
        public Part ChangeBomType(BomTypeChange bomTypeChange, IEnumerable<string> privileges);
    }
}
