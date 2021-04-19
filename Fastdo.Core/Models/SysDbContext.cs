using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fastdo.Core.Models
{
    public partial class SysDbContext : IdentityDbContext<AppUser>
    {
        public SysDbContext()
        {
        }

        public SysDbContext(DbContextOptions<SysDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<LzDrug> LzDrugs { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StkDrug> StkDrugs { get; set; }
        public virtual DbSet<PharmacyInStock> PharmaciesInStocks { get; set; }
        public virtual DbSet<Complain> Complains { get; set; }
        public virtual DbSet<LzDrugRequest> LzDrugRequests { get; set; }
        public virtual DbSet<StkDrugInStkDrgPackageReq> StkDrugInStkDrgPackagesRequests { get; set; }
        public virtual DbSet<StkDrugPackageRequest> StkDrugPackagesRequests { get; set; }
        public virtual DbSet<StockInStkDrgPackageReq> StockInStkDrgPackageReqs { get; set; }
        public virtual DbSet<AdminHistory> AdminHistoryEntries { get; set; }

        public virtual DbSet<PharmacyInStockClass> PharmaciesInStockClasses { get; set; }
        public virtual DbSet<StockWithPharmaClass> StocksWithPharmaClasses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);         
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<Admin>()
                .HasOne(a => a.SuperAdmin)
                .WithMany(a=>a.SubAdmins)
                .HasForeignKey("SuperAdminId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PharmacyInStock>()
                .HasKey(t => new { t.PharmacyId, t.StockId });

            builder.Entity<PharmacyInStock>()
                .HasOne(t => t.Pharmacy)
                .WithMany(p => p.GoinedStocks)
                .HasForeignKey(t => t.PharmacyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PharmacyInStock>()
                .HasOne(t => t.Stock)
                .WithMany(p => p.GoinedPharmacies)
                .HasForeignKey(t => t.StockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<LzDrugRequest>()
                 .HasIndex(i => new { i.LzDrugId, i.PharmacyId })
                 .IsUnique();
            builder.Entity<StockWithPharmaClass>()
                .HasIndex(i => new { i.StockId, i.ClassName }).IsUnique();

            builder.Entity<StkDrugInStkDrgPackageReq>()
                 .HasIndex(i => new { i.StkDrugId, i.StkPackageId })
                 .IsUnique();
 

            builder.Entity<PharmacyInStockClass>()
               .HasIndex(t => new { t.PharmacyId, t.StockClassId }).IsUnique();

            builder.Entity<StockInStkDrgPackageReq>()
                .HasIndex(t => new { t.StockId, t.PackageId }).IsUnique();
            builder.Entity<StockInStkDrgPackageReq>()
                .HasOne(e => e.Package)
                .WithMany(e => e.AssignedStocks)
                .HasForeignKey(e=>e.PackageId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<StockInStkDrgPackageReq>()
                .HasOne(e => e.Stock)
                .WithMany(e => e.RequestedPackages)
                .HasForeignKey(e=>e.StockId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    //*844# *319#  0333  
    }
}
