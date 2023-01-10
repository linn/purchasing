namespace Linn.Purchasing.Resources.Boms
{
    public class CopyBomResource
    {
        public string SrcPartNumber { get; set; }

        public int DestBomId { get; set; }

        public string DestPartNumber { get; set; }

        public int CrfNumber { get; set; }
    }
}
