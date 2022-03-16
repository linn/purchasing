namespace Linn.Purchasing.Domain.LinnApps.PartSuppliers
{
    public class LeadTimeUpdateModel
    {
        public LeadTimeUpdateModel(string partNumber, string leadTimeWeeks)
        {
            this.PartNumber = partNumber;
            this.LeadTimeWeeks = leadTimeWeeks;
        }

        public string PartNumber { get; set; }

        public string LeadTimeWeeks { get; set; }
    }
}
