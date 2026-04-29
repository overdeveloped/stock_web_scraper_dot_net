using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeldonStockScannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinvizCompany",
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
                    table.PrimaryKey("PK_FinvizCompany", x => x.Ticker);
                });

            migrationBuilder.CreateTable(
                name: "WatchList",
                columns: table => new
                {
                    WatchListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchListName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchList", x => x.WatchListId);
                });

            migrationBuilder.CreateTable(
                name: "FinvizCompanyEntityWatchListEntity",
                columns: table => new
                {
                    CompaniesTicker = table.Column<string>(type: "nvarchar(12)", nullable: false),
                    WatchlistsWatchListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinvizCompanyEntityWatchListEntity", x => new { x.CompaniesTicker, x.WatchlistsWatchListId });
                    table.ForeignKey(
                        name: "FK_FinvizCompanyEntityWatchListEntity_FinvizCompany_CompaniesTicker",
                        column: x => x.CompaniesTicker,
                        principalTable: "FinvizCompany",
                        principalColumn: "Ticker",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinvizCompanyEntityWatchListEntity_WatchList_WatchlistsWatchListId",
                        column: x => x.WatchlistsWatchListId,
                        principalTable: "WatchList",
                        principalColumn: "WatchListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinvizCompanyEntityWatchListEntity_WatchlistsWatchListId",
                table: "FinvizCompanyEntityWatchListEntity",
                column: "WatchlistsWatchListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinvizCompanyEntityWatchListEntity");

            migrationBuilder.DropTable(
                name: "FinvizCompany");

            migrationBuilder.DropTable(
                name: "WatchList");
        }
    }
}
