using Microsoft.EntityFrameworkCore.Migrations;

namespace MacroNewt.Migrations
{
    public partial class dailyTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetDailyCalories",
                table: "DailyCalTotal",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetDailyCalories",
                table: "DailyCalTotal");
        }
    }
}
