namespace Linn.Purchasing.Domain.LinnApps
{
    using System.Collections.Generic;

    public class Nominal
    {
        public string NominalCode { get; set; }

        public string Description { get; set; }

        public IEnumerable<NominalAccount> NominalAccounts { get; set; }
    }
}
