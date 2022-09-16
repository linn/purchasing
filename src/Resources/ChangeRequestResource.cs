namespace Linn.Purchasing.Resources
{
    using System;

    public class ChangeRequestResource
    {
        public string DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public string DateEntered { get; set; }

        public string ChangeState { get; set; }

        public string ReasonForChange { get; set; }

        public string DescriptionOfChange { get; set; }
    }
}
