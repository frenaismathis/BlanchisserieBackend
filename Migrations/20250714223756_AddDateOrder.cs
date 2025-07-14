using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlanchisserieBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddDateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "ClientOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "ClientOrders");
        }
    }
}
