using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.Finviz_Company;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FinvizCompanyEntity> FinvizCompany { get; set; }
        public DbSet<WatchListEntity> WatchList { get; set; }

        // DON'T USE THIS. IT IS NOT DEPENDNACY INJECTION
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        //}


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WatchListEntity>()
                .HasKey(w => w.Id);

            modelBuilder.Entity<FinvizCompanyEntity>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<WatchListEntity>()
                .HasMany(w => w.Companies)
                .WithMany(c => c.Watchlists)
                .UsingEntity(j => j.ToTable("WatchListCompanies"));
        }
    }
}
