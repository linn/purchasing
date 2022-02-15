﻿namespace Linn.Purchasing.Domain.LinnApps.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Linn.Common.Persistence;
    using Linn.Common.Reporting.Layouts;
    using Linn.Common.Reporting.Models;
    using Linn.Purchasing.Domain.LinnApps.ExternalServices;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using MoreLinq;

    public class SpendsReportService : ISpendsReportService
    {
        private readonly IPurchaseLedgerPack purchaseLedgerPack;

        private readonly IReportingHelper reportingHelper;

        private readonly IQueryRepository<SupplierSpend> spendsRepository;

        private readonly IRepository<VendorManager, string> vendorManagerRepository;

        //private readonly IRepository<PurchaseOrder, int> purchaseOrderRepository;

        //private readonly IRepository<Supplier, int> supplierRepository;

        //private readonly IQueryRepository<Part> partRepository;

        public SpendsReportService(
            IQueryRepository<SupplierSpend> spendsRepository,
            IRepository<VendorManager, string> vendorManagerRepository,
            IPurchaseLedgerPack purchaseLedgerPack,
            //IRepository<PurchaseOrder, int> purchaseOrderRepository,
            //IRepository<Supplier, int> supplierRepository,
            //IQueryRepository<Part> partRepository,
            IReportingHelper reportingHelper)
        {
            this.spendsRepository = spendsRepository;
            this.vendorManagerRepository = vendorManagerRepository;
            this.purchaseLedgerPack = purchaseLedgerPack;
            this.reportingHelper = reportingHelper;
            //this.purchaseOrderRepository = purchaseOrderRepository;
            //this.supplierRepository = supplierRepository;
            //this.partRepository = partRepository;
        }

        public ResultsModel GetSpendBySupplierReport(string vendorManagerId)
        {
            var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;

            var supplierSpends = this.spendsRepository.FilterBy(
                        x => x.LedgerPeriod >= previousYearStartLedgerPeriod && x.LedgerPeriod <= currentLedgerPeriod
                             && (string.IsNullOrWhiteSpace(vendorManagerId) || x.Supplier.VendorManager == vendorManagerId))
                    .ToList();

            var vendorManagerName = "ALL";
            if (!string.IsNullOrWhiteSpace(vendorManagerId))
            {
                var vendorManager = this.vendorManagerRepository.FindById(vendorManagerId);
                vendorManagerName = $"{vendorManagerId} - {vendorManager.Employee.FullName} ({vendorManager.UserNumber})";
            }

            var reportLayout = new SimpleGridLayout(
                this.reportingHelper,
                CalculationValueModelType.TextValue,
                null,
                $"Spend by supplier report for Vendor Manager: {vendorManagerName}. For this financial year and last, excludes factors & VAT.");

            AddSupplierReportColumns(reportLayout);

            var values = new List<CalculationValueModel>();

            var distinctSupplierSpends = supplierSpends.DistinctBy(x => x.SupplierId).Select(
                x => new SupplierSpendWithTotals
                         {
                             BaseTotal = x.BaseTotal.HasValue ? x.BaseTotal.Value : 0,
                             LedgerPeriod = x.LedgerPeriod,
                             Supplier = x.Supplier,
                             SupplierId = x.SupplierId,
                             MonthTotal =
                                 supplierSpends.Where(
                                         s => s.SupplierId == x.SupplierId && s.LedgerPeriod == currentLedgerPeriod)
                                     .Sum(z => z.BaseTotal.HasValue ? z.BaseTotal.Value : 0),
                             YearTotal =
                                 supplierSpends.Where(
                                         s => s.SupplierId == x.SupplierId && s.LedgerPeriod >= yearStartLedgerPeriod)
                                     .Sum(z => z.BaseTotal.HasValue ? z.BaseTotal.Value : 0),
                             PrevYearTotal = supplierSpends.Where(
                                     s => s.SupplierId == x.SupplierId
                                          && s.LedgerPeriod >= previousYearStartLedgerPeriod
                                          && s.LedgerPeriod < yearStartLedgerPeriod)
                                 .Sum(z => z.BaseTotal.HasValue ? z.BaseTotal.Value : 0)
                         }).OrderByDescending(x => x.PrevYearTotal).ThenByDescending(s => s.YearTotal).ThenByDescending(s => s.MonthTotal);

            foreach (var supplier in distinctSupplierSpends)
            {
                ExtractDetailsForSupplierReport(values, supplier);
            }

            reportLayout.SetGridData(values);
            var model = reportLayout.GetResultsModel();

            return model;
        }

        public ResultsModel GetSpendByPartReport(int supplierId)
        {
            throw new NotImplementedException();
            //var currentLedgerPeriod = this.purchaseLedgerPack.GetLedgerPeriod();
            //var yearStartLedgerPeriod = this.purchaseLedgerPack.GetYearStartLedgerPeriod();
            //var previousYearStartLedgerPeriod = yearStartLedgerPeriod - 12;

            //var supplierSpends = this.spendsRepository.FilterBy(
            //            x => x.LedgerPeriod >= previousYearStartLedgerPeriod && x.LedgerPeriod <= currentLedgerPeriod
            //                 && x.SupplierId == supplierId).ToList();

            //var purchaseOrders = this.purchaseOrderRepository.FilterBy(
            //    x => x.SupplierId == supplierId &&
            //         supplierSpends.Any(s => x.Details.Any(d => d.Line == s.OrderLine && d.OrderNumber == s.OrderNumber)));

            //var supplier = this.supplierRepository.FindById(supplierId);


            //var reportLayout = new SimpleGridLayout(
            //    this.reportingHelper,
            //    CalculationValueModelType.TextValue,
            //    null,
            //    $"Spend by part report for Supplier: {supplier.Name}. For this financial year and last, excludes factors & VAT.");

            //AddPartReportColumns(reportLayout);

            //var values = new List<CalculationValueModel>();

            //var partSpends = supplierSpends.Select(
            //    s => new PartSpend
            //    {
            //        LedgerPeriod = s.LedgerPeriod,
            //        BaseTotal = s.BaseTotal,
            //        OrderNumber = s.OrderNumber,
            //        OrderLine = s.OrderLine,
            //        PartNumber = purchaseOrders.First(po => po.OrderNumber == s.OrderNumber).Details
            //                     .First(x => x.Line == s.OrderLine).PartNumber
            //    });


            //var distinctPartSpends = partSpends.DistinctBy(x => x.PartNumber).Select(
            //    x => new PartSpendWithTotals
            //    {
            //        BaseTotal = x.BaseTotal,
            //        LedgerPeriod = x.LedgerPeriod,
            //        PartNumber = x.PartNumber,
            //        PartDescription = this.partRepository.FindBy(p => p.PartNumber == x.PartNumber).Description,
            //        MonthTotal = partSpends.Where(
            //                             s => s.PartNumber == x.PartNumber && s.LedgerPeriod == currentLedgerPeriod)
            //                         .Sum(z => z.BaseTotal),
            //        YearTotal = partSpends.Where(
            //                             s => s.PartNumber == x.PartNumber && s.LedgerPeriod >= yearStartLedgerPeriod)
            //                         .Sum(z => z.BaseTotal),
            //        PrevYearTotal = partSpends.Where(
            //                         s => s.PartNumber == x.PartNumber
            //                              && s.LedgerPeriod >= previousYearStartLedgerPeriod
            //                              && s.LedgerPeriod < yearStartLedgerPeriod)
            //                     .Sum(z => z.BaseTotal)
            //    }).OrderByDescending(x => x.PrevYearTotal).ThenByDescending(s => s.YearTotal).ThenByDescending(s => s.MonthTotal);


            //foreach (var part in distinctPartSpends)
            //{
            //    ExtractDetailsForPartReport(values, part);
            //}

            //reportLayout.SetGridData(values);
            //var model = reportLayout.GetResultsModel();

            //return model;
        }

        private static void AddSupplierReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("SupplierId", "Supplier Id", GridDisplayType.TextValue),
                        new AxisDetailsModel("Name", "Name", GridDisplayType.TextValue),
                        new AxisDetailsModel("LastYear", "Last Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisYear", "This Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisMonth", "This Month", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });
        }

        private static void AddPartReportColumns(SimpleGridLayout reportLayout)
        {
            reportLayout.AddColumnComponent(
                null,
                new List<AxisDetailsModel>
                    {
                        new AxisDetailsModel("PartNumber", "PartNumber", GridDisplayType.TextValue)
                            {
                                AllowWrap = false
                            },
                        new AxisDetailsModel("Description", "Description", GridDisplayType.TextValue),
                        new AxisDetailsModel("LastYear", "Last Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisYear", "This Year", GridDisplayType.Value) { DecimalPlaces = 2 },
                        new AxisDetailsModel("ThisMonth", "This Month", GridDisplayType.Value) { DecimalPlaces = 2 }
                    });
        }

        private static void ExtractDetailsForSupplierReport(ICollection<CalculationValueModel> values, SupplierSpendWithTotals supplier)
        {
            var currentRowId = $"{supplier.SupplierId}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "SupplierId", TextDisplay = $"{supplier.SupplierId}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "Name", TextDisplay = supplier.Supplier.Name
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "LastYear",
                        Value = supplier.PrevYearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId, ColumnId = "ThisYear",
                        Value = supplier.YearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisMonth",
                        Value = supplier.MonthTotal
                    });
        }

        private static void ExtractDetailsForPartReport(ICollection<CalculationValueModel> values, PartSpendWithTotals part)
        {
            var currentRowId = $"{part.PartNumber}";
            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "PartNumber",
                        TextDisplay = $"{part.PartNumber}"
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "Description",
                        TextDisplay = part.PartDescription
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "LastYear",
                        Value = part.PrevYearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisYear",
                        Value = part.YearTotal
                    });

            values.Add(
                new CalculationValueModel
                    {
                        RowId = currentRowId,
                        ColumnId = "ThisMonth",
                        Value = part.MonthTotal
                    });
        }
    }
}
