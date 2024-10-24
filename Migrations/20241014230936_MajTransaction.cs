using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiMagazin.Migrations
{
    /// <inheritdoc />
    public partial class MajTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "idString",
                table: "Transaction",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idString",
                table: "Transaction");
        }
    }
}
