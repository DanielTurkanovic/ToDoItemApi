using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoItemApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_Title",
                table: "ToDoItems");

            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_UserId",
                table: "ToDoItems");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_UserId_Title",
                table: "ToDoItems",
                columns: new[] { "UserId", "Title" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoItems_UserId_Title",
                table: "ToDoItems");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_Title",
                table: "ToDoItems",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_UserId",
                table: "ToDoItems",
                column: "UserId");
        }
    }
}
