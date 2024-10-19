using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class Migraion02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Admin",
                table: "Users",
                newName: "IsAdmin");

            migrationBuilder.AddColumn<int>(
                name: "IsDispatched",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDispatched",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "IsAdmin",
                table: "Users",
                newName: "Admin");
        }
    }
}
