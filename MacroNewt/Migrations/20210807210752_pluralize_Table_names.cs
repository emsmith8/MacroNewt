using Microsoft.EntityFrameworkCore.Migrations;

namespace MacroNewt.Migrations
{
    public partial class pluralize_Table_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyCalTotal_AspNetUsers_Id",
                table: "DailyCalTotal");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Meal_MealId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Measure_Nutrient_NutrientId",
                table: "Measure");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrient_Food_FoodId",
                table: "Nutrient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nutrient",
                table: "Nutrient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measure",
                table: "Measure");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meal",
                table: "Meal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Food",
                table: "Food");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyCalTotal",
                table: "DailyCalTotal");

            migrationBuilder.RenameTable(
                name: "Nutrient",
                newName: "Nutrients");

            migrationBuilder.RenameTable(
                name: "Measure",
                newName: "Measures");

            migrationBuilder.RenameTable(
                name: "Meal",
                newName: "Meals");

            migrationBuilder.RenameTable(
                name: "Food",
                newName: "Foods");

            migrationBuilder.RenameTable(
                name: "DailyCalTotal",
                newName: "DailyCalTotals");

            migrationBuilder.RenameIndex(
                name: "IX_Nutrient_FoodId",
                table: "Nutrients",
                newName: "IX_Nutrients_FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_Measure_NutrientId",
                table: "Measures",
                newName: "IX_Measures_NutrientId");

            migrationBuilder.RenameIndex(
                name: "IX_Food_MealId",
                table: "Foods",
                newName: "IX_Foods_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyCalTotal_Id",
                table: "DailyCalTotals",
                newName: "IX_DailyCalTotals_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nutrients",
                table: "Nutrients",
                column: "NutrientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measures",
                table: "Measures",
                column: "MeasureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meals",
                table: "Meals",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Foods",
                table: "Foods",
                column: "FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyCalTotals",
                table: "DailyCalTotals",
                column: "DailyCalTotalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCalTotals_AspNetUsers_Id",
                table: "DailyCalTotals",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Meals_MealId",
                table: "Foods",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measures_Nutrients_NutrientId",
                table: "Measures",
                column: "NutrientId",
                principalTable: "Nutrients",
                principalColumn: "NutrientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrients_Foods_FoodId",
                table: "Nutrients",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "FoodId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyCalTotals_AspNetUsers_Id",
                table: "DailyCalTotals");

            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Meals_MealId",
                table: "Foods");

            migrationBuilder.DropForeignKey(
                name: "FK_Measures_Nutrients_NutrientId",
                table: "Measures");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutrients_Foods_FoodId",
                table: "Nutrients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nutrients",
                table: "Nutrients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Measures",
                table: "Measures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Meals",
                table: "Meals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Foods",
                table: "Foods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyCalTotals",
                table: "DailyCalTotals");

            migrationBuilder.RenameTable(
                name: "Nutrients",
                newName: "Nutrient");

            migrationBuilder.RenameTable(
                name: "Measures",
                newName: "Measure");

            migrationBuilder.RenameTable(
                name: "Meals",
                newName: "Meal");

            migrationBuilder.RenameTable(
                name: "Foods",
                newName: "Food");

            migrationBuilder.RenameTable(
                name: "DailyCalTotals",
                newName: "DailyCalTotal");

            migrationBuilder.RenameIndex(
                name: "IX_Nutrients_FoodId",
                table: "Nutrient",
                newName: "IX_Nutrient_FoodId");

            migrationBuilder.RenameIndex(
                name: "IX_Measures_NutrientId",
                table: "Measure",
                newName: "IX_Measure_NutrientId");

            migrationBuilder.RenameIndex(
                name: "IX_Foods_MealId",
                table: "Food",
                newName: "IX_Food_MealId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyCalTotals_Id",
                table: "DailyCalTotal",
                newName: "IX_DailyCalTotal_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nutrient",
                table: "Nutrient",
                column: "NutrientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Measure",
                table: "Measure",
                column: "MeasureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Meal",
                table: "Meal",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Food",
                table: "Food",
                column: "FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyCalTotal",
                table: "DailyCalTotal",
                column: "DailyCalTotalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyCalTotal_AspNetUsers_Id",
                table: "DailyCalTotal",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Meal_MealId",
                table: "Food",
                column: "MealId",
                principalTable: "Meal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Measure_Nutrient_NutrientId",
                table: "Measure",
                column: "NutrientId",
                principalTable: "Nutrient",
                principalColumn: "NutrientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutrient_Food_FoodId",
                table: "Nutrient",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "FoodId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
