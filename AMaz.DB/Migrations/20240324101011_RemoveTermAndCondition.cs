using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMaz.DB.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTermAndCondition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contribution_TermAndCondition_TermAndConditionId",
                table: "Contribution");

            migrationBuilder.DropTable(
                name: "TermAndCondition");

            migrationBuilder.DropIndex(
                name: "IX_Contribution_TermAndConditionId",
                table: "Contribution");

            migrationBuilder.DropColumn(
                name: "TermAndConditionId",
                table: "Contribution");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TermAndConditionId",
                table: "Contribution",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TermAndCondition",
                columns: table => new
                {
                    TermAndConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermAndCondition", x => x.TermAndConditionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_TermAndConditionId",
                table: "Contribution",
                column: "TermAndConditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contribution_TermAndCondition_TermAndConditionId",
                table: "Contribution",
                column: "TermAndConditionId",
                principalTable: "TermAndCondition",
                principalColumn: "TermAndConditionId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
