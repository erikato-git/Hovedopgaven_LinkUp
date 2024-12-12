using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkUp_REST_API.Migrations
{
    /// <inheritdoc />
    public partial class update_media_remove_behaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medias_ProfileId",
                table: "Medias");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProfileId",
                table: "Medias",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ProfileId",
                table: "Medias",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medias_ProfileId",
                table: "Medias");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProfileId",
                table: "Medias",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medias_ProfileId",
                table: "Medias",
                column: "ProfileId",
                unique: true);
        }
    }
}
