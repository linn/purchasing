namespace Linn.Purchasing.Resources
{
    public class NominalAccountResource
    {
        public int AccountId { get; set; }

        public NominalResource Nominal { get; set; }

        public DepartmentResource Department { get; set; }
    }
}
