namespace Linn.Purchasing.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.MaterialRequirements;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.PurchaseLedger;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrderReqs;
    using Linn.Purchasing.Domain.LinnApps.PurchaseOrders;
    using Linn.Purchasing.Domain.LinnApps.Reports.Models;
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

        public DbSet<FullAddress> FullAddresses { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<SigningLimit> SigningLimits { get; set; }

        public DbSet<SigningLimitLog> SigningLimitLogs { get; set; }

        public DbSet<Tariff> Tariffs { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<OrderMethod> OrderMethods { get; set; }

        public DbSet<LinnDeliveryAddress> LinnDeliveryAddresses { get; set; }

        public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; }

        public DbSet<PurchaseLedger> PurchaseLedgers { get; set; }

        public DbSet<PreferredSupplierChange> PreferredSupplierChanges { get; set; }

        public DbSet<PriceChangeReason> PriceChangeReasons { get; set; }

        public DbSet<PartHistoryEntry> PartHistory { get; set; }

        public DbSet<PartCategory> PartCategories { get; set; }

        public DbSet<SupplierOrderHoldHistoryEntry> SupplierOrderHoldHistories { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<VendorManager> VendorManagers { get; set; }

        public DbSet<SupplierSpend> SupplierSpends { get; set; }

        public DbSet<UnacknowledgedOrders> UnacknowledgedOrders { get; set; }

        public DbSet<SuppliersWithUnacknowledgedOrders> SuppliersWithUnacknowledgedOrders { get; set; }

        public DbSet<SupplierGroupsWithUnacknowledgedOrders> SupplierGroupsWithUnacknowledgedOrders { get; set; }

        public DbSet<Planner> Planners { get; set; }

        public DbSet<SupplierGroup> SupplierGroups { get; set; }

        public DbSet<SupplierContact> SupplierContacts { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<PlCreditDebitNote> PlCreditDebitNotes { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<TqmsJobref> TqmsJobrefs { get; set; }

        public DbSet<PartReceivedRecord> TqmsView { get; set; }

        public DbSet<PurchaseOrderReq> PurchaseOrderReqs { get; set; }

        public DbSet<PurchaseOrderDelivery> PurchaseOrderDeliveries { get; set; }

        public DbSet<PurchaseOrderReqState> PurchaseOrderReqStates { get; set; }

        public DbSet<PurchaseOrderReqStateChange> PurchaseOrderReqStateChanges { get; set; }

        public DbSet<OverbookAllowedByLog> AllowOverbookLogs { get; set; }

        public DbSet<MrpRunLog> MrpRunLogs { get; set; }

        public DbSet<PartsInInspectionExcludingFails> WhatsInInspectionExcludingFailedView { get; set; }

        public DbSet<PartsInInspectionIncludingFails> WhatsInInspectionIncludingFailedView { get; set; }

        public DbSet<WhatsInInspectionPurchaseOrdersData> WhatsInInspectionPurchaseOrdersView { get; set; }

        public DbSet<ReceiptPrefSupDiff> ReceiptPrefsupDiffs { get; set; }

        public DbSet<WhatsInInspectionStockLocationsData> WhatsInInspectionStockLocationsView { get; set; }

        public DbSet<WhatsInInspectionBackOrderData> WhatsInInspectionBackOrderView { get; set; }

        public DbSet<MrMaster> MrMaster { get; set; }

        public DbSet<LedgerPeriod> LedgerPeriods { get; set; }

        public DbSet<LinnWeek> LinnWeeks { get; set; }
        
        public DbSet<CancelledOrderDetail> CancelledPurchaseOrderDetails { get; set; }

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
            this.BuildFullAddresses(builder);
            this.BuildPurchaseOrders(builder);
            this.BuildPurchaseOrderDetails(builder);
            this.BuildPurchaseOrderDeliveries(builder);
            this.BuildOverbookAllowedBy(builder);
            this.BuildTariffs(builder);
            this.BuildSigningLimits(builder);
            this.BuildSigningLimitLogs(builder);
            this.BuildCurrencies(builder);
            this.BuildLinnDeliveryAddresses(builder);
            this.BuildUnitsOfMeasure(builder);
            this.BuildPurchaseLedgers(builder);
            this.BuildTransactionTypes(builder);
            this.BuildPreferredSupplierChanges(builder);
            this.BuildPriceChangeReasons(builder);
            this.BuildPartHistory(builder);
            this.BuildPartCategories(builder);
            this.BuildSupplierOrderHoldHistories(builder);
            this.BuildAddresses(builder);
            this.BuildCountries(builder);
            this.BuildVendorManagers(builder);
            this.BuildSpendsView(builder);
            this.BuildUnacknowledgedOrderSuppliers(builder);
            this.BuildUnacknowledgedOrderSupplierGroups(builder);
            this.BuildUnacknowledgedOrders(builder);
            this.BuildPlanners(builder);
            this.BuildSupplierGroups(builder);
            this.BuildSupplierContacts(builder);
            this.BuildPersons(builder);
            this.BuildPlCreditDebitNotes(builder);
            this.BuildCreditDebitNoteTypes(builder);
            this.BuildPhoneList(builder);
            this.BuildOrganisations(builder);
            this.BuildTqmsJobRefs(builder);
            this.BuildPartsReceivedView(builder);
            this.BuildPurchaseOrderReqs(builder);
            this.BuildDepartments(builder);
            this.BuildNominals(builder);
            this.BuildPurchaseOrderReqStates(builder);
            this.BuildPurchaseOrderReqStateChanges(builder);
            this.BuildMrRunLogs(builder);
            this.BuildWhatsInInspectionExcludingFailedView(builder);
            this.BuildWhatsInInspectionIncludingFailedView(builder);
            this.BuildWhatsInInspectionPurchaseOrdersView(builder);
            this.BuildDocumentTypes(builder);
            this.BuildPrefsupVsReceiptsView(builder);
            this.BuildMrOrders(builder);
            this.BuildWhatsInInspectionStockLocationsView(builder);
            this.BuildWhatsInInspectionBackOrderView(builder);
            this.BuildMrMaster(builder);
            this.BuildLedgerPeriods(builder);
            this.BuildLinnWeeks(builder);
            this.BuildCancelledPODetails(builder);
            this.BuildPurchaseOrderPostings(builder);
            this.BuildNominalAccounts(builder);
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

        private void BuildPlanners(ModelBuilder builder)
        {
            var entity = builder.Entity<Planner>().ToTable("PLANNERS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("PLANNER");
        }

        private void BuildAddresses(ModelBuilder builder)
        {
            var entity = builder.Entity<Address>().ToTable("ADDRESSES");
            entity.HasKey(e => e.AddressId);
            entity.Property(e => e.AddressId).HasColumnName("ADDRESS_ID");
            entity.Property(e => e.Addressee).HasColumnName("ADDRESSEE").HasMaxLength(40);
            entity.Property(e => e.Addressee2).HasColumnName("ADDRESSEE_2").HasMaxLength(40);
            entity.Property(e => e.Line1).HasColumnName("ADDRESS_1").HasMaxLength(40);
            entity.Property(e => e.Line2).HasColumnName("ADDRESS_2").HasMaxLength(40);
            entity.Property(e => e.Line3).HasColumnName("ADDRESS_3").HasMaxLength(40);
            entity.Property(e => e.Line4).HasColumnName("ADDRESS_4").HasMaxLength(40);
            entity.HasOne(e => e.Country).WithMany().HasForeignKey("COUNTRY");
            entity.Property(e => e.PostCode).HasColumnName("POSTAL_CODE").HasMaxLength(20);
            entity.HasOne(a => a.FullAddress).WithMany().HasForeignKey(x => x.AddressId);
        }

        private void BuildCountries(ModelBuilder builder)
        {
            var entity = builder.Entity<Country>().ToTable("COUNTRIES");
            entity.HasKey(c => c.CountryCode);
            entity.Property(c => c.CountryCode).HasColumnName("COUNTRY_CODE").HasMaxLength(2);
            entity.Property(c => c.Name).HasColumnName("NAME").HasMaxLength(50);
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
            entity.HasOne(e => e.CreatedBy).WithMany().HasForeignKey("CREATED_BY");
            entity.Property(e => e.MinimumOrderQty).HasColumnName("MINIMUM_ORDER_QTY");
            entity.Property(e => e.OrderIncrement).HasColumnName("ORDER_INCREMENT");
            entity.Property(e => e.CurrencyUnitPrice).HasColumnName("CURRENCY_UNIT_PRICE_OURS");
            entity.Property(e => e.LeadTimeWeeks).HasColumnName("LEAD_TIME_WEEKS");
            entity.HasOne(e => e.MadeInvalidBy).WithMany().HasForeignKey("MADE_INVALID_BY");
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
            entity.HasOne(e => e.DeliveryFullAddress).WithMany().HasForeignKey("DELIVERY_ADDRESS_ID");
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
            entity.Property(e => e.PackWasteStatus).HasColumnName("PACK_WASTE_STATUS").HasMaxLength(1);
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
            entity.HasOne(a => a.OrderAddress).WithMany().HasForeignKey("ORD_ADDRESS_ID");
            entity.HasOne(a => a.InvoiceFullAddress).WithMany().HasForeignKey("INV_ADDRESS_ID");
            entity.HasOne(a => a.VendorManager).WithMany().HasForeignKey("VENDOR_MANAGER");
            entity.HasOne(a => a.Planner).WithMany().HasForeignKey("PLANNER");
            entity.HasOne(a => a.AccountController).WithMany().HasForeignKey("ACCOUNT_CONTROLLER");
            entity.Property(a => a.DateOpened).HasColumnName("DATE_OPENED");
            entity.HasOne(a => a.OpenedBy).WithMany().HasForeignKey("OPENED_BY");
            entity.Property(a => a.DateClosed).HasColumnName("DATE_CLOSED");
            entity.Property(a => a.ReasonClosed).HasColumnName("REASON_CLOSED");
            entity.HasOne(a => a.ClosedBy).WithMany().HasForeignKey("CLOSED_BY");
            entity.Property(a => a.Notes).HasColumnName("NOTES").HasMaxLength(1000);
            entity.Property(a => a.OrganisationId).HasColumnName("ORGANISATION_ID");
            entity.HasMany(s => s.SupplierContacts).WithOne().HasForeignKey(c => c.SupplierId);
            entity.Property(s => s.Country).HasColumnName("COUNTRY");
            entity.HasOne(s => s.Group).WithMany().HasForeignKey("SUPPLIER_GROUP");
        }

        private void BuildOrganisations(ModelBuilder builder)
        {
            var entity = builder.Entity<Organisation>().ToTable("ORGANISATIONS");
            entity.HasKey(c => c.OrgId);
            entity.Property(c => c.OrgId).HasColumnName("ORG_ID");
            entity.Property(c => c.AddressId).HasColumnName("ADDRESS_ID");
            entity.Property(c => c.DateCreated).HasColumnName("DATE_CREATED");
            entity.Property(c => c.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(25);
            entity.Property(c => c.EmailAddress).HasColumnName("EMAIL_ADDRESS").HasMaxLength(50);
            entity.Property(c => c.Title).HasColumnName("TITLE").HasMaxLength(80);
            entity.Property(c => c.WebAddress).HasColumnName("WEB_ADDRESS").HasMaxLength(300);
        }

        private void BuildSupplierContacts(ModelBuilder builder)
        {
            var s = builder.Entity<SupplierContact>().ToTable("SUPPLIER_CONTACTS");
            s.HasKey(a => a.ContactId);
            s.Property(e => e.ContactId).HasColumnName("CONTACT_ID");
            s.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            s.Property(e => e.IsMainInvoiceContact).HasColumnName("MAIN_INVOICE_CONTACT");
            s.Property(e => e.IsMainOrderContact).HasColumnName("MAIN_ORDER_CONTACT");
            s.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER");
            s.Property(e => e.MobileNumber).HasColumnName("MOBILE_NUMBER");
            s.Property(e => e.EmailAddress).HasColumnName("EMAIL_ADDRESS");
            s.Property(e => e.JobTitle).HasColumnName("JOB_TITLE");
            s.Property(e => e.Comments).HasColumnName("COMMENTS");
            s.HasOne(e => e.Person).WithMany().HasForeignKey("PERSON_ID");
            s.Property(e => e.DateCreated).HasColumnName("DATE_CREATED");
        }

        private void BuildPersons(ModelBuilder builder)
        {
            var p = builder.Entity<Person>().ToTable("PERSONS");
            p.HasKey(x => x.Id);
            p.Property(x => x.Id).HasColumnName("PERSON_ID");
            p.Property(x => x.FirstName).HasColumnName("FIRST_NAME").HasMaxLength(20);
            p.Property(x => x.LastName).HasColumnName("LAST_NAME").HasMaxLength(20);
            p.Property(x => x.DateCreated).HasColumnName("DATE_CREATED");
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
            entity.HasOne(e => e.PhoneListEntry).WithMany().HasForeignKey(e => e.Id);
        }

        private void BuildPhoneList(ModelBuilder builder)
        {
            var entity = builder.Entity<PhoneListEntry>().ToTable("PHONE_LIST");
            entity.HasKey(m => m.UserNumber);
            entity.Property(e => e.UserNumber).HasColumnName("USER_NUMBER");
            entity.Property(e => e.EmailAddress).HasColumnName("EMAIL_ADDRESS");
        }

        private void BuildFullAddresses(ModelBuilder builder)
        {
            var entity = builder.Entity<FullAddress>().ToTable("ADDRESSES_VIEW");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("ADDRESS_ID");
            entity.Property(a => a.AddressString).HasColumnName("ADDRESS");
        }

        private void BuildPurchaseOrders(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrder>().ToTable("PL_ORDERS");
            entity.HasKey(o => o.OrderNumber);
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.Cancelled).HasColumnName("CANCELLED").HasMaxLength(1);
            entity.Property(o => o.SupplierId).HasColumnName("SUPP_SUPPLIER_ID");
            entity.HasOne(o => o.Supplier).WithMany().HasForeignKey(o => o.SupplierId);
            entity.Property(o => o.DocumentTypeName).HasColumnName("DOCUMENT_TYPE");
            entity.HasOne(o => o.DocumentType).WithMany().HasForeignKey(o => o.DocumentTypeName);
            entity.Property(o => o.OrderDate).HasColumnName("DATE_OF_ORDER");
            entity.Property(o => o.Overbook).HasColumnName("OVERBOOK");
            entity.Property(o => o.OverbookQty).HasColumnName("OVERBOOK_QTY");
            entity.Property(o => o.CurrencyCode).HasColumnName("CURR_CODE");
            entity.HasOne(o => o.Currency).WithMany().HasForeignKey(o => o.CurrencyCode);
            entity.Property(o => o.OrderContactName).HasColumnName("CONTACT_NAME");
            entity.HasMany(o => o.Details).WithOne(d => d.PurchaseOrder).HasForeignKey(d => d.OrderNumber);
            entity.Property(o => o.OrderMethodName).HasColumnName("PL_ORDER_METHOD");
            entity.HasOne(o => o.OrderMethod).WithMany().HasForeignKey(o => o.OrderMethodName);
            entity.Property(o => o.ExchangeRate).HasColumnName("EXCHANGE_RATE").HasMaxLength(19);
            entity.Property(o => o.IssuePartsToSupplier).HasColumnName("ISSUE_PARTS_TO_SUPPLIER").HasMaxLength(1);
            entity.Property(o => o.DeliveryAddressId).HasColumnName("DELIVERY_ADDRESS");
            entity.HasOne(o => o.DeliveryAddress).WithMany().HasForeignKey(o => o.DeliveryAddressId);
            entity.Property(o => o.RequestedById).HasColumnName("REQUESTED_BY");
            entity.HasOne(o => o.RequestedBy).WithMany().HasForeignKey(o => o.RequestedById);
            entity.Property(o => o.EnteredById).HasColumnName("ENTERED_BY");
            entity.HasOne(o => o.EnteredBy).WithMany().HasForeignKey(o => o.EnteredById);
            entity.Property(o => o.QuotationRef).HasColumnName("QUOTATION_REF").HasMaxLength(50);
            entity.Property(o => o.SentByMethod).HasColumnName("SENT_BY_METHOD").HasMaxLength(20);
            entity.Property(o => o.Remarks).HasColumnName("REMARKS").HasMaxLength(500);
            entity.Property(o => o.AuthorisedById).HasColumnName("AUTHORISED_BY").HasMaxLength(6);
            entity.HasOne(o => o.AuthorisedBy).WithMany().HasForeignKey(o => o.AuthorisedById);
            entity.Property(o => o.FilCancelled).HasColumnName("FIL_CANCELLED").HasMaxLength(1);
            entity.Property(o => o.DateFilCancelled).HasColumnName("DATE_FIL_CANCELLED");
            entity.Property(o => o.PeriodFilCancelled).HasColumnName("PERIOD_FIL_CANCELLED");
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
            entity.Property(o => o.SuppliersDesignation).HasColumnName("SUPPLIERS_DESIGNATION").HasMaxLength(2000);
            entity.HasMany(d => d.PurchaseDeliveries).WithOne().HasForeignKey(d => new { d.OrderNumber, d.OrderLine });
            entity.Property(o => o.BaseNetTotal).HasColumnName("BASE_NET_TOTAL").HasMaxLength(18);
            entity.Property(o => o.NetTotalCurrency).HasColumnName("NET_TOTAL").HasMaxLength(18);
            entity.HasOne(o => o.Part).WithMany(p => p.PurchaseOrderDetails).HasForeignKey(o => o.PartNumber);
            entity.Property(o => o.PartNumber).HasColumnName("PART_NUMBER");
            entity.Property(o => o.StockPoolCode).HasColumnName("STOCK_POOL_CODE");
            entity.Property(o => o.OriginalOrderNumber).HasColumnName("ORIGINAL_ORDER_NUMBER").HasMaxLength(8);
            entity.Property(o => o.OriginalOrderLine).HasColumnName("ORIGINAL_ORDER_LINE").HasMaxLength(6);
            entity.Property(o => o.OurUnitOfMeasure).HasColumnName("OUR_UNIT_OF_MEASURE").HasMaxLength(14);
            entity.Property(o => o.OrderUnitOfMeasure).HasColumnName("ORDER_UNIT_OF_MEASURE").HasMaxLength(14);
            entity.Property(o => o.Duty).HasColumnName("DUTY_PERCENT").HasMaxLength(8);
            entity.Property(o => o.OrderUnitPriceCurrency).HasColumnName("NEXT_ORDER_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.BaseOrderUnitPrice).HasColumnName("BASE_ORDER_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.BaseOurUnitPrice).HasColumnName("BASE_OUR_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.OurUnitPriceCurrency).HasColumnName("NEXT_OUR_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.VatTotalCurrency).HasColumnName("VAT_TOTAL").HasMaxLength(18);
            entity.Property(o => o.BaseVatTotal).HasColumnName("BASE_VAT_TOTAL").HasMaxLength(18);
            entity.Property(o => o.DetailTotalCurrency).HasColumnName("DETAIL_TOTAL").HasMaxLength(18);
            entity.Property(o => o.BaseDetailTotal).HasColumnName("BASE_DETAIL_TOTAL").HasMaxLength(18);
            entity.Property(o => o.DeliveryInstructions).HasColumnName("DELIVERY_INSTRUCTIONS").HasMaxLength(200);
            entity.Property(o => o.DeliveryConfirmedById).HasColumnName("DELIVERY_CONFIRMED_BY").HasMaxLength(6);
            entity.HasOne(o => o.DeliveryConfirmedBy).WithMany().HasForeignKey(o => o.DeliveryConfirmedById);
            entity.Property(o => o.InternalComments).HasColumnName("INTERNAL_COMMENTS").HasMaxLength(300);
            entity.HasMany(d => d.CancelledDetails).WithOne().HasForeignKey(cd => new { cd.OrderNumber, cd.LineNumber });
            entity.HasMany(d => d.MrOrders).WithOne().HasForeignKey(mr => new { mr.OrderNumber, mr.LineNumber });
            entity.HasMany(d => d.OrderPostings).WithOne().HasForeignKey(p => new { p.OrderNumber, p.LineNumber });
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
            entity.Property(o => o.NetTotalCurrency).HasColumnName("NET_TOTAL").HasMaxLength(18);
            entity.Property(d => d.QuantityOutstanding).HasColumnName("QTY_OUTSTANDING");
            entity.Property(d => d.CallOffDate).HasColumnName("CALL_OFF_DATE");
            entity.Property(d => d.SupplierConfirmationComment).HasColumnName("SUPPLIER_CONFIRMATION_COMMENT").HasMaxLength(2000);
            entity.Property(o => o.BaseOurUnitPrice).HasColumnName("BASE_OUR_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.BaseOrderUnitPrice).HasColumnName("BASE_ORDER_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.BaseNetTotal).HasColumnName("BASE_NET_TOTAL").HasMaxLength(18);
            entity.Property(o => o.BaseVatTotal).HasColumnName("BASE_VAT_TOTAL").HasMaxLength(18);
            entity.Property(o => o.BaseDeliveryTotal).HasColumnName("BASE_DELIVERY_TOTAL").HasMaxLength(18);
            entity.Property(o => o.OurUnitPriceCurrency).HasColumnName("OUR_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.OrderUnitPriceCurrency).HasColumnName("ORDER_UNIT_PRICE").HasMaxLength(19);
            entity.Property(o => o.VatTotalCurrency).HasColumnName("VAT_TOTAL").HasMaxLength(18);
            entity.Property(o => o.DeliveryTotalCurrency).HasColumnName("DELIVERY_TOTAL").HasMaxLength(18);
        }

        private void BuildOverbookAllowedBy(ModelBuilder builder)
        {
            var entity = builder.Entity<OverbookAllowedByLog>().ToTable("PL_OVERBOOK_ALLOWED_BY");
            entity.HasKey(a => a.Id);
            entity.Property(o => o.Id).HasColumnName("ID").HasMaxLength(8);
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.OverbookGrantedBy).HasColumnName("OVERBOOK_GRANTED_BY").HasMaxLength(6);
            entity.Property(o => o.OverbookDate).HasColumnName("OVERBOOK_DATE");
            entity.Property(o => o.OverbookQty).HasColumnName("OVERBOOK_QTY").HasMaxLength(14);
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
            entity.HasOne(e => e.FullAddress).WithMany().HasForeignKey(e => e.AddressId);
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
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("VM_ID").HasMaxLength(1);
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
            entity.Property(e => e.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderLine).HasColumnName("ORDER_LINE");
            entity.HasOne(x => x.Supplier).WithMany().HasForeignKey(z => z.SupplierId);
        }

        private void BuildUnacknowledgedOrders(ModelBuilder builder)
        {
            var entity = builder.Entity<UnacknowledgedOrders>().ToTable("UNACKNOWLEDGED_ORDERS_VIEW").HasNoKey();
            entity.Property(e => e.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderLine).HasColumnName("ORDER_LINE");
            entity.Property(e => e.DeliveryNumber).HasColumnName("DELIVERY_SEQ");
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(e => e.CallOffDate).HasColumnName("CALL_OFF_DATE");
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.Property(e => e.SupplierName).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(e => e.SuppliersDesignation).HasColumnName("SUPPLIERS_DESIGNATION").HasMaxLength(2000);
            entity.Property(e => e.OrganisationId).HasColumnName("SUPPLIER_ORGANISATION_ID");
            entity.Property(e => e.OrderDeliveryQuantity).HasColumnName("ORDER_DELIVERY_QTY");
            entity.Property(e => e.OurDeliveryQuantity).HasColumnName("OUR_DELIVERY_QTY");
            entity.Property(e => e.OrderUnitPrice).HasColumnName("ORDER_UNIT_PRICE");
            entity.Property(e => e.RequestedDate).HasColumnName("REQUESTED_DATE");
            entity.Property(e => e.SupplierGroupId).HasColumnName("SUPPLIER_GROUP_ID");
            entity.Property(e => e.SupplierGroupName).HasColumnName("SUPPLIER_GROUP_NAME");
            entity.Property(e => e.CurrencyCode).HasColumnName("CURRENCY_CODE").HasMaxLength(4);
        }

        private void BuildUnacknowledgedOrderSuppliers(ModelBuilder builder)
        {
            var entity = builder.Entity<SuppliersWithUnacknowledgedOrders>().ToTable("UNACKNOWLEDGED_ORDER_SUPPLIERS")
                .HasNoKey();
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.Property(e => e.SupplierName).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(e => e.SupplierGroupId).HasColumnName("SUPPLIER_GROUP_ID");
            entity.Property(e => e.VendorManager).HasColumnName("VENDOR_MANAGER").HasMaxLength(1);
            entity.Property(e => e.Planner).HasColumnName("PLANNER");
            entity.Property(e => e.SupplierGroupName).HasColumnName("SUPPLIER_GROUP_NAME").HasMaxLength(50);
        }

        private void BuildUnacknowledgedOrderSupplierGroups(ModelBuilder builder)
        {
            var entity = builder.Entity<SupplierGroupsWithUnacknowledgedOrders>().ToTable("UNACK_ORDER_SUPPLIER_GROUPS")
                .HasNoKey();
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(50);
            entity.Property(e => e.SupplierGroupId).HasColumnName("SUPPLIER_GROUP_ID");
            entity.Property(e => e.VendorManager).HasColumnName("VENDOR_MANAGER").HasMaxLength(1);
            entity.Property(e => e.Planner).HasColumnName("PLANNER");
            entity.Property(e => e.SupplierGroupName).HasColumnName("SUPPLIER_GROUP_NAME").HasMaxLength(50);
        }

        private void BuildSupplierGroups(ModelBuilder builder)
        {
            var entity = builder.Entity<SupplierGroup>().ToTable("SUPPLIER_GROUPS");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
        }

        private void BuildPlCreditDebitNotes(ModelBuilder builder)
        {
            var entity = builder.Entity<PlCreditDebitNote>().ToTable("PL_CREDIT_DEBIT_NOTES");
            entity.HasKey(a => a.NoteNumber);
            entity.Property(a => a.NoteNumber).HasColumnName("CDNOTE_ID");
            entity.Property(a => a.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(a => a.OrderQty).HasColumnName("ORDER_QTY");
            entity.Property(a => a.ClosedBy).HasColumnName("CLOSED_BY");
            entity.Property(a => a.DateClosed).HasColumnName("DATE_CLOSED");
            entity.Property(a => a.NetTotal).HasColumnName("NET_TOTAL");
            entity.Property(a => a.ReturnsOrderNumber).HasColumnName("RETURNS_ORDER_NUMBER");
            entity.Property(a => a.Notes).HasColumnName("NOTES").HasMaxLength(200);
            entity.Property(a => a.ReasonClosed).HasColumnName("REASON_CLOSED").HasMaxLength(2000);
            entity.HasOne(a => a.Supplier).WithMany().HasForeignKey("SUPPLIER_ID");
            entity.Property(a => a.DateCreated).HasColumnName("DATE_CREATED");
            entity.Property(a => a.Total).HasColumnName("TOTAL_INC_VAT");
            entity.Property(a => a.OrderUnitPrice).HasColumnName("ORDER_UNIT_PRICE");
            entity.Property(a => a.OrderUnitOfMeasure).HasColumnName("ORDER_UNIT_OF_MEASURE");
            entity.Property(a => a.VatTotal).HasColumnName("VAT_TOTAL");
            entity.Property(a => a.SuppliersDesignation).HasColumnName("SUPPLIERS_DESIGNATION");
            entity.HasOne(a => a.PurchaseOrder).WithMany().HasForeignKey("ORIGINAL_ORDER_NUMBER");
            entity.HasOne(a => a.Currency).WithMany().HasForeignKey("CURRENCY");
            entity.Property(a => a.ReturnsOrderLine).HasColumnName("RETURNS_ORDER_LINE");
            entity.Property(a => a.VatRate).HasColumnName("VAT_RATE");
            entity.Property(a => a.CancelledBy).HasColumnName("CANCELLED_BY");
            entity.Property(a => a.DateCancelled).HasColumnName("DATE_CANCELLED");
            entity.Property(a => a.ReasonCancelled).HasColumnName("REASON_CANCELLED");
            entity.HasOne(a => a.NoteType).WithMany().HasForeignKey("CDNOTE_TYPE");
        }

        private void BuildCreditDebitNoteTypes(ModelBuilder builder)
        {
            var entity = builder.Entity<CreditDebitNoteType>().ToTable("CDNOTES_TYPES");
            entity.HasKey(a => a.Type);
            entity.Property(a => a.Type).HasColumnName("CDNOTE_TYPE");
            entity.Property(a => a.Description).HasColumnName("DESCRIPTION");
            entity.Property(a => a.PrintDescription).HasColumnName("PRINT_DESCRIPTION");
        }

        private void BuildTqmsJobRefs(ModelBuilder builder)
        {
            var entity = builder.Entity<TqmsJobref>().ToTable("TQMS_JOBREFS");
            entity.HasKey(a => a.Jobref);
            entity.Property(a => a.Jobref).HasColumnName("JOBREF");
            entity.Property(a => a.Date).HasColumnName("JOBREF_DATE");
        }

        private void BuildPartsReceivedView(ModelBuilder builder)
        {
            var entity = builder.Entity<PartReceivedRecord>().ToTable("PARTS_RECEIVED_VIEW").HasNoKey();
            entity.Property(a => a.PartNumber).HasColumnName("PART_NUMBER");
            entity.Property(a => a.JobRef).HasColumnName("JOBREF").HasColumnType("VARCHAR2");
            entity.Property(a => a.TqmsGroup).HasColumnName("TQMS_GROUP").HasColumnType("VARCHAR2");
            entity.Property(a => a.OverstockQty).HasColumnName("OVERSTOCK_QTY");
            entity.Property(a => a.OverStockValue).HasColumnName("OVERSTOCK_VALUE");
            entity.Property(a => a.OrderNumber).HasColumnName("ORDER_NUMBER").HasColumnType("VARCHAR2");
            entity.Property(a => a.SupplierId).HasColumnName("SUPP_SUPPLIER_ID");
            entity.Property(a => a.DateBooked).HasColumnName("DATE_BOOKED");
            entity.Property(a => a.MaterialPrice).HasColumnName("MATERIAL_PRICE");
            entity.Property(a => a.PartPrice).HasColumnName("PART_PRICE");
            entity.Property(a => a.SupplierName).HasColumnName("SUPPLIER_NAME").HasColumnType("VARCHAR2");
            entity.Property(a => a.PartNumber).HasColumnName("PART_NUMBER").HasColumnType("VARCHAR2");
            entity.Property(a => a.Qty).HasColumnName("QTY");
        }

        private void BuildPurchaseOrderReqs(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrderReq>().ToTable("BLUE_REQS");
            entity.HasKey(e => e.ReqNumber);
            entity.Property(e => e.ReqNumber).HasColumnName("BLUE_REQ_NUMBER");
            entity.Property(e => e.State).HasColumnName("BR_STATE").HasMaxLength(20);
            entity.Property(e => e.ReqDate).HasColumnName("REQ_DATE");
            entity.Property(e => e.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.Qty).HasColumnName("QTY").HasMaxLength(19);
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION").HasMaxLength(2000);
            entity.Property(e => e.UnitPrice).HasColumnName("UNIT_PRICE").HasMaxLength(19);
            entity.Property(e => e.Carriage).HasColumnName("CARRIAGE").HasMaxLength(18);
            entity.Property(e => e.TotalReqPrice).HasColumnName("TOTAL_REQ_PRICE").HasMaxLength(18);
            entity.Property(e => e.CurrencyCode).HasColumnName("CURRENCY").HasMaxLength(4);
            entity.HasOne(e => e.Currency).WithMany().HasForeignKey(x => x.CurrencyCode);
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID").HasMaxLength(6);
            entity.HasOne(e => e.Supplier).WithMany().HasForeignKey(x => x.SupplierId);
            entity.Property(e => e.SupplierName).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(e => e.SupplierContact).HasColumnName("SUPPLIER_CONTACT").HasMaxLength(50);
            entity.Property(e => e.AddressLine1).HasColumnName("ADDRESS_1").HasMaxLength(40);
            entity.Property(e => e.AddressLine2).HasColumnName("ADDRESS_2").HasMaxLength(40);
            entity.Property(e => e.AddressLine3).HasColumnName("ADDRESS_3").HasMaxLength(40);
            entity.Property(e => e.AddressLine4).HasColumnName("ADDRESS_4").HasMaxLength(40);
            entity.Property(e => e.PostCode).HasColumnName("POSTAL_CODE").HasMaxLength(20);
            entity.Property(e => e.CountryCode).HasColumnName("COUNTRY_CODE").HasMaxLength(2);
            entity.HasOne(e => e.Country).WithMany().HasForeignKey(x => x.CountryCode);
            entity.Property(e => e.PhoneNumber).HasColumnName("PHONE_NUMBER").HasMaxLength(40);
            entity.Property(e => e.QuoteRef).HasColumnName("QUOTE_REF").HasMaxLength(200);
            entity.Property(e => e.DateRequired).HasColumnName("DATE_REQUIRED");
            entity.Property(e => e.RequestedById).HasColumnName("REQUESTED_BY").HasMaxLength(6);
            entity.HasOne(e => e.RequestedBy).WithMany().HasForeignKey(x => x.RequestedById);
            entity.Property(e => e.AuthorisedById).HasColumnName("AUTHORISED_BY").HasMaxLength(6);
            entity.HasOne(e => e.AuthorisedBy).WithMany().HasForeignKey(x => x.AuthorisedById);
            entity.Property(e => e.RemarksForOrder).HasColumnName("REMARKS_FOR_ORDER").HasMaxLength(200);
            entity.Property(e => e.DepartmentCode).HasColumnName("DEPARTMENT").HasMaxLength(10);
            entity.HasOne(e => e.Department).WithMany().HasForeignKey(x => x.DepartmentCode);
            entity.Property(e => e.NominalCode).HasColumnName("NOMINAL").HasMaxLength(10);
            entity.HasOne(e => e.Nominal).WithMany().HasForeignKey(x => x.NominalCode);
            entity.Property(e => e.TurnedIntoOrderById).HasColumnName("TURNED_INTO_ORDER_BY").HasMaxLength(6);
            entity.HasOne(e => e.TurnedIntoOrderBy).WithMany().HasForeignKey(x => x.TurnedIntoOrderById);
            entity.Property(e => e.FinanceCheckById).HasColumnName("FINANCE_CHECKED_BY").HasMaxLength(6);
            entity.HasOne(e => e.FinanceCheckBy).WithMany().HasForeignKey(x => x.FinanceCheckById);
            entity.Property(e => e.SecondAuthById).HasColumnName("SECONDARY_AUTH_BY").HasMaxLength(6);
            entity.HasOne(e => e.SecondAuthBy).WithMany().HasForeignKey(x => x.SecondAuthById);
            entity.Property(e => e.Email).HasColumnName("EMAIL_ADDRESS").HasMaxLength(50);
            entity.Property(e => e.InternalNotes).HasColumnName("INTERNAL_ONLY_ORDER_NOTES").HasMaxLength(300);
            entity.HasOne(e => e.ReqState).WithMany().HasForeignKey(e => e.State);
        }

        private void BuildNominals(ModelBuilder builder)
        {
            builder.Entity<Nominal>().ToTable("LINN_NOMINALS");
            builder.Entity<Nominal>().HasKey(n => n.NominalCode);
            builder.Entity<Nominal>().Property(n => n.NominalCode).HasColumnName("NOMINAL_CODE");
            builder.Entity<Nominal>().Property(n => n.Description).HasColumnName("DESCRIPTION");
        }

        private void BuildDepartments(ModelBuilder builder)
        {
            var e = builder.Entity<Department>().ToTable("LINN_DEPARTMENTS");
            e.HasKey(d => d.DepartmentCode);
            e.Property(d => d.DepartmentCode).HasColumnName("DEPARTMENT_CODE").HasMaxLength(10);
            e.Property(d => d.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        }

        private void BuildPurchaseOrderReqStates(ModelBuilder builder)
        {
            var e = builder.Entity<PurchaseOrderReqState>().ToTable("BLUE_REQ_STATES");
            e.HasKey(d => d.State);
            e.Property(d => d.State).HasColumnName("BR_STATE").HasMaxLength(20);
            e.Property(d => d.Description).HasColumnName("DESCRIPTION").HasMaxLength(200);
            e.Property(d => d.DisplayOrder).HasColumnName("DISPLAY_ORDER");
            e.Property(d => d.IsFinalState).HasColumnName("FINAL_STATE").HasMaxLength(1);
        }

        private void BuildPurchaseOrderReqStateChanges(ModelBuilder builder)
        {
            var e = builder.Entity<PurchaseOrderReqStateChange>().ToTable("BLUE_REQ_STATE_CHANGES");
            e.HasKey(s => new { s.FromState, s.ToState });
            e.Property(s => s.FromState).HasColumnName("FROM_STATE").HasMaxLength(20);
            e.Property(s => s.ToState).HasColumnName("TO_STATE").HasMaxLength(20);
            e.Property(s => s.UserAllowed).HasColumnName("USER_ALLOWED").HasMaxLength(1);
            e.Property(s => s.ComputerAllowed).HasColumnName("COMPUTER_STANDARD").HasMaxLength(1);
        }

        private void BuildMrRunLogs(ModelBuilder builder)
        {
            var e = builder.Entity<MrpRunLog>().ToTable("MR_RUNLOG");
            e.HasKey(d => d.MrRunLogId);
            e.Property(d => d.MrRunLogId).HasColumnName("MR_RUNLOG_ID");
            e.Property(d => d.JobRef).HasColumnName("JOBREF").HasMaxLength(6);
            e.Property(d => d.BuildPlanName).HasColumnName("BUILD_PLAN_NAME").HasMaxLength(10);
            e.Property(d => d.RunDate).HasColumnName("RUNDATE");
            e.Property(d => d.RunDetails).HasColumnName("RUN_DETAILS").HasMaxLength(2000);
            e.Property(d => d.FullRun).HasColumnName("FULL_RUN").HasMaxLength(1);
            e.Property(d => d.Kill).HasColumnName("KILL").HasMaxLength(1);
            e.Property(d => d.Success).HasColumnName("SUCCESS").HasMaxLength(1);
            e.Property(d => d.LoadMessage).HasColumnName("LOAD_MESSAGE").HasMaxLength(2000);
            e.Property(d => d.MrMessage).HasColumnName("MR_MESSAGE").HasMaxLength(2000);
            e.Property(d => d.DateTidied).HasColumnName("DATE_TIDIED");
        }

        private void BuildWhatsInInspectionExcludingFailedView(ModelBuilder builder)
        {
            var e = builder.Entity<PartsInInspectionExcludingFails>().ToView("WHATS_IN_INSP_EXCL_FAIL_VIEW");
            e.HasNoKey();
            e.Property(m => m.PartNumber).HasColumnName("PART_NUMBER");
            e.Property(m => m.Description).HasColumnName("DESCRIPTION");
            e.Property(m => m.OurUnitOfMeasure).HasColumnName("OUR_UNIT_OF_MEASURE");
            e.Property(m => m.QtyInStock).HasColumnName("QTY_IN_STOCK");
            e.Property(m => m.QtyInInspection).HasColumnName("QTY_IN_INSP");
            e.Property(m => m.MinDate).HasColumnName("MINDATE");
            e.Property(m => m.RawOrFinished).HasColumnName("RM_FG");
        }

        private void BuildWhatsInInspectionIncludingFailedView(ModelBuilder builder)
        {
            var e = builder.Entity<PartsInInspectionIncludingFails>().ToView("WHATS_IN_INSP_INCL_FAIL_VIEW");
            e.HasNoKey();
            e.Property(m => m.PartNumber).HasColumnName("PART_NUMBER");
            e.Property(m => m.Description).HasColumnName("DESCRIPTION");
            e.Property(m => m.OurUnitOfMeasure).HasColumnName("OUR_UNIT_OF_MEASURE");
            e.Property(m => m.QtyInStock).HasColumnName("QTY_IN_STOCK");
            e.Property(m => m.QtyInInspection).HasColumnName("QTY_IN_INSP");
            e.Property(m => m.MinDate).HasColumnName("MINDATE");
            e.Property(m => m.RawOrFinished).HasColumnName("RM_FG");
        }

        private void BuildWhatsInInspectionPurchaseOrdersView(ModelBuilder builder)
        {
            var e = builder.Entity<WhatsInInspectionPurchaseOrdersData>().ToView("WHATS_IN_INSP_PO_VIEW");
            e.HasNoKey();
            e.Property(m => m.PartNumber).HasColumnName("PART_NUMBER").HasColumnType("VARCHAR2");
            e.Property(m => m.State).HasColumnName("STATE").HasColumnType("VARCHAR2");
            e.Property(m => m.OrderType).HasColumnName("ORDER_TYPE").HasColumnType("VARCHAR2");
            e.Property(m => m.OrderNumber).HasColumnName("ORDER_NUMBER");
            e.Property(m => m.Qty).HasColumnName("QTY");
            e.Property(m => m.Cancelled).HasColumnName("CANCELLED").HasColumnType("VARCHAR2");
            e.Property(m => m.QtyPassed).HasColumnName("PASSED");
            e.Property(m => m.QtyReceived).HasColumnName("RECEIVED");
            e.Property(m => m.QtyReturned).HasColumnName("RETURNED");
        }

        private void BuildWhatsInInspectionStockLocationsView(ModelBuilder builder)
        {
            var e = builder.Entity<WhatsInInspectionStockLocationsData>().ToView("WHATS_IN_INSP_ST_LOC_VIEW");
            e.HasNoKey();
            e.Property(m => m.PartNumber).HasColumnName("PART_NUMBER");
            e.Property(m => m.State).HasColumnName("STATE");
            e.Property(m => m.Batch).HasColumnName("BATCH");
            e.Property(m => m.Qty).HasColumnName("QTY");
            e.Property(m => m.Location).HasColumnName("LOC");
            e.Property(m => m.BatchRef).HasColumnName("BATCH_REF");
            e.Property(m => m.StockRotationDate).HasColumnName("STOCK_ROTATION_DATE");
        }

        private void BuildWhatsInInspectionBackOrderView(ModelBuilder builder)
        {
            var e = builder.Entity<WhatsInInspectionBackOrderData>().ToView("WHATS_IN_INSP_BACK_ORDER_VIEW");
            e.HasNoKey();
            e.Property(m => m.ArticleNumber).HasColumnName("ARTICLE_NUMBER");
            e.Property(m => m.Story).HasColumnName("STORY");
            e.Property(m => m.QtyInInspection).HasColumnName("QTY_IN_INSPECTION");
            e.Property(m => m.QtyNeeded).HasColumnName("QTY_NEEDED");
        }

        private void BuildDocumentTypes(ModelBuilder builder)
        {
            var entity = builder.Entity<DocumentType>().ToTable("DOCUMENT_TYPES");
            entity.HasKey(d => d.Name);
            entity.Property(d => d.Name).HasColumnName("NAME").HasMaxLength(6);
            entity.Property(d => d.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        }

        private void BuildMrOrders(ModelBuilder builder)
        {
            var entity = builder.Entity<MrOrder>().ToTable("MR_ORDERS");
            entity.HasKey(e => new { e.OrderNumber, e.JobRef });
            entity.Property(d => d.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(d => d.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(d => d.JobRef).HasColumnName("JOBREF").HasMaxLength(6);
            entity.Property(d => d.LineNumber).HasColumnName("ORDER_LINE").HasMaxLength(6);
        }

        private void BuildPrefsupVsReceiptsView(ModelBuilder builder)
        {
            var entity = builder.Entity<ReceiptPrefSupDiff>().ToTable("PREFSUP_VS_RECEIPTS_VIEW");
            entity.HasKey(m => m.PlReceiptId);
            entity.Property(e => e.PlReceiptId).HasColumnName("PLREC_ID");
            entity.Property(e => e.CurrencyUnitPrice).HasColumnName("CURRENCY_UNIT_PRICE");
            entity.Property(e => e.Qty).HasColumnName("QTY");
            entity.Property(e => e.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(e => e.Difference).HasColumnName("DIFF");
            entity.Property(e => e.PrefsupCurrencyUnitPrice).HasColumnName("PREF_SUP_CURRENCY_UNIT_PRICE");
            entity.Property(e => e.PrefsupBaseUnitPrice).HasColumnName("PS_BASE_UNIT_PRICE");
            entity.Property(e => e.ReceiptBaseUnitPrice).HasColumnName("RECEIPT_BASE_UNIT_PRICE");
            entity.Property(e => e.DateBooked).HasColumnName("DATE_BOOKED");
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.Property(e => e.SupplierName).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(e => e.PreferredSupplier).HasColumnName("PREFERRED_SUPPLIER").HasMaxLength(1);
            entity.Property(e => e.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(e => e.OrderLine).HasColumnName("ORDER_LINE");
            entity.Property(e => e.OrderCurrency).HasColumnName("ORDER_CURRENCY").HasMaxLength(4);
            entity.Property(e => e.PrefsupCurrency).HasColumnName("ORDER_CURRENCY").HasMaxLength(4);
            entity.Property(e => e.MPVReason).HasColumnName("MPV_REASON").HasMaxLength(20);
        }

        private void BuildMrMaster(ModelBuilder builder)
        {
            var entity = builder.Entity<MrMaster>().ToTable("MR_MASTER").HasNoKey();
            entity.Property(e => e.JobRef).HasColumnName("JOBREF");
            entity.Property(e => e.RunDate).HasColumnName("RUNDATE");
            entity.Property(e => e.RunLogIdCurrentlyInProgress).HasColumnName("RUNLOG_ID_IN_PROGRESS");
        }

        private void BuildLedgerPeriods(ModelBuilder builder)
        {
            var entity = builder.Entity<LedgerPeriod>().ToTable("LEDGER_PERIODS");
            entity.HasKey(e => e.PeriodNumber);
            entity.Property(d => d.PeriodNumber).HasColumnName("PERIOD_NUMBER");
            entity.Property(d => d.MonthName).HasColumnName("MONTH_NAME");
        }

        private void BuildLinnWeeks(ModelBuilder builder)
        {
            var entity = builder.Entity<LinnWeek>().ToTable("LINN_WEEKS");
            entity.HasKey(e => e.WeekNumber);
            entity.Property(d => d.WeekNumber).HasColumnName("LINN_WEEK_NUMBER");
            entity.Property(d => d.StartsOn).HasColumnName("LINN_WEEK_START_DATE");
            entity.Property(d => d.EndsOn).HasColumnName("LINN_WEEK_END_DATE");
        }
    
        private void BuildCancelledPODetails(ModelBuilder builder)
        {
            var entity = builder.Entity<CancelledOrderDetail>().ToTable("PL_CANCELLED_DETAILS");
            entity.HasKey(e => e.Id);
            entity.Property(d => d.Id).HasColumnName("PLOC_ID").HasMaxLength(6);
            entity.Property(d => d.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(d => d.LineNumber).HasColumnName("ORDER_LINE").HasMaxLength(6);
            entity.Property(d => d.DeliverySequence).HasColumnName("DELIVERY_SEQ").HasMaxLength(6);
            entity.Property(d => d.DateCancelled).HasColumnName("DATE_ORDER_CANCELLED");
            entity.Property(d => d.DateFilCancelled).HasColumnName("DATE_FIL_CANCELLED");
            entity.Property(e => e.CancelledById).HasColumnName("ORDER_CANCELLED_BY").HasMaxLength(6);
            entity.HasOne(e => e.CancelledBy).WithMany().HasForeignKey(x => x.CancelledById);
            entity.Property(e => e.FilCancelledById).HasColumnName("FIL_CANCELLED_BY").HasMaxLength(6);
            entity.HasOne(e => e.FilCancelledBy).WithMany().HasForeignKey(x => x.FilCancelledById);
            entity.Property(d => d.ReasonCancelled).HasColumnName("REASON_CANCELLED").HasMaxLength(200);
            entity.Property(d => d.PeriodCancelled).HasColumnName("PERIOD_CANCELLED");
            entity.Property(d => d.PeriodFilCancelled).HasColumnName("PERIOD_CANCELLED");
            entity.Property(e => e.ValueCancelled).HasColumnName("VALUE_CANCELLED").HasMaxLength(16);
            entity.Property(e => e.BaseValueFilCancelled).HasColumnName("BASE_VALUE_FIL_CANCELLED").HasMaxLength(16);
            entity.Property(e => e.ValueFilCancelled).HasColumnName("VALUE_FIL_CANCELLED").HasMaxLength(16);
            entity.Property(e => e.DateUncancelled).HasColumnName("DATE_UNCANCELLED");
            entity.Property(e => e.DateFilUncancelled).HasColumnName("DATE_UNFIL_CANCELLED");
            entity.Property(e => e.DatePreviouslyCancelled).HasColumnName("DATE_PREVIOUSLY_CANCELLED");
            entity.Property(e => e.DatePreviouslyFilCancelled).HasColumnName("DATE_PREVIOUSLY_FIL_CANCELLED");
        }

        private void BuildPurchaseOrderPostings(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrderPosting>().ToTable("PL_ORDER_POSTINGS");
            entity.HasKey(e => e.Id);
            entity.Property(d => d.Id).HasColumnName("PLORP_ID").HasMaxLength(10);
            entity.Property(d => d.LineNumber).HasColumnName("PLORL_ORDER_LINE").HasMaxLength(6);
            entity.Property(d => d.OrderNumber).HasColumnName("PLORL_ORDER_NUMBER");
            entity.Property(d => d.Qty).HasColumnName("QTY").HasMaxLength(6);
            entity.Property(d => d.Product).HasColumnName("PRODUCT").HasMaxLength(10);
            entity.Property(d => d.Person).HasColumnName("PERSON").HasMaxLength(6);
            entity.Property(d => d.Building).HasColumnName("BUILDING").HasMaxLength(10);
            entity.Property(d => d.Vehicle).HasColumnName("VEHICLE").HasMaxLength(10);
            entity.Property(d => d.Notes).HasColumnName("NOTES").HasMaxLength(200);
            entity.Property(d => d.NominalAccountId).HasColumnName("NOMACC_ID").HasMaxLength(6);
            entity.HasOne(e => e.NominalAccount).WithMany().HasForeignKey(p => p.NominalAccountId);
        }

        private void BuildNominalAccounts(ModelBuilder builder)
        {
            var entity = builder.Entity<NominalAccount>().ToTable("NOMINAL_ACCOUNTS");
            entity.HasKey(e => e.AccountId);
            entity.Property(d => d.AccountId).HasColumnName("NOMACC_ID").HasMaxLength(6);
            entity.HasOne(e => e.Department).WithMany().HasForeignKey("DEPARTMENT");
            entity.HasOne(e => e.Nominal).WithMany().HasForeignKey("NOMINAL");
        }
    }
}
