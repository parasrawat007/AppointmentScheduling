using Microsoft.EntityFrameworkCore.Migrations;

namespace AppointmentScheduling.Migrations
{
    public partial class FixingAppointmetEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndeDate",
                table: "Appointments",
                newName: "EndDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Appointments",
                newName: "EndeDate");
        }
    }
}
