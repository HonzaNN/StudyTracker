using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyTracker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class newmig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectEntityId1",
                table: "Activity",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SubjectEntityId1",
                table: "Activity",
                column: "SubjectEntityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId1",
                table: "Activity",
                column: "SubjectEntityId1",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Subjects_SubjectEntityId1",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_SubjectEntityId1",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "SubjectEntityId1",
                table: "Activity");
        }
    }
}
