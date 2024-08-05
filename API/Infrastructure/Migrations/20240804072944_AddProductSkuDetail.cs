using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSkuDetail : Migration
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

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrderSKUs_ProductSkuId",
                table: "OfflineOrderSKUs",
                column: "ProductSkuId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineOrderSKUs_ProductSKUs_ProductSkuId",
                table: "OfflineOrderSKUs",
                column: "ProductSkuId",
                principalTable: "ProductSKUs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineOrderSKUs_ProductSKUs_ProductSkuId",
                table: "OfflineOrderSKUs");

            migrationBuilder.DropIndex(
                name: "IX_OfflineOrderSKUs_ProductSkuId",
                table: "OfflineOrderSKUs");

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
