using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectX.Migrations
{
    /// <inheritdoc />
    public partial class updatejopCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategorysTATUS",
                table: "JopCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategorysTATUS",
                table: "JopCategories");
        }
    }
}
