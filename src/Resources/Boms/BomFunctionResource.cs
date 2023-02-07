namespace Linn.Purchasing.Resources.Boms
{
    public class BomFunctionResource
    {
        public string SrcPartNumber { get; set; }

        public string DestPartNumber { get; set; }

        public int CrfNumber { get; set; }

        public string SubAssembly { get; set; }

        public string AddOrOverwrite { get; set; }

        public string RootName { get; set; }
    }
}
