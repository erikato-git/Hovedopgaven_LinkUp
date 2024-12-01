using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REST_API.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NameOfEducation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GraduationYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                });

            migrationBuilder.CreateTable(
                name: "PersonInformation",
                columns: table => new
                {
                    PersonInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInformation", x => x.PersonInformationId);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonInformationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Accounts_PersonInformation_PersonInformationId",
                        column: x => x.PersonInformationId,
                        principalTable: "PersonInformation",
                        principalColumn: "PersonInformationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AlternativeTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isFlagged = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PortfolioId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AudienceSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AudienceSpecifications",
                columns: table => new
                {
                    AudienceSpecificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Professions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudienceSpecifications", x => x.AudienceSpecificationId);
                    table.ForeignKey(
                        name: "FK_AudienceSpecifications_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    KeywordId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Availability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.KeywordId);
                    table.ForeignKey(
                        name: "FK_Keywords_Educations_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Educations",
                        principalColumn: "EducationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Keywords_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pitches",
                columns: table => new
                {
                    PitchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SendingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TextMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitches", x => x.PitchId);
                    table.ForeignKey(
                        name: "FK_Pitches_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioItems",
                columns: table => new
                {
                    PortfolioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Projects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioItems", x => x.PortfolioId);
                    table.ForeignKey(
                        name: "FK_PortfolioItems_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PersonInformationId",
                table: "Accounts",
                column: "PersonInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_AudienceSpecifications_ProfileId",
                table: "AudienceSpecifications",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_EducationId",
                table: "Keywords",
                column: "EducationId",
                unique: true,
                filter: "[EducationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_ProfileId",
                table: "Keywords",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pitches_ProfileId",
                table: "Pitches",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItems_ProfileId",
                table: "PortfolioItems",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudienceSpecifications");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Pitches");

            migrationBuilder.DropTable(
                name: "PortfolioItems");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "PersonInformation");
        }
    }
}
