using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Builders.Bills.Database.Migrations
{
    /// <inheritdoc />
    public partial class BillTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Bills",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Bills");
        }
    }
}
