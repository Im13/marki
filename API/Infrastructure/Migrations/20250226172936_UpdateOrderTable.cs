using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTable : Migration
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

            migrationBuilder.AddColumn<double>(
                name: "BankTransferedAmount",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ExtraFee",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OrderDiscount",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OrderNote",
                table: "Orders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ShippingFee",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "Orders",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankTransferedAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExtraFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDiscount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNote",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");

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
