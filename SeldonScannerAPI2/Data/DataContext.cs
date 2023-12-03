using Microsoft.EntityFrameworkCore;
using SeldonStockScannerAPI.models;

namespace SeldonStockScannerAPI.Data
{
    public class DataContext : DbContext
    {
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
    }
}
