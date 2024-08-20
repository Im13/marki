using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusEntityToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OfflineOrders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                table: "OfflineOrders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfflineOrders_OrderStatusId",
                table: "OfflineOrders",
                column: "OrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfflineOrders_OrderStatus_OrderStatusId",
                table: "OfflineOrders",
                column: "OrderStatusId",
                principalTable: "OrderStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfflineOrders_OrderStatus_OrderStatusId",
                table: "OfflineOrders");

            migrationBuilder.DropIndex(
                name: "IX_OfflineOrders_OrderStatusId",
                table: "OfflineOrders");

            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                table: "OfflineOrders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OfflineOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
