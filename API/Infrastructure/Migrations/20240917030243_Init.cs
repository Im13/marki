using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    EmailAddress = table.Column<string>(type: "TEXT", nullable: true),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShortName = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryTime = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DivisionType = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneCode = table.Column<int>(type: "INTEGER", nullable: false),
                    CodeName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopeeOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<string>(type: "TEXT", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
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
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerEmail = table.Column<string>(type: "TEXT", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ShipToAddress_Fullname = table.Column<string>(type: "TEXT", nullable: true),
                    ShipToAddress_CityOrProvinceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipToAddress_DistrictId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipToAddress_WardId = table.Column<int>(type: "INTEGER", nullable: false),
                    ShipToAddress_Street = table.Column<string>(type: "TEXT", nullable: true),
                    ShipToAddress_PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryMethodId = table.Column<int>(type: "INTEGER", nullable: true),
                    Subtotal = table.Column<double>(type: "REAL", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                        column: x => x.DeliveryMethodId,
                        principalTable: "DeliveryMethods",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    ProductSKU = table.Column<string>(type: "TEXT", nullable: true),
                    ImportPrice = table.Column<double>(type: "REAL", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    ProductTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    DivisionType = table.Column<string>(type: "TEXT", nullable: true),
                    CodeName = table.Column<string>(type: "TEXT", nullable: true),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    ReturnedQuantity = table.Column<string>(type: "TEXT", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemOrdered_ProductItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemOrdered_ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    ItemOrdered_PictureUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OptionName = table.Column<string>(type: "TEXT", nullable: true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductSKUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SKU = table.Column<string>(type: "TEXT", nullable: true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    ImportPrice = table.Column<double>(type: "REAL", nullable: false),
                    Barcode = table.Column<string>(type: "TEXT", nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Weight = table.Column<float>(type: "REAL", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSKUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSKUs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CodeName = table.Column<string>(type: "TEXT", nullable: true),
                    DivisionType = table.Column<string>(type: "TEXT", nullable: true),
                    ShortCodeName = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptionValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ValueName = table.Column<string>(type: "TEXT", nullable: true),
                    ValueTempId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductOptionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptionValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOptionValues_ProductOptions_ProductOptionId",
                        column: x => x.ProductOptionId,
                        principalTable: "ProductOptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false),
                    PublicId = table.Column<string>(type: "TEXT", nullable: true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductSKUsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_ProductSKUs_ProductSKUsId",
                        column: x => x.ProductSKUsId,
                        principalTable: "ProductSKUs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Photos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfflineOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShippingFee = table.Column<double>(type: "REAL", nullable: false),
                    OrderDiscount = table.Column<double>(type: "REAL", nullable: false),
                    BankTransferedAmount = table.Column<double>(type: "REAL", nullable: false),
                    ExtraFee = table.Column<double>(type: "REAL", nullable: false),
                    Total = table.Column<double>(type: "REAL", nullable: false),
                    OrderNote = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrderCareStaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerCareStaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReceiverName = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiverPhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    DistrictId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProvinceId = table.Column<int>(type: "INTEGER", nullable: false),
                    WardId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfflineOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_OrderStatus_OrderStatusId",
                        column: x => x.OrderStatusId,
                        principalTable: "OrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfflineOrders_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSKUValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ValueTempId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductOptionValueId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProductSKUsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSKUValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductOptionValues_ProductOptionValueId",
                        column: x => x.ProductOptionValueId,
                        principalTable: "ProductOptionValues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSKUValues_ProductSKUs_ProductSKUsId",
                        column: x => x.ProductSKUsId,
                        principalTable: "ProductSKUs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OfflineOrderSKUs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductSkuId = table.Column<int>(type: "INTEGER", nullable: true),
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
                        name: "FK_OfflineOrderSKUs_ProductSKUs_ProductSkuId",
                        column: x => x.ProductSkuId,
                        principalTable: "ProductSKUs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_CustomerId",
                table: "OfflineOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_DistrictId",
                table: "OfflineOrders",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_OrderStatusId",
                table: "OfflineOrders",
                column: "OrderStatusId");

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
                name: "IX_OfflineOrderSKUs_ProductSkuId",
                table: "OfflineOrderSKUs",
                column: "ProductSkuId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductSKUsId",
                table: "Photos",
                column: "ProductSKUsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptions_ProductId",
                table: "ProductOptions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionValues_ProductOptionId",
                table: "ProductOptionValues",
                column: "ProductOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUs_ProductId",
                table: "ProductSKUs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductOptionValueId",
                table: "ProductSKUValues",
                column: "ProductOptionValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductSKUsId",
                table: "ProductSKUValues",
                column: "ProductSKUsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopeeProducts_ShopeeOrderId",
                table: "ShopeeProducts",
                column: "ShopeeOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictId",
                table: "Wards",
                column: "DistrictId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfflineOrderSKUs");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "ProductSKUValues");

            migrationBuilder.DropTable(
                name: "ShopeeProducts");

            migrationBuilder.DropTable(
                name: "OfflineOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ProductOptionValues");

            migrationBuilder.DropTable(
                name: "ProductSKUs");

            migrationBuilder.DropTable(
                name: "ShopeeOrders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "DeliveryMethods");

            migrationBuilder.DropTable(
                name: "ProductOptions");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "ProductTypes");
        }
    }
}
