using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMaz.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileLinks",
                table: "Contribution");

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<int>(type: "int", nullable: false),
                    MIMEType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContributionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.FileId);
                    table.ForeignKey(
                        name: "FK_File_Contribution_ContributionId",
                        column: x => x.ContributionId,
                        principalTable: "Contribution",
                        principalColumn: "ContributionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_ContributionId",
                table: "File",
                column: "ContributionId");

            migrationBuilder.CreateIndex(
                name: "IX_File_Name",
                table: "File",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.AddColumn<string>(
                name: "FileLinks",
                table: "Contribution",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
