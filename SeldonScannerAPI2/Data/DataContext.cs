using Microsoft.EntityFrameworkCore;
using StockScannerCommonCode.model;

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

        public DbSet<SuperHero> SuperHeroes { get; set; }

        public DbSet<Plus500Symbol> plus500_symbols { get; set; }
    }
}
