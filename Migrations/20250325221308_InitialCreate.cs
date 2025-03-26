using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stato",
                table: "Prenotazioni");

            migrationBuilder.AddColumn<bool>(
                name: "StatoDisponibile",
                table: "Prenotazioni",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatoDisponibile",
                table: "Prenotazioni");

            migrationBuilder.AddColumn<string>(
                name: "Stato",
                table: "Prenotazioni",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
