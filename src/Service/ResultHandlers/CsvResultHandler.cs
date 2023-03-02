namespace Linn.Purchasing.Service.ResultHandlers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using CsvHelper;

    using Linn.Common.Facade.Carter;
    using Linn.Purchasing.Facade;

    using Microsoft.AspNetCore.Http;

    // todo - move to common
    public class CsvResultHandler : IHandler
    {
        public bool CanHandle(object model, string contentType)
        {
            return contentType.IndexOf("csv", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public async Task Handle(
            HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            var result = (CsvResult) model;
            res.ContentType = "text/csv; charset=utf-8";
            res.Headers.ContentDisposition = $"attachment; filename=\"{result.Title}\"";
            var sw = new StringWriter();

            var writer = new CsvWriter(sw, CultureInfo.InvariantCulture);

            // currently only handles lists of lists of strings
            foreach (var line in result.Data)
            {
                foreach (var field in line)
                {
                    writer.WriteField(field);
                }

                await writer.NextRecordAsync();
            }

            await writer.FlushAsync();
            await res.WriteAsync(sw.ToString(), cancellationToken: cancellationToken);
        }
    }
}
