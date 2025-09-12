using GymManagement.Domain.Entities.OperatingHoursModel;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Gym_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "schedule",
                table: "gyms");

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "gyms",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<OperatingHours>(
                name: "operating_hours",
                table: "gyms",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "region",
                table: "gyms",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country",
                table: "gyms");

            migrationBuilder.DropColumn(
                name: "operating_hours",
                table: "gyms");

            migrationBuilder.DropColumn(
                name: "region",
                table: "gyms");

            migrationBuilder.AddColumn<string>(
                name: "schedule",
                table: "gyms",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
