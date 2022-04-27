namespace Linn.Purchasing.Domain.LinnApps
{
    public class NominalAccount
    {
        public int AccountId { get; set; }

        public Nominal Nominal { get; set; }

        public Department Department { get; set; }
    }
}
