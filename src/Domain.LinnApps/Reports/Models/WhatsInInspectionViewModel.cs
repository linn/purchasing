﻿namespace Linn.Purchasing.Domain.LinnApps.Reports.Models
{
    using System;

    public class WhatsInInspectionViewModel
    {
        public string PartNumber { get; set; }

        public string Description { get; set; }

        public string OurUnitOfMeasure { get; set; }

        public decimal QtyInStock { get; set; }

        public decimal QtyInInspection { get; set; }

        public DateTime? MinDate { get; set; }
    }
}
