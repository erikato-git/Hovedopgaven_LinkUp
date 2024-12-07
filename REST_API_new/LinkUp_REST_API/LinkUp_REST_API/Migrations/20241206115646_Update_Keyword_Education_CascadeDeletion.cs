using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkUp_REST_API.Migrations
{
    /// <inheritdoc />
    public partial class Update_Keyword_Education_CascadeDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Keywords_Educations_EducationId",
                table: "Keywords");

            migrationBuilder.DropIndex(
                name: "IX_Keywords_EducationId",
                table: "Keywords");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_KeywordId",
                table: "Educations",
                column: "KeywordId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Keywords_KeywordId",
                table: "Educations",
                column: "KeywordId",
                principalTable: "Keywords",
                principalColumn: "KeywordId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Keywords_KeywordId",
                table: "Educations");

            migrationBuilder.DropIndex(
                name: "IX_Educations_KeywordId",
                table: "Educations");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_EducationId",
                table: "Keywords",
                column: "EducationId",
                unique: true,
                filter: "[EducationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_Educations_EducationId",
                table: "Keywords",
                column: "EducationId",
                principalTable: "Educations",
                principalColumn: "EducationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
