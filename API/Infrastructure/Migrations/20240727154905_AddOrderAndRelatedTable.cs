using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderAndRelatedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSKUs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfflineOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShippingFee = table.Column<double>(type: "REAL", nullable: false),
                    OrderDiscount = table.Column<double>(type: "REAL", nullable: false),
                    BankTranferedAmount = table.Column<double>(type: "REAL", nullable: false),
                    ExtraFee = table.Column<double>(type: "REAL", nullable: false),
                    OrderNote = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderCareStaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerCareStaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReceiverName = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiverPhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: true),
                    WardId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfflineOrderSKUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductSKUId = table.Column<int>(type: "INTEGER", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    OfflineOrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineOrderSKUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfflineOrderSKUs_OfflineOrders_OfflineOrderId",
                        column: x => x.OfflineOrderId,
                        principalTable: "OfflineOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OfflineOrderSKUs_ProductSKUs_ProductSKUId",
                        column: x => x.ProductSKUId,
                        principalTable: "ProductSKUs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_CustomerId",
                table: "OfflineOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_DistrictId",
                table: "OfflineOrders",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_ProvinceId",
                table: "OfflineOrders",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_WardId",
                table: "OfflineOrders",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrderSKUs_OfflineOrderId",
                table: "OfflineOrderSKUs",
                column: "OfflineOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrderSKUs_ProductSKUId",
                table: "OfflineOrderSKUs",
                column: "ProductSKUId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs");

            migrationBuilder.DropTable(
                name: "OfflineOrderSKUs");

            migrationBuilder.DropTable(
                name: "OfflineOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductSKUs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
