using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlanchisserieBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddClientOrderStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ClientOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ClientOrders");
        }
    }
}
