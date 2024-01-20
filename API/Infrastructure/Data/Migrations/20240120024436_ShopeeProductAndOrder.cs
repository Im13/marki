using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShopeeProductAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "ShopeeOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<string>(type: "TEXT", nullable: true),
                    OrderDate = table.Column<string>(type: "TEXT", nullable: true),
                    OrderStatus = table.Column<string>(type: "TEXT", nullable: true),
                    ShipmentCode = table.Column<string>(type: "TEXT", nullable: true),
                    ShippingCompany = table.Column<string>(type: "TEXT", nullable: true),
                    ReturnStatus = table.Column<string>(type: "TEXT", nullable: true),
                    TotalOrderValue = table.Column<double>(type: "REAL", nullable: false),
                    ShopVoucher = table.Column<double>(type: "REAL", nullable: false),
                    ShopeeCoinReturn = table.Column<double>(type: "REAL", nullable: false),
                    ShopeeVoucher = table.Column<double>(type: "REAL", nullable: false),
                    ShopeeComboDiscount = table.Column<double>(type: "REAL", nullable: false),
                    ShopComboDiscount = table.Column<double>(type: "REAL", nullable: false),
                    EstimatedShippingFee = table.Column<double>(type: "REAL", nullable: false),
                    CustomerShippingFee = table.Column<double>(type: "REAL", nullable: false),
                    EstimatedShoppingFeeShopeeDiscount = table.Column<double>(type: "REAL", nullable: false),
                    ReturnOrderFee = table.Column<double>(type: "REAL", nullable: false),
                    TotalOrderCustomerPaid = table.Column<double>(type: "REAL", nullable: false),
                    OrderCompletedDate = table.Column<string>(type: "TEXT", nullable: true),
                    OrderPaidDate = table.Column<string>(type: "TEXT", nullable: true),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: true),
                    FixedFee = table.Column<double>(type: "REAL", nullable: false),
                    ServiceFee = table.Column<double>(type: "REAL", nullable: false),
                    PaymentFee = table.Column<double>(type: "REAL", nullable: false),
                    Deposit = table.Column<double>(type: "REAL", nullable: false),
                    CustomerUsername = table.Column<string>(type: "TEXT", nullable: true),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Province = table.Column<string>(type: "TEXT", nullable: true),
                    District = table.Column<string>(type: "TEXT", nullable: true),
                    Ward = table.Column<string>(type: "TEXT", nullable: true),
                    AddressDetails = table.Column<string>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopeeOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopeeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SKU = table.Column<string>(type: "TEXT", nullable: true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    ProductPropertySKU = table.Column<string>(type: "TEXT", nullable: true),
                    ProductProperty = table.Column<string>(type: "TEXT", nullable: true),
                    Cost = table.Column<double>(type: "REAL", nullable: false),
                    ShopDiscount = table.Column<double>(type: "REAL", nullable: false),
                    ShopeeSale = table.Column<double>(type: "REAL", nullable: false),
                    TotalShopSale = table.Column<double>(type: "REAL", nullable: false),
                    SalePrice = table.Column<double>(type: "REAL", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ReturnedQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSellingPrice = table.Column<double>(type: "REAL", nullable: false),
                    ShopeeOrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopeeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopeeProducts_ShopeeOrders_ShopeeOrderId",
                        column: x => x.ShopeeOrderId,
                        principalTable: "ShopeeOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopeeProducts_ShopeeOrderId",
                table: "ShopeeProducts",
                column: "ShopeeOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopeeProducts");

            migrationBuilder.DropTable(
                name: "ShopeeOrders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
