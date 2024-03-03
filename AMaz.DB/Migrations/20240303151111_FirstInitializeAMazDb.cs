using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMaz.DB.Migrations
{
    /// <inheritdoc />
    public partial class FirstInitializeAMazDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateTimeFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.AcademicYearId);
                });

            migrationBuilder.CreateTable(
                name: "Faculty",
                columns: table => new
                {
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faculty", x => x.FacultyId);
                });

            migrationBuilder.CreateTable(
                name: "TermAndCondition",
                columns: table => new
                {
                    TermAndConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermAndCondition", x => x.TermAndConditionId);
                });

            migrationBuilder.CreateTable(
                name: "Magazine",
                columns: table => new
                {
                    MagazineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstClosureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinalClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazine", x => x.MagazineId);
                    table.ForeignKey(
                        name: "FK_Magazine_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "AcademicYearId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Magazine_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastDeactivatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FacultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "FacultyId");
                });

            migrationBuilder.CreateTable(
                name: "Contribution",
                columns: table => new
                {
                    ContributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileLinks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSeenByOrdinator = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptedTerms = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MagazineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TermAndConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contribution", x => x.ContributionId);
                    table.ForeignKey(
                        name: "FK_Contribution_Magazine_MagazineId",
                        column: x => x.MagazineId,
                        principalTable: "Magazine",
                        principalColumn: "MagazineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contribution_TermAndCondition_TermAndConditionId",
                        column: x => x.TermAndConditionId,
                        principalTable: "TermAndCondition",
                        principalColumn: "TermAndConditionId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Contribution_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_MagazineId",
                table: "Contribution",
                column: "MagazineId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_TermAndConditionId",
                table: "Contribution",
                column: "TermAndConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_Title",
                table: "Contribution",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_UserId",
                table: "Contribution",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Faculty_Name",
                table: "Faculty",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Magazine_AcademicYearId",
                table: "Magazine",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Magazine_FacultyId",
                table: "Magazine",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Magazine_Name",
                table: "Magazine",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FacultyId",
                table: "Users",
                column: "FacultyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contribution");

            migrationBuilder.DropTable(
                name: "Magazine");

            migrationBuilder.DropTable(
                name: "TermAndCondition");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropTable(
                name: "Faculty");
        }
    }
}
