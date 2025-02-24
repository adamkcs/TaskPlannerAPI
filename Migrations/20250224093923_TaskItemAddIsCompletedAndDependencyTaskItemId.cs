using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class TaskItemAddIsCompletedAndDependencyTaskItemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DependencyTaskItemId",
                table: "TaskItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "TaskItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItems_DependencyTaskItemId",
                table: "TaskItems",
                column: "DependencyTaskItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItems_TaskItems_DependencyTaskItemId",
                table: "TaskItems",
                column: "DependencyTaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItems_TaskItems_DependencyTaskItemId",
                table: "TaskItems");

            migrationBuilder.DropIndex(
                name: "IX_TaskItems_DependencyTaskItemId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "DependencyTaskItemId",
                table: "TaskItems");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "TaskItems");
        }
    }
}
