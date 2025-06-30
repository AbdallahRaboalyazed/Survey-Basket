using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurveyBasket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledColumnToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                columns: new[] { "IsDisabled", "PasswordHash" },
                values: new object[] { false, "AQAAAAIAAYagAAAAEPPP7U5PcGFgXBiuxwSEbCfedwQ+E9W1neJZLSaU5oejiMgrpWHgkWO1phfs6qtKYA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6dc6528a-b280-4770-9eae-82671ee81ef7",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEEIACTKYWqGTcEXBEF7rnLjY0E8I06kq/wxM2OSA5HnS/ZVSdQCZ/4LaOI/G/dJIlg==");
        }
    }
}
