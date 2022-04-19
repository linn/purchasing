namespace Linn.Purchasing.Domain.LinnApps.Keys
{
    public class MrOrderKey
    {
        public int OrderNumber { get; set; }

        //public int OrderLine { get; set; 
        //todo add this as primary key to database

        public string JobRef { get; set; }
    }
}
