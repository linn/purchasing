namespace Linn.Purchasing.Persistence.LinnApps
{
    using Linn.Common.Configuration;
    using Linn.Purchasing.Domain.LinnApps;
    using Linn.Purchasing.Domain.LinnApps.Parts;
    using Linn.Purchasing.Domain.LinnApps.PartSuppliers;
    using Linn.Purchasing.Domain.LinnApps.Suppliers;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ServiceDbContext : DbContext
    {
        public static readonly LoggerFactory MyLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public DbSet<Thing> Things { get; set; }

        public DbSet<PartSupplier> PartSuppliers { get; set; }

        public DbSet<Part> Parts { get; set; }

        public DbSet<Part> Suppliers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<PackagingGroup> PackagingGroups { get; set; }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Model.AddAnnotation("MaxIdentifierLength", 30);
            this.BuildThings(builder);
            this.BuildThingDetails(builder);
            this.BuildThingCodes(builder);
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

        private void BuildThings(ModelBuilder builder)
        {
            var entity = builder.Entity<Thing>().ToTable("THINGS");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).HasColumnName("ID");
            entity.Property(a => a.Name).HasColumnName("NAME");
            entity.Property(a => a.CodeId).HasColumnName("THING_CODE");
            entity.Property(a => a.RecipientAddress).HasColumnName("RECIPIENT_ADDRESS");
            entity.Property(a => a.RecipientName).HasColumnName("RECIPIENT_NAME");
            entity.HasOne(d => d.Code).WithMany(p => p.Things).HasForeignKey(d => d.CodeId);
            entity.HasMany(a => a.Details).WithOne();
        }

        private void BuildThingDetails(ModelBuilder builder)
        {
            var h = builder.Entity<ThingDetail>().ToTable("THING_DETAILS");
            h.HasKey(a => new { a.ThingId, a.DetailId });
            h.Property(a => a.ThingId).HasColumnName("THING_ID");
            h.Property(a => a.DetailId).HasColumnName("DETAIL_ID");
            h.Property(a => a.Description).HasColumnName("DESCRIPTION");
        }

        private void BuildThingCodes(ModelBuilder builder)
        {
            var h = builder.Entity<ThingCode>().ToTable("THING_CODES");
            h.HasKey(a => a.Code);
            h.Property(a => a.Code).HasColumnName("CODE");
            h.Property(a => a.CodeName).HasColumnName("CODE_NAME");
        }

        private void BuildPartSuppliers(ModelBuilder builder)
        {
            var entity = builder.Entity<PartSupplier>().ToTable("PART_SUPPLIERS");
            entity.HasKey(e => new { e.PartNumber, e.SupplierId });
            entity.Property(e => e.ReelOrBoxQty).HasColumnName("REEL_OR_BOX_QTY");
            entity.Property(e => e.RohsCompliant).HasColumnName("ROHS_COMPLIANT").HasMaxLength(1);
            entity.Property(e => e.RohsCategory).HasColumnName("ROHS_CATEGORY").HasMaxLength(6);
            entity.Property(e => e.RohsComments).HasColumnName("ROHS_COMMENTS").HasMaxLength(160);
            entity.HasOne(ps => ps.PackagingGroup).WithMany().HasForeignKey("PAGRP_ID");
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
            entity.Property(e => e.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.HasOne(e => e.OrderMethod).WithMany().HasForeignKey("PL_ORDER_METHOD");
            entity.Property(e => e.Currency).HasColumnName("CURR_CODE").HasMaxLength(4);
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
        }

        private void BuildParts(ModelBuilder builder)
        {
            var entity = builder.Entity<Part>().ToTable("PARTS");
            entity.HasKey(a => a.PartNumber);
            entity.Property(a => a.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(a => a.Description).HasColumnName("DESCRIPTION").HasMaxLength(50);
        }

        private void BuildSuppliers(ModelBuilder builder)
        {
            var entity = builder.Entity<Supplier>().ToTable("SUPPLIERS");
            entity.HasKey(a => a.SupplierId);
            entity.Property(a => a.SupplierId).HasColumnName("SUPPLIER_ID");
            entity.Property(a => a.Name).HasColumnName("SUPPLIER_NAME").HasMaxLength(50);
            entity.Property(a => a.LedgerStream).HasColumnName("LEDGER_STREAM");
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
            var entity = builder.Entity<Employee>().ToTable("EMPLOYEES");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("EMPLOYEE_NUMBER");
            entity.Property(e => e.FullName).HasColumnName("NAME").HasMaxLength(4000);
        }

        private void BuildAddresses(ModelBuilder builder)
        {
            var entity = builder.Entity<Address>().ToTable("ADDRESSES");
            entity.HasKey(m => m.Id);
            entity.Property(e => e.Id).HasColumnName("ADDRESS_ID");
            entity.Property(a => a.FullAddress).HasColumnName("ADDRESS");
        }

        private void BuildPurchaseOrders(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrder>().ToTable("PL_ORDERS");
            entity.HasKey(o => o.OrderNumber);
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.SupplierId).HasColumnName("SUPP_SUPPLIER_ID");
            entity.HasOne(o => o.Supplier).WithMany().HasForeignKey(o => o.SupplierId);
            entity.Property(o => o.DocumentType).HasColumnName("DOCUMENT_TYPE");

        }

        private void BuildPurchaseOrderDetails(ModelBuilder builder)
        {
            var entity = builder.Entity<PurchaseOrderDetail>().ToTable("PL_ORDER_DETAILS");
            entity.HasKey(a => new { a.OrderNumber, a.Line });
            entity.Property(o => o.OrderNumber).HasColumnName("ORDER_NUMBER");
            entity.Property(o => o.Line).HasColumnName("ORDER_LINE");
            entity.Property(o => o.RohsCompliant).HasColumnName("ROHS_COMPLIANT");
            entity.Property(o => o.OurQty).HasColumnName("OUR_QTY");
            entity.Property(o => o.PartNumber).HasColumnName("PART_NUMBER").HasMaxLength(14);
            entity.Property(o => o.SuppliersDesignation).HasColumnName("SUPPLIERS_DESIGNATION").HasMaxLength(2000);
            //entity.HasOne(a => a.SalesArticle).WithMany(x => x.PurchaseOrderDetails).HasForeignKey(z => z.PartNumber);
            entity.HasOne(d => d.PurchaseOrder).WithMany(o => o.Details)
                .HasForeignKey(d => d.OrderNumber);
            entity.Property(o => o.NetTotal).HasColumnName("NET_TOTAL").HasMaxLength(18);
        }
    }
}
