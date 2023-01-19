namespace Linn.Purchasing.Domain.LinnApps.Boms
{
    using System.Collections.Generic;
    using System.Linq;

    public class BomChangeDetail
    {
        public BomChangeDetail(BomDetail addDetail, BomDetail deleteDetail, IEnumerable<BomDetail> deleteDetails)
        {
            if (addDetail != null)
            {
                this.DetailId = addDetail.DetailId;
                this.AddPartNumber = addDetail.PartNumber;
                this.AddQty = addDetail.Qty;
                this.AddGenerateRequirement = addDetail.GenerateRequirement;
            }

            if (addDetail?.AddReplaceSeq != null)
            {
                if (deleteDetails != null)
                {
                    var replacedDetail = deleteDetails.SingleOrDefault(d => d.DeleteReplaceSeq == addDetail.AddReplaceSeq);
                    if (replacedDetail != null)
                    {
                        this.DeletePartNumber = replacedDetail.PartNumber;
                        this.DeleteQty = replacedDetail.Qty;
                        this.DeleteGenerateRequirement = replacedDetail.GenerateRequirement;
                    }
                }
            }

            if ((deleteDetail != null) && (deleteDetail.DeleteReplaceSeq == null))
            {
                this.DetailId = deleteDetail.DetailId;
                this.DeletePartNumber = deleteDetail.PartNumber;
                this.DeleteQty = deleteDetail.Qty;
                this.DeleteGenerateRequirement = deleteDetail.GenerateRequirement;
            }
        }

        public int DetailId { get; set; }

        public string DeletePartNumber { get; set; }

        public decimal? DeleteQty { get; set; }

        public string DeleteGenerateRequirement { get; set; }

        public string AddPartNumber { get; set; }

        public decimal? AddQty { get; set; }

        public string AddGenerateRequirement { get; set; }
    }
}
