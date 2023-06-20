using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyTracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class newmig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Subjects_SubjectId",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_SubjectId",
                table: "Activity");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId",
                table: "Activity",
                column: "SubjectEntityId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId",
                table: "Activity");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SubjectId",
                table: "Activity",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId",
                table: "Activity",
                column: "SubjectEntityId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Subjects_SubjectId",
                table: "Activity",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
