using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddressOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipToAddress_WardId",
                table: "Orders",
                newName: "WardId");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Street",
                table: "Orders",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_PhoneNumber",
                table: "Orders",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_Fullname",
                table: "Orders",
                newName: "Fullname");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_DistrictId",
                table: "Orders",
                newName: "DistrictId");

            migrationBuilder.RenameColumn(
                name: "ShipToAddress_CityOrProvinceId",
                table: "Orders",
                newName: "CityOrProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WardId",
                table: "Orders",
                newName: "ShipToAddress_WardId");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Orders",
                newName: "ShipToAddress_Street");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Orders",
                newName: "ShipToAddress_PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "Orders",
                newName: "ShipToAddress_Fullname");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Orders",
                newName: "ShipToAddress_DistrictId");

            migrationBuilder.RenameColumn(
                name: "CityOrProvinceId",
                table: "Orders",
                newName: "ShipToAddress_CityOrProvinceId");
        }
    }
}
