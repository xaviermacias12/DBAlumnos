using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DBAlumnos.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFotoUrlPendiente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Alumnos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Alumnos");
        }
    }
}
