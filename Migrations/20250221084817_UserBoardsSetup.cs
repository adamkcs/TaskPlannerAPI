using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskPlannerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserBoardsSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabel_Labels_LabelId",
                table: "TaskLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabel_TaskItems_TaskItemId",
                table: "TaskLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLabel",
                table: "TaskLabel");

            migrationBuilder.RenameTable(
                name: "TaskLabel",
                newName: "TaskLabels");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabel_LabelId",
                table: "TaskLabels",
                newName: "IX_TaskLabels_LabelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLabels",
                table: "TaskLabels",
                columns: new[] { "TaskItemId", "LabelId" });

            migrationBuilder.CreateTable(
                name: "UserBoards",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBoards", x => new { x.UserId, x.BoardId });
                    table.ForeignKey(
                        name: "FK_UserBoards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBoards_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBoards_BoardId",
                table: "UserBoards",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabels_Labels_LabelId",
                table: "TaskLabels",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabels_TaskItems_TaskItemId",
                table: "TaskLabels",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabels_Labels_LabelId",
                table: "TaskLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskLabels_TaskItems_TaskItemId",
                table: "TaskLabels");

            migrationBuilder.DropTable(
                name: "UserBoards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLabels",
                table: "TaskLabels");

            migrationBuilder.RenameTable(
                name: "TaskLabels",
                newName: "TaskLabel");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLabels_LabelId",
                table: "TaskLabel",
                newName: "IX_TaskLabel_LabelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLabel",
                table: "TaskLabel",
                columns: new[] { "TaskItemId", "LabelId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabel_Labels_LabelId",
                table: "TaskLabel",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLabel_TaskItems_TaskItemId",
                table: "TaskLabel",
                column: "TaskItemId",
                principalTable: "TaskItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
