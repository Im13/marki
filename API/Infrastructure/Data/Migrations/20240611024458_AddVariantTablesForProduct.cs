using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantTablesForProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "SKUId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "ValueId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "SKUId",
                table: "ProductSKUs");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductOptionValues");

            migrationBuilder.DropColumn(
                name: "OptionId",
                table: "ProductOptions");

            migrationBuilder.RenameColumn(
                name: "ValueId",
                table: "ProductOptionValues",
                newName: "ValueTempId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSKUId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "ProductSKUs",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "ProductSKUs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ProductSKUs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ImportPrice",
                table: "ProductSKUs",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "Products",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues",
                column: "ProductSKUId",
                principalTable: "ProductSKUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "ProductSKUs");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ProductSKUs");

            migrationBuilder.DropColumn(
                name: "ImportPrice",
                table: "ProductSKUs");

            migrationBuilder.RenameColumn(
                name: "ValueTempId",
                table: "ProductOptionValues",
                newName: "ValueId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductSKUId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SKUId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValueId",
                table: "ProductSKUValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SKU",
                table: "ProductSKUs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SKUId",
                table: "ProductSKUs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductOptionValues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OptionId",
                table: "ProductOptions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSKUValues_ProductSKUs_ProductSKUId",
                table: "ProductSKUValues",
                column: "ProductSKUId",
                principalTable: "ProductSKUs",
                principalColumn: "Id");
        }
    }
}
