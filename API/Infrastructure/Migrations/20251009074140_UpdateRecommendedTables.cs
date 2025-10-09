using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRecommendedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTrending",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Style",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductCoOccurrences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId1 = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId2 = table.Column<int>(type: "INTEGER", nullable: false),
                    CoOccurrenceCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Product1Id = table.Column<int>(type: "INTEGER", nullable: true),
                    Product2Id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCoOccurrences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCoOccurrences_Products_Product1Id",
                        column: x => x.Product1Id,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductCoOccurrences_Products_Product2Id",
                        column: x => x.Product2Id,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductTrendings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CartCount = table.Column<int>(type: "INTEGER", nullable: false),
                    PurchaseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    TrendingScore = table.Column<double>(type: "REAL", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTrendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTrendings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastActivityAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.UniqueConstraint("AK_UserSessions_SessionId", x => x.SessionId);
                });

            migrationBuilder.CreateTable(
                name: "SessionInteractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SessionId = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    InteractionType = table.Column<int>(type: "INTEGER", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionInteractions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionInteractions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionInteractions_UserSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "UserSessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCoOccurrences_Product1Id",
                table: "ProductCoOccurrences",
                column: "Product1Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCoOccurrences_Product2Id",
                table: "ProductCoOccurrences",
                column: "Product2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTrendings_ProductId",
                table: "ProductTrendings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionInteractions_ProductId_Type",
                table: "SessionInteractions",
                columns: new[] { "ProductId", "InteractionType" });

            migrationBuilder.CreateIndex(
                name: "IX_SessionInteractions_SessionId_Date",
                table: "SessionInteractions",
                columns: new[] { "SessionId", "InteractionDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCoOccurrences");

            migrationBuilder.DropTable(
                name: "ProductTrendings");

            migrationBuilder.DropTable(
                name: "SessionInteractions");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropColumn(
                name: "IsTrending",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Material",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Style",
                table: "Products");
        }
    }
}
