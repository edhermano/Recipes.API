using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recipes.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Directions = table.Column<string>(type: "TEXT", nullable: true),
                    Difficulty = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<short>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Qty = table.Column<int>(type: "INTEGER", nullable: false),
                    Measurement = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Difficulty", "Directions", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "Easy", "Mix bake serve", "Cajun Meatloaf", (short)5 },
                    { 2, "Easy", "Mix bake serve", "Mexican Lasagna", (short)5 },
                    { 3, "Easy", "Mix Roll bake serve", "Enchilades", (short)5 }
                });

            migrationBuilder.InsertData(
                table: "Ingredients",
                columns: new[] { "Id", "Measurement", "Name", "Qty", "RecipeId" },
                values: new object[,]
                {
                    { 1, "lbs", "Ground Beef", 2, 1 },
                    { 2, null, "Egg", 2, 1 },
                    { 3, "cup", "Bread Crumbs", 1, 1 },
                    { 4, "oz", "Lasagna Noodles", 8, 2 },
                    { 5, "oz", "Chorizo", 8, 2 },
                    { 6, "oz", "Ground Beef", 8, 2 },
                    { 7, null, "Tortillas", 12, 3 },
                    { 8, "oz", "Enchilada Sauce", 16, 3 },
                    { 9, "oz", "Cheese", 16, 3 },
                    { 10, "oz", "Chicken", 12, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_RecipeId",
                table: "Ingredients",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
