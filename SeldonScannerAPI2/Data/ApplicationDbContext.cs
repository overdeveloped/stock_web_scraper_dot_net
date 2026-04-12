using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SeldonStockScannerAPI.Models;

namespace SeldonStockScannerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<FinvizCompanyEntity> FinvizCompanye { get; set; }
        public DbSet<WatchListEntity> WatchList { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }


        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        //{
        //    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

        //}
    }
}
