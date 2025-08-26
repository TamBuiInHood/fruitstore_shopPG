using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infastructure.Persistance.Migrations
{
    public partial class Update_Ordering_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");
        }
    }
}
