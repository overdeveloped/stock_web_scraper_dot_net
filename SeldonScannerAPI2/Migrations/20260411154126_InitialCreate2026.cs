using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeldonStockScannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinvizCompanyEntities",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MarketCap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Price = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Change = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Volume = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinvizCompanyEntities", x => x.Ticker);
                });

            migrationBuilder.CreateTable(
                name: "WatchListEntity",
                columns: table => new
                {
                    Ticker = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListEntity", x => x.Ticker);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinvizCompanyEntities");

            migrationBuilder.DropTable(
                name: "WatchListEntity");
        }
    }
}
