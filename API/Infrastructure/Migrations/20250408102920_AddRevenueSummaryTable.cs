using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRevenueSummaryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevenueSummaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalRevenue = table.Column<double>(type: "REAL", nullable: false),
                    TotalOrders = table.Column<int>(type: "INTEGER", nullable: false),
                    ShopeeRevenue = table.Column<double>(type: "REAL", nullable: false),
                    FacebookRevenue = table.Column<double>(type: "REAL", nullable: false),
                    InstagramRevenue = table.Column<double>(type: "REAL", nullable: false),
                    WebsiteRevenue = table.Column<double>(type: "REAL", nullable: false),
                    OfflineRevenue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueSummaries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevenueSummaries_Date",
                table: "RevenueSummaries",
                column: "Date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevenueSummaries");
        }
    }
}
