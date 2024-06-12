using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForeignKeyProductSKUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductOptionValues_ProductOptionValueId",
                table: "ProductSKUValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductOptions_ProductOptionId",
                table: "ProductSKUValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductSKUValues_ProductOptionId",
                table: "ProductSKUValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductSKUValues_ProductOptionValueId",
                table: "ProductSKUValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductSKUValues_ProductSKUId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ProductOptionId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ProductOptionValueId",
                table: "ProductSKUValues");

            migrationBuilder.RenameColumn(
                name: "ProductSKUId",
                table: "ProductSKUValues",
                newName: "OptionValueId");

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
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs");

            migrationBuilder.RenameColumn(
                name: "OptionValueId",
                table: "ProductSKUValues",
                newName: "ProductSKUId");

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductOptionId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductOptionValueId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: true);

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
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductOptionId",
                table: "ProductSKUValues",
                column: "ProductOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductOptionValueId",
                table: "ProductSKUValues",
                column: "ProductOptionValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductSKUId",
                table: "ProductSKUValues",
                column: "ProductSKUId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUs_Products_ProductId",
                table: "ProductSKUs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductOptionValues_ProductOptionValueId",
                table: "ProductSKUValues",
                column: "ProductOptionValueId",
                principalTable: "ProductOptionValues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductOptions_ProductOptionId",
                table: "ProductSKUValues",
                column: "ProductOptionId",
                principalTable: "ProductOptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues",
                column: "ProductSKUId",
                principalTable: "ProductSKUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
