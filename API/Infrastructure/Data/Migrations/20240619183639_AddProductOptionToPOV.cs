using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductOptionToPOV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptionValues_ProductOptions_ProductOptionsId",
                table: "ProductOptionValues");

            migrationBuilder.RenameColumn(
                name: "ProductOptionsId",
                table: "ProductOptionValues",
                newName: "ProductOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOptionValues_ProductOptionsId",
                table: "ProductOptionValues",
                newName: "IX_ProductOptionValues_ProductOptionId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptionValues_ProductOptions_ProductOptionId",
                table: "ProductOptionValues",
                column: "ProductOptionId",
                principalTable: "ProductOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptionValues_ProductOptions_ProductOptionId",
                table: "ProductOptionValues");

            migrationBuilder.RenameColumn(
                name: "ProductOptionId",
                table: "ProductOptionValues",
                newName: "ProductOptionsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductOptionValues_ProductOptionId",
                table: "ProductOptionValues",
                newName: "IX_ProductOptionValues_ProductOptionsId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptionValues_ProductOptions_ProductOptionsId",
                table: "ProductOptionValues",
                column: "ProductOptionsId",
                principalTable: "ProductOptions",
                principalColumn: "Id");
        }
    }
}
