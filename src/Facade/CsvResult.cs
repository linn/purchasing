namespace Linn.Purchasing.Facade
{
    using System.Collections.Generic;

    public class CsvResult
    {
        public CsvResult(string title)
        {
            this.Title = $"{title}.csv";
        }

        public IEnumerable<IEnumerable<string>> Data { get; set; }

        public string Title { get; }
    }
}
