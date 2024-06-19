using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOptionValuesToProductSKUValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductOptionValueId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSKUValues_ProductOptionValueId",
                table: "ProductSKUValues",
                column: "ProductOptionValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductOptionValues_ProductOptionValueId",
                table: "ProductSKUValues",
                column: "ProductOptionValueId",
                principalTable: "ProductOptionValues",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductOptionValues_ProductOptionValueId",
                table: "ProductSKUValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductSKUValues_ProductOptionValueId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ProductOptionValueId",
                table: "ProductSKUValues");

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
