using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.models;

namespace SeldonStockScannerAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Open teminal and check if Entity Framework is installed:
        // dotnet ef
        // Install Entity Framework:
        // dotnet tool install --global dotnet-ef
        // Add migration:
        // dotnet ef migrations add <name>


        public DbSet<Plus500Symbol> plus500Symbols { get; set; }

        public DbSet<FinvizCompanyEntity> finvizCompanyEntitys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=seldon;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }
    }
}
