using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSKUValuefFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSKUsId",
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
                name: "IX_ProductSKUValues_ProductSKUsId",
                table: "ProductSKUValues",
                column: "ProductSKUsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUsId",
                table: "ProductSKUValues",
                column: "ProductSKUsId",
                principalTable: "ProductSKUs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUsId",
                table: "ProductSKUValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductSKUValues_ProductSKUsId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ProductSKUsId",
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
