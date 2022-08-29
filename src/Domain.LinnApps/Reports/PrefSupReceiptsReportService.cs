namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;

    public class PrefSupReceiptsReportService : IPrefSupReceiptsReportService
    {
        private readonly IQueryRepository<ReceiptPrefSupDiff> receiptRepository;

        private readonly IReportingHelper reportingHelper;

        public PrefSupReceiptsReportService(
            IReportingHelper reportingHelper,
            IQueryRepository<ReceiptPrefSupDiff> receiptRepository)
        {
            this.reportingHelper = reportingHelper;
            this.receiptRepository = receiptRepository;
        }

        public string GetReportCurrencyValue(decimal currencyValue, string currency, decimal gbpValue, bool justGbp)
        {
            if (justGbp)
            {
                return gbpValue.ToString(CultureInfo.InvariantCulture);
            }

            if (currency == "GBP")
            {
                return $"{gbpValue} GBP";
            }

            return $"{currencyValue} {currency} \n{gbpValue} GBP";
        }

        public ResultsModel GetReport(DateTime fromDate, DateTime toDate, bool justGbp = false)
        {
            var results = this.receiptRepository.FilterBy(
                e => e.DateBooked >= fromDate && e.DateBooked <= toDate && e.Difference != 0)
                .OrderByDescending(e => e.Difference);

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.Value,
                null,
                $"Receipts vs Pref Sup Price");

            this.AddReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            foreach (var result in results)
            {
                var rowId = $"{result.OrderNumber}/{result.OrderLine}/{result.PlReceiptId}";

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "DateBooked", TextDisplay = result.DateBooked.ToShortDateString()
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "PartNumber", TextDisplay = result.PartNumber
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PartDescription",
                            TextDisplay = result.PartDescription
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Qty",
                            TextDisplay = result.Qty.ToString(CultureInfo.InvariantCulture)
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "Order", TextDisplay = $"{result.OrderNumber}/{result.OrderLine}"
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "Supplier",
                            TextDisplay = $"{result.SupplierId} {result.SupplierName}"
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "OrderPrice",
                            TextDisplay = this.GetReportCurrencyValue(
                                result.CurrencyUnitPrice,
                                result.OrderCurrency,
                                result.ReceiptBaseUnitPrice,
                                justGbp)
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId,
                            ColumnId = "PrefSupPrice",
                            TextDisplay = this.GetReportCurrencyValue(
                                result.PrefsupCurrencyUnitPrice,
                                result.PrefsupCurrency,
                                result.PrefsupBaseUnitPrice,
                                justGbp)
                        });

                values.Add(
                    new CalculationValueModel
                        {
                            RowId = rowId, ColumnId = "Diff", TextDisplay = $"{result.Difference}"
                        });

                values.Add(
                    new CalculationValueModel { RowId = rowId, ColumnId = "MPV", TextDisplay = $"{result.MPVReason}" });
            }

            reportLayout.SetGridData(values);

            var model = reportLayout.GetResultsModel();
            return model;
        }

        private void AddReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("DateBooked", "Date Booked", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("PartNumber", "Part Number", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("PartDescription", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("Qty", "Qty", GridDisplayType.TextValue),
                        new AxisDetailsModel("Order", "Order", GridDisplayType.TextValue),
                        new AxisDetailsModel("Supplier", "Supplier", GridDisplayType.TextValue),
                        new AxisDetailsModel("OrderPrice", "Order Price", GridDisplayType.TextValue),
                        new AxisDetailsModel("PrefSupPrice", "Prefsup Price", GridDisplayType.TextValue),
                        new AxisDetailsModel("Diff", "Diff", GridDisplayType.TextValue),
                        new AxisDetailsModel("MPV", "MPV Reason", GridDisplayType.TextValue)
                    });
        }
    }
}
