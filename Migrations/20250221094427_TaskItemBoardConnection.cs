using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemBoardConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_BoardId",
                table: "TaskItems",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_Boards_BoardId",
                table: "TaskItems",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_Boards_BoardId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_BoardId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "TaskItems");
        }
    }
}
