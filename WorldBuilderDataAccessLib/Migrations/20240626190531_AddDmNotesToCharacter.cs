using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorldBuilderDataAccessLib.Migrations
{
    /// <inheritdoc />
    public partial class AddDmNotesToCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DmNotes",
                table: "Characters",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DmNotes",
                table: "Characters");
        }
    }
}
