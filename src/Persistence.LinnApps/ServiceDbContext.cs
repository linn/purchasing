namespace Linn.Purchasing.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public DbSet<PartSupplier> PartSuppliers { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<PackagingGroup> PackagingGroups { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public DbSet<PurchaseOrderDelivery> PurchaseOrderDeliveries { get; set; }

        public DbSet<SigningLimit> SigningLimits { get; set; }

        public DbSet<SigningLimitLog> SigningLimitLogs { get; set; }

        public DbSet<Tariff> Tariffs { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<OrderMethod> OrderMethods { get; set; }

        public DbSet<LinnDeliveryAddress> LinnDeliveryAddresses { get; set; }

        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }

        public DbSet<PurchaseLedger> PurchaseLedgers { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<PreferredSupplierChange> PreferredSupplierChanges { get; set; }

        public DbSet<PriceChangeReason> PriceChangeReasons { get; set; }

        public DbSet<PartHistoryEntry> PartHistory { get; set; }

        public DbSet<PartCategory> PartCategories { get; set; }

        public DbSet<SupplierOrderHoldHistoryEntry> SupplierOrderHoldHistories { get; set; }

        public DbSet<VendorManager> VendorManagers { get; set; }

        public DbSet<SupplierSpend> SupplierSpends { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            base.OnModelCreating(builder);
            this.BuildPartSuppliers(builder);
            this.BuildParts(builder);
            this.BuildSuppliers(builder);
            this.BuildOrderMethods(builder);
            this.BuildEmployees(builder);
            this.BuildPackagingGroups(builder);
            this.BuildManufacturers(builder);
            this.BuildAddresses(builder);
            this.BuildPurchaseOrders(builder);
            this.BuildPurchaseOrderDetails(builder);
            this.BuildPurchaseOrderDeliveries(builder);
            this.BuildTariffs(builder);
            this.BuildSigningLimits(builder);
            this.BuildSigningLimitLogs(builder);
            this.BuildCurrencies(builder);
            this.BuildOrderMethods(builder);
            this.BuildLinnDeliveryAddresses(builder);
            this.BuildUnitsOfMeasure(builder);
            this.BuildPurchaseLedgers(builder);
            this.BuildTransactionTypes(builder);
            this.BuildPreferredSupplierChanges(builder);
            this.BuildPriceChangeReasons(builder);
            this.BuildPartHistory(builder);
            this.BuildPartCategories(builder);
            this.BuildSupplierOrderHoldHistories(builder);
            this.BuildVendorManagers(builder);
            this.BuildSpendsView(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var host = ConfigurationManager.Configuration["DATABASE_HOST"];
            var userId = ConfigurationManager.Configuration["DATABASE_USER_ID"];
            var password = ConfigurationManager.Configuration["DATABASE_PASSWORD"];
            var serviceId = ConfigurationManager.Configuration["DATABASE_NAME"];

            var dataSource =
                $"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={serviceId})(SERVER=dedicated)))";

            var connectionString = $"Data Source={dataSource};User Id={userId};Password={password};";

            optionsBuilder.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11"));
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.EnableSensitiveDataLogging(true);
            base.OnConfiguring(optionsBuilder);
        }

        private void BuildPriceChangeReasons(ModelBuilder builder)
        {
            var entity = builder.Entity<PriceChangeReason>().ToTable("PRICE_CHANGE_REASONS");
            entity.HasKey(e => e.ReasonCode);
            entity.Property(e => e.ReasonCode).HasColumnName("REASON_CODE");
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION");
        }

        private void BuildPreferredSupplierChanges(ModelBuilder builder)
        {
            var entity = builder.Entity<PreferredSupplierChange>().ToTable("PREFERRED_SUPPLIER_CHANGES");
            entity.HasKey(e => new { e.PartNumber, e.Seq });
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER");
            entity.Property(e => e.Seq).HasColumnName("SEQ");
            entity.Property(e => e.DateChanged).HasColumnName("DATE_CHANGED");
            entity.Property(e => e.Remarks).HasColumnName("REMARKS");
            entity.Property(e => e.NewPrice).HasColumnName("NEW_PRICE");
            entity.Property(e => e.OldPrice).HasColumnName("OLD_PRICE");
            entity.HasOne(e => e.OldSupplier).WithMany().HasForeignKey("OLD_SUPPLIER_ID");
            entity.HasOne(e => e.NewSupplier).WithMany().HasForeignKey("NEW_SUPPLIER_ID");
            entity.HasOne(e => e.ChangeReason).WithMany().HasForeignKey("CHANGE_REASON");
            entity.HasOne(e => e.ChangedBy).WithMany().HasForeignKey("CHANGED_BY");
            entity.HasOne(e => e.NewCurrency).WithMany().HasForeignKey("NEW_CURRENCY");
            entity.HasOne(e => e.OldCurrency).WithMany().HasForeignKey("OLD_CURRENCY");
            entity.Property(e => e.BaseOldPrice).HasColumnName("BASE_OLD_PRICE");
            entity.Property(e => e.BaseNewPrice).HasColumnName("BASE_NEW_PRICE");
        }

        private void BuildPartSuppliers(ModelBuilder builder)
        {
            var entity = builder.Entity<PartSupplier>().ToTable("PART_SUPPLIERS");
            entity.HasKey(e => new { e.PartNumber, e.SupplierId });
            entity.Property(e => e.ReelOrBoxQty).HasColumnName("REEL_OR_BOX_QTY");
            entity.Property(e => e.RohsCompliant).HasColumnName("ROHS_COMPLIANT").HasMaxLength(1);
            entity.Property(e => e.RohsCategory).HasColumnName("ROHS_CATEGORY").HasMaxLength(6);
            entity.Property(e => e.RohsComments).HasColumnName("ROHS_COMMENTS").HasMaxLength(160);
            entity.HasOne(e => e.PackagingGroup).WithMany().HasForeignKey("PAGRP_ID");
            entity.HasOne(e => e.CreatedBy).WithMany().HasForeignKey("CREATED_BY");
            entity.Property(e => e.WebAddress).HasColumnName("WEB_ADDRESS").HasMaxLength(200);
            entity.Property(e => e.MinimumOrderQty).HasColumnName("MINIMUM_ORDER_QTY");
            entity.Property(e => e.OrderIncrement).HasColumnName("ORDER_INCREMENT");
            entity.Property(e => e.CurrencyUnitPrice).HasColumnName("CURRENCY_UNIT_PRICE_OURS");
            entity.Property(e => e.LeadTimeWeeks).HasColumnName("LEAD_TIME_WEEKS");
            entity.HasOne(e => e.MadeInvalidBy).WithMany().HasForeignKey("MADE_INVALID_BY");
            entity.Property(e => e.OrderConversionFactor).HasColumnName("ORDER_CONV_FACTOR_US_TO_THEM");
            entity.Property(e => e.UnitOfMeasure).HasColumnName("ORDER_UNIT_OF_MEASURE").HasMaxLength(14);
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.HasOne(e => e.Part).WithMany().HasForeignKey(p => p.PartNumber);
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.HasOne(e => e.Supplier).WithMany().HasForeignKey(e => e.SupplierId);
            entity.HasOne(e => e.OrderMethod).WithMany().HasForeignKey("PL_ORDER_METHOD");
            entity.HasOne(e => e.Currency).WithMany().HasForeignKey("CURR_CODE");
            entity.Property(e => e.MinimumDeliveryQty).HasColumnName("MINIMUM_DELIVERY_QTY");
            entity.Property(e => e.DamagesPercent).HasColumnName("DAMAGES_PERCENT");
            entity.Property(e => e.DateCreated).HasColumnName("DATE_CREATED");
            entity.Property(e => e.DateInvalid).HasColumnName("DATE_INVALID");
            entity.Property(e => e.SupplierDesignation).HasColumnName("SUPPLIER_DESIGNATION").HasMaxLength(2000);
            entity.Property(e => e.DeliveryInstructions).HasColumnName("DELIVERY_INSTRUCTIONS").HasMaxLength(200);
            entity.Property(e => e.OverbookingAllowed).HasColumnName("OVERBOOKING_ALLOWED").HasMaxLength(1);
            entity.Property(e => e.BaseOurUnitPrice).HasColumnName("BASE_OUR_UNIT_PRICE");
            entity.Property(e => e.NotesForBuyer).HasColumnName("NOTES_FOR_BUYER").HasMaxLength(200);
            entity.HasOne(e => e.DeliveryAddress).WithMany().HasForeignKey("DELIVERY_ADDRESS_ID");
            entity.Property(e => e.JitReorderWeeks).HasColumnName("JIT_REORDER_WEEKS");
            entity.Property(e => e.JitReorderOrderNumber).HasColumnName("JIT_REORDER_ORDER_NUMBER");
            entity.Property(e => e.JitReorderOrderLine).HasColumnName("JIT_REORDER_ORDER_LINE");
            entity.Property(e => e.JitBookinOrderNumber).HasColumnName("JIT_BOOKIN_ORDER_NUMBER");
            entity.Property(e => e.JitBookinOrderLine).HasColumnName("JIT_BOOKIN_ORDER_LINE");
            entity.Property(e => e.JitStatus).HasColumnName("JIT_STATUS").HasMaxLength(10);
            entity.Property(e => e.VendorPartNumber).HasColumnName("VENDOR_PART_NUMBER").HasMaxLength(20);
            entity.HasOne(e => e.Manufacturer).WithMany().HasForeignKey("MANUFACTURER");
            entity.Property(e => e.ManufacturerPartNumber).HasColumnName("MANUFACTURER_PART_NUMBER").HasMaxLength(20);
            entity.Property(e => e.OurCurrencyPriceToShowOnOrder).HasColumnName("OUR_CURR_PRICE_SHOW_ON_ORDER");
            entity.HasOne(e => e.Tariff).WithMany().HasForeignKey("TARIFF_ID");
            entity.Property(e => e.DateRohsCompliant).HasColumnName("DATE_ROHS_COMPLIANT");
            entity.Property(e => e.PackWasteStatus).HasColumnName("PACK_WASTE_STATUS").HasMaxLength(1);
            entity.Property(e => e.ContractLeadTimeWeeks).HasColumnName("CONTRACT_LEAD_TIME_WEEKS");
            entity.Property(e => e.DutyPercent).HasColumnName("DUTY_PERCENT");
            entity.Property(e => e.SupplierRanking).HasColumnName("SUPPLIER_RANKING");
        }

        private void BuildPartHistory(ModelBuilder builder)
        {
            var entity = builder.Entity<PartHistoryEntry>().ToTable("PART_HISTORY");
            entity.HasKey(e => new { e.PartNumber, e.Seq });
            entity.Property(e => e.ChangedBy).HasColumnName("CHANGED_BY");
            entity.Property(e => e.ChangeType).HasColumnName("CHANGE_TYPE").HasMaxLength(20);
            entity.Property(e => e.DateChanged).HasColumnName("DATE_CHANGED");
            entity.Property(e => e.NewBaseUnitPrice).HasColumnName("NEW_BASE_UNIT_PRICE");
            entity.Property(e => e.NewBomType).HasColumnName("NEW_BOM_TYPE").HasMaxLength(1);
            entity.Property(e => e.NewCurrency).HasColumnName("NEW_CURRENCY").HasMaxLength(4);
            entity.Property(e => e.NewCurrencyUnitPrice).HasColumnName("NEW_CURRENCY_UNIT_PRICE");
            entity.Property(e => e.NewLabourPrice).HasColumnName("NEW_LABOUR_PRICE");
            entity.Property(e => e.NewMaterialPrice).HasColumnName("NEW_MATERIAL_PRICE");
            entity.Property(e => e.NewPreferredSupplierId).HasColumnName("NEW_PREFERRED_SUPPLIER_ID");
            entity.Property(e => e.OldBaseUnitPrice).HasColumnName("OLD_BASE_UNIT_PRICE");
            entity.Property(e => e.OldBomType).HasColumnName("OLD_BOM_TYPE").HasMaxLength(1);
            entity.Property(e => e.OldCurrency).HasColumnName("OLD_CURRENCY").HasMaxLength(4);
            entity.Property(e => e.OldCurrencyUnitPrice).HasColumnName("OLD_CURRENCY_UNIT_PRICE");
            entity.Property(e => e.OldLabourPrice).HasColumnName("OLD_LABOUR_PRICE");
            entity.Property(e => e.OldMaterialPrice).HasColumnName("OLD_MATERIAL_PRICE");
            entity.Property(e => e.OldPreferredSupplierId).HasColumnName("OLD_PREFERRED_SUPPLIER_ID");
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(e => e.PriceChangeReason).HasColumnName("PRICE_CHANGE_REASON").HasMaxLength(10);
            entity.Property(e => e.Remarks).HasColumnName("REMARKS").HasMaxLength(200);
            entity.Property(e => e.Seq).HasColumnName("SEQ");
        }

        private void BuildPartCategories(ModelBuilder builder)
        {
            var q = builder.Entity<PartCategory>().ToTable("PART_CATEGORIES");
            q.HasKey(e => e.Category);
            q.Property(e => e.Category).HasColumnName("CATEGORY").HasMaxLength(2);
            q.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(30);
        }

        private void BuildParts(ModelBuilder builder)
        {
            var entity = builder.Entity<Part>().ToTable("PARTS");
            entity.HasKey(a => a.PartNumber);
            entity.Property(a => a.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(a => a.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
            entity.Property(a => a.StockControlled).HasColumnName("STOCK_CONTROLLED").HasMaxLength(1);
            entity.Property(a => a.Id).HasColumnName("BRIDGE_ID");
            entity.Property(a => a.BomType).HasColumnName("BOM_TYPE");
            entity.Property(a => a.BaseUnitPrice).HasColumnName("BASE_UNIT_PRICE");
            entity.Property(a => a.MaterialPrice).HasColumnName("MATERIAL_PRICE");
            entity.Property(a => a.LabourPrice).HasColumnName("LABOUR_PRICE");
            entity.HasOne(a => a.Currency).WithMany().HasForeignKey("CURRENCY");
            entity.HasOne(a => a.PreferredSupplier).WithMany().HasForeignKey("PREFERRED_SUPPLIER");
            entity.Property(a => a.CurrencyUnitPrice).HasColumnName("CURRENCY_UNIT_PRICE");
        }

        private void BuildSuppliers(ModelBuilder builder)
        {
            var entity = builder.Entity<Supplier>().ToTable("SUPPLIERS");
            entity.HasKey(a => a.SupplierId);
            entity.Property(a => a.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.Property(a => a.Name).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(a => a.Planner).HasColumnName("PLANNER");
            entity.Property(a => a.VendorManager).HasColumnName("VENDOR_MANAGER").HasMaxLength(1);
            entity.Property(a => a.WebAddress).HasColumnName("WEB_ADDRESS").HasMaxLength(300);
            entity.Property(a => a.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(25);
            entity.Property(a => a.OrderContactMethod).HasColumnName("PREFERRED_CONTACT_METHOD").HasMaxLength(20);
            entity.Property(a => a.InvoiceContactMethod).HasColumnName("INV_PREFERRED_CONTACT_METHOD").HasMaxLength(20);
            entity.Property(a => a.LiveOnOracle).HasColumnName("LIVE_ON_ORACLE").HasMaxLength(2);
            entity.Property(a => a.SuppliersReference).HasColumnName("SUPPLIERS_REFERENCE_FOR_US").HasMaxLength(30);
            entity.HasOne(a => a.InvoiceGoesTo).WithMany().HasForeignKey("INVOICE_GOES_TO_SUPP");
            entity.Property(a => a.ExpenseAccount).HasColumnName("EXPENSE_ACCOUNT").HasMaxLength(1);
            entity.Property(a => a.PaymentDays).HasColumnName("PAYMENT_DAYS");
            entity.Property(a => a.PaymentMethod).HasColumnName("PAYMENT_METHOD").HasMaxLength(20);
            entity.Property(a => a.PaysInFc).HasColumnName("PAYS_IN_FC").HasMaxLength(1);
            entity.HasOne(a => a.Currency).WithMany().HasForeignKey("CURRENCY");
            entity.Property(a => a.AccountingCompany).HasColumnName("ACCOUNTING_COMPANY").HasMaxLength(10);
            entity.Property(a => a.ApprovedCarrier).HasColumnName("APPROVED_CARRIER").HasMaxLength(1);
            entity.Property(a => a.VatNumber).HasColumnName("VAT_NUMBER").HasMaxLength(20);
            entity.HasOne(a => a.PartCategory).WithMany().HasForeignKey("PART_CATEGORY");
            entity.Property(a => a.OrderHold).HasColumnName("ORDER_HOLD").HasMaxLength(1);
            entity.Property(a => a.NotesForBuyer).HasColumnName("NOTES_FOR_BUYER").HasMaxLength(200);
            entity.Property(a => a.DeliveryDay).HasColumnName("DELIVERY_DAY").HasMaxLength(10);
            entity.HasOne(a => a.RefersToFc).WithMany().HasForeignKey("REFERS_TO_FC_SUPPLIER");
            entity.Property(a => a.PmDeliveryDaysGrace).HasColumnName("PM_DELIVERY_DAYS_GRACE");
        }

        private void BuildSupplierOrderHoldHistories(ModelBuilder builder)
        {
            var entity = builder.Entity<SupplierOrderHoldHistoryEntry>().ToTable("SUPPLIER_ORDER_HOLD_HISTORIES");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("SOHH_ID");
            entity.Property(a => a.DateOffHold).HasColumnName("DATE_OFF_HOLD");
            entity.Property(a => a.DateOnHold).HasColumnName("DATE_ON_HOLD");
            entity.Property(a => a.PutOnHoldBy).HasColumnName("PUT_ON_HOLD_BY");
            entity.Property(a => a.TakenOffHoldBy).HasColumnName("TAKEN_OFF_HOLD_BY").HasMaxLength(200);
            entity.Property(a => a.ReasonOnHold).HasColumnName("REASON_ON_HOLD").HasMaxLength(200);
            entity.Property(a => a.ReasonOffHold).HasColumnName("REASON_OFF_HOLD").HasMaxLength(200);
            entity.Property(a => a.SupplierId).HasColumnName("SUPP_SUPPLIER_ID");
        }

        private void BuildOrderMethods(ModelBuilder builder)
        {
            var entity = builder.Entity<OrderMethod>().ToTable("PL_ORDER_METHODS");
            entity.HasKey(m => m.Name);
            entity.Property(a => a.Name).HasColumnName("PL_ORDER_METHOD").HasMaxLength(10);
            entity.Property(a => a.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        }

        private void BuildManufacturers(ModelBuilder builder)
        {
            var entity = builder.Entity<Manufacturer>().ToTable("MANUFACTURERS");
            entity.HasKey(m => m.Code);
            entity.Property(e => e.Code).HasColumnName("CODE").HasMaxLength(6);
            entity.Property(e => e.Name).HasColumnName("DESCRIPTION").HasMaxLength(30);
        }

        private void BuildPackagingGroups(ModelBuilder builder)
        {
            var entity = builder.Entity<PackagingGroup>().ToTable("PACKAGING_GROUPS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PAGRP_ID");
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        }

        private void BuildEmployees(ModelBuilder builder)
        {
            var entity = builder.Entity<Employee>().ToTable("AUTH_USER_NAME_VIEW");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("USER_NUMBER");
            entity.Property(e => e.FullName).HasColumnName("USER_NAME").HasMaxLength(4000);
        }

        private void BuildAddresses(ModelBuilder builder)
        {
            var entity = builder.Entity<Address>().ToTable("ADDRESSES_VIEW");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("ADDRESS_ID");
            entity.Property(a => a.FullAddress).HasColumnName("ADDRESS");
        }

        private void BuildPurchaseOrders(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrder>().ToTable("PL_ORDERS");
            entity.HasKey(o => o.OrderNumber);
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.Cancelled).HasColumnName("CANCELLED").HasMaxLength(1);
            entity.Property(o => o.SupplierId).HasColumnName("SUPP_SUPPLIER_ID");
            entity.HasOne(o => o.Supplier).WithMany().HasForeignKey(o => o.SupplierId);
            entity.Property(o => o.DocumentType).HasColumnName("DOCUMENT_TYPE");
            entity.Property(o => o.OrderDate).HasColumnName("DATE_OF_ORDER");
            entity.Property(o => o.Overbook).HasColumnName("OVERBOOK");
            entity.Property(o => o.OverbookQty).HasColumnName("OVERBOOK_QTY");
            entity.HasOne(o => o.Currency).WithMany().HasForeignKey("CURR_CODE");
        }

        private void BuildPurchaseOrderDetails(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrderDetail>().ToTable("PL_ORDER_DETAILS");
            entity.HasKey(a => new { a.OrderNumber, a.Line });
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.Cancelled).HasColumnName("CANCELLED").HasMaxLength(1);
            entity.Property(o => o.Line).HasColumnName("ORDER_LINE");
            entity.Property(o => o.RohsCompliant).HasColumnName("ROHS_COMPLIANT");
            entity.Property(o => o.OurQty).HasColumnName("OUR_QTY");
            entity.Property(o => o.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(o => o.SuppliersDesignation).HasColumnName("SUPPLIERS_DESIGNATION").HasMaxLength(2000);
            entity.HasOne(d => d.PurchaseOrder).WithMany(o => o.Details)
                .HasForeignKey(d => d.OrderNumber);
            entity.HasMany(d => d.PurchaseDeliveries).WithOne(o => o.PurchaseOrderDetail)
                .HasForeignKey(o => new { o.OrderNumber, o.OrderLine });
            entity.Property(o => o.BaseNetTotal).HasColumnName("BASE_NET_TOTAL").HasMaxLength(18);
            entity.Property(o => o.NetTotalCurrency).HasColumnName("NET_TOTAL").HasMaxLength(18);
        }

        private void BuildPurchaseOrderDeliveries(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrderDelivery>().ToTable("PL_DELIVERIES");
            entity.HasKey(a => new { a.DeliverySeq, a.OrderNumber, a.OrderLine });
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.Cancelled).HasColumnName("CANCELLED").HasMaxLength(1);
            entity.Property(o => o.OrderLine).HasColumnName("ORDER_LINE");
            entity.Property(o => o.DeliverySeq).HasColumnName("DELIVERY_SEQ");

            entity.Property(o => o.DateAdvised).HasColumnName("ADVISED_DATE");
            entity.Property(o => o.DateRequested).HasColumnName("REQUESTED_DATE");

            entity.Property(o => o.OurDeliveryQty).HasColumnName("OUR_DELIVERY_QTY").HasMaxLength(19);
            entity.Property(o => o.OrderDeliveryQty).HasColumnName("ORDER_DELIVERY_QTY").HasMaxLength(19);
            entity.Property(o => o.QtyNetReceived).HasColumnName("QTY_NET_RECEIVED").HasMaxLength(19);

            entity.HasOne(d => d.PurchaseOrderDetail).WithMany(o => o.PurchaseDeliveries);
            entity.Property(o => o.NetTotal).HasColumnName("NET_TOTAL").HasMaxLength(18);
        }

        private void BuildSigningLimits(ModelBuilder builder)
        {
            var entity = builder.Entity<SigningLimit>().ToTable("PURCH_SIGNING_LIMITS");
            entity.HasKey(m => m.UserNumber);
            entity.Property(e => e.UserNumber).HasColumnName("USER_NUMBER");
            entity.Property(a => a.ProductionLimit).HasColumnName("PRODUCTION_SIGNING_LIMIT");
            entity.Property(a => a.SundryLimit).HasColumnName("SUNDRY_LIMIT");
            entity.Property(a => a.Unlimited).HasColumnName("UNLIMITED").HasMaxLength(1);
            entity.Property(a => a.ReturnsAuthorisation).HasColumnName("RETURNS_AUTHORISATION").HasMaxLength(1);
            entity.HasOne(a => a.User).WithMany(e => e.SigningLimits).HasForeignKey(a => a.UserNumber);
        }

        private void BuildSigningLimitLogs(ModelBuilder builder)
        {
            var entity = builder.Entity<SigningLimitLog>().ToTable("PURCH_SIGNING_LIMITS_LOG");
            entity.HasKey(m => m.UserNumber);
            entity.Property(e => e.UserNumber).HasColumnName("USER_NUMBER");
            entity.Property(a => a.ProductionLimit).HasColumnName("PRODUCTION_SIGNING_LIMIT");
            entity.Property(a => a.SundryLimit).HasColumnName("SUNDRY_LIMIT");
            entity.Property(a => a.Unlimited).HasColumnName("UNLIMITED").HasMaxLength(1);
            entity.Property(a => a.ReturnsAuthorisation).HasColumnName("RETURNS_AUTHORISATION").HasMaxLength(1);
            entity.Property(a => a.LogId).HasColumnName("LOG_ID");
            entity.Property(a => a.LogAction).HasColumnName("LOG_ACTION").HasMaxLength(20);
            entity.Property(a => a.LogUserNumber).HasColumnName("LOG_USER_NUMBER");
            entity.Property(a => a.LogTime).HasColumnName("LOG_DATE");
        }

        private void BuildTariffs(ModelBuilder builder)
        {
            var entity = builder.Entity<Tariff>().ToTable("TARIFFS");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("TARIFF_ID");
            entity.Property(e => e.Code).HasColumnName("TARIFF_CODE");
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION");
        }

        private void BuildCurrencies(ModelBuilder builder)
        {
            var entity = builder.Entity<Currency>().ToTable("CURRENCIES");
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasColumnName("CODE");
            entity.Property(e => e.Name).HasColumnName("NAME");
        }

        private void BuildLinnDeliveryAddresses(ModelBuilder builder)
        {
            var entity = builder.Entity<LinnDeliveryAddress>().ToTable("LINN_DELIVERY_ADDRESSES");
            entity.HasKey(e => e.AddressId);
            entity.HasOne(e => e.Address).WithMany().HasForeignKey(e => e.AddressId);
            entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");
            entity.Property(e => e.Description).HasColumnName("DELIVERY_ADDRESS_DESCRIPTION");
            entity.Property(e => e.IsMainDeliveryAddress).HasColumnName("MAIN_DELIVERY_ADDRESS");
            entity.Property(e => e.DateObsolete).HasColumnName("DATE_OBSOLETE");
        }

        private void BuildUnitsOfMeasure(ModelBuilder builder)
        {
            var entity = builder.Entity<UnitOfMeasure>().ToTable("UNITS_OF_MEASURE");
            entity.HasKey(e => e.Unit);
            entity.Property(e => e.Unit).HasColumnName("UNIT_OF_MEASURE");
        }

        private void BuildPurchaseLedgers(ModelBuilder builder)
        {
            var e = builder.Entity<PurchaseLedger>().ToTable("PURCHASE_LEDGER");
            e.HasKey(p => p.Pltref);
            e.Property(p => p.Pltref).HasColumnName("PL_TREF").HasMaxLength(8);
            e.Property(p => p.SupplierId).HasColumnName("SUPPLIER_ID").HasMaxLength(6);
            e.Property(p => p.OrderLine).HasColumnName("ORDER_LINE").HasMaxLength(6);
            e.Property(p => p.OrderNumber).HasColumnName("ORDER_NUMBER");
            e.Property(p => p.DatePosted).HasColumnName("DATE_POSTED");
            e.Property(p => p.PlState).HasColumnName("PL_STATE");
            e.Property(p => p.PlQuantity).HasColumnName("PL_QTY").HasMaxLength(19);
            e.Property(p => p.PlNetTotal).HasColumnName("PL_NET_TOTAL").HasMaxLength(14);
            e.Property(p => p.PlVat).HasColumnName("PL_VAT").HasMaxLength(14);
            e.Property(p => p.PlTotal).HasColumnName("PL_TOTAL").HasMaxLength(14);
            e.Property(p => p.BaseNetTotal).HasColumnName("BASE_NET_TOTAL").HasMaxLength(14);
            e.Property(p => p.BaseVatTotal).HasColumnName("BASE_VAT_TOTAL").HasMaxLength(14);
            e.Property(p => p.BaseTotal).HasColumnName("BASE_TOTAL").HasMaxLength(14);
            e.Property(p => p.InvoiceDate).HasColumnName("INVOICE_DATE");
            e.Property(p => p.PlInvoiceRef).HasColumnName("PL_INVOICE_REF").HasMaxLength(30);
            e.Property(p => p.PlDeliveryRef).HasColumnName("PL_DELIVERY_REF").HasMaxLength(20);
            e.Property(p => p.CompanyRef).HasColumnName("COMPANY_REF").HasMaxLength(8);
            e.Property(p => p.Currency).HasColumnName("CURRENCY").HasMaxLength(4);
            e.Property(p => p.LedgerPeriod).HasColumnName("LEDGER_PERIOD");
            e.Property(p => p.PostedBy).HasColumnName("POSTED_BY").HasMaxLength(6);
            e.Property(p => p.DebitNomacc).HasColumnName("DEBIT_NOMACC").HasMaxLength(6);
            e.Property(p => p.CreditNomacc).HasColumnName("CREDIT_NOMACC").HasMaxLength(6);
            e.Property(p => p.PlTransType).HasColumnName("PL_TRANS_TYPE").HasMaxLength(12);
            e.Property(p => p.BaseCurrency).HasColumnName("BASE_CURRENCY").HasMaxLength(4);
            e.Property(p => p.Carriage).HasColumnName("CARRIAGE").HasMaxLength(14);
            e.Property(p => p.UnderOver).HasColumnName("UNDER_OVER").HasMaxLength(14);
            e.Property(p => p.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasMaxLength(19);
            e.Property(p => p.LedgerStream).HasColumnName("LEDGER_STREAM").HasMaxLength(8);
            e.HasOne(a => a.TransactionType).WithMany().HasForeignKey(a => a.PlTransType);
        }

        private void BuildTransactionTypes(ModelBuilder builder)
        {
            var entity = builder.Entity<TransactionType>().ToTable("PL_TRANS_TYPES");
            entity.HasKey(m => m.TransType);
            entity.Property(e => e.TransType).HasColumnName("PL_TRANS_TYPE").HasMaxLength(12);
            entity.Property(e => e.Description).HasColumnName("TRANS_DESCRIPTION").HasMaxLength(100);
            entity.Property(e => e.DebitNomacc).HasColumnName("DEBIT_NOMACC").HasMaxLength(6);
            entity.Property(e => e.CreditNomacc).HasColumnName("CREDIT_NOMACC").HasMaxLength(6);
            entity.Property(e => e.DebitOrCredit).HasColumnName("DEBIT_OR_CREDIT").HasMaxLength(1);
            entity.Property(e => e.DateInvalid).HasColumnName("DATE_INVALID");
            entity.Property(e => e.TransactionCategory).HasColumnName("TRANS_CATEGORY").HasMaxLength(10);
        }

        private void BuildVendorManagers(ModelBuilder builder)
        {
            var entity = builder.Entity<VendorManager>().ToTable("VENDOR_MANAGERS");
            entity.HasKey(m => m.VmId);
            entity.Property(e => e.VmId).HasColumnName("VM_ID").HasMaxLength(1);
            entity.Property(e => e.UserNumber).HasColumnName("USER_NUMBER").HasMaxLength(6);
            entity.Property(e => e.PmMeasured).HasColumnName("PM_MEASURED").HasMaxLength(1);
            entity.HasOne(x => x.Employee).WithOne().HasForeignKey<VendorManager>(z => z.UserNumber);
        }

        private void BuildSpendsView(ModelBuilder builder)
        {
            var entity = builder.Entity<SupplierSpend>().ToTable("PL_PL_SUPPLIERS_EX_VAT");
            entity.HasKey(m => m.PlTref);
            entity.Property(e => e.PlTref).HasColumnName("PL_TREF");
            entity.Property(e => e.BaseTotal).HasColumnName("BASE_TOTAL");
            entity.Property(e => e.LedgerPeriod).HasColumnName("LEDGER_PERIOD");
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.HasOne(x => x.Supplier).WithMany().HasForeignKey(z => z.SupplierId);
        }
    }
}
