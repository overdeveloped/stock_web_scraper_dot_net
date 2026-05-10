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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
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
                    table.PrimaryKey("PK_FinvizCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WatchListName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchListCompanies",
                columns: table => new
                {
                    CompaniesId = table.Column<int>(type: "int", nullable: false),
                    WatchlistsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListCompanies", x => new { x.CompaniesId, x.WatchlistsId });
                    table.ForeignKey(
                        name: "FK_WatchListCompanies_FinvizCompany_CompaniesId",
                        column: x => x.CompaniesId,
                        principalTable: "FinvizCompany",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchListCompanies_WatchList_WatchlistsId",
                        column: x => x.WatchlistsId,
                        principalTable: "WatchList",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchListCompanies_WatchlistsId",
                table: "WatchListCompanies",
                column: "WatchlistsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchListCompanies");

            migrationBuilder.DropTable(
                name: "FinvizCompany");

            migrationBuilder.DropTable(
                name: "WatchList");
        }
    }
}
