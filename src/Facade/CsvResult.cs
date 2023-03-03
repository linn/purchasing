namespace Linn.Purchasing.Facade
{
    public class CsvResult<T>
    {
        public CsvResult(string title)
        {
            this.Title = $"{title}.csv";
        }

        public T Data { get; set; }

        public string Title { get; }
    }
}
