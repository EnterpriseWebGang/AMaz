using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMaz.DB.Migrations
{
    /// <inheritdoc />
    public partial class NewContribution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcceptedDate",
                table: "Contribution",
                newName: "ApprovedDate");

            migrationBuilder.AddColumn<string>(
                name: "CoordinatorComment",
                table: "Contribution",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordinatorComment",
                table: "Contribution");

            migrationBuilder.RenameColumn(
                name: "ApprovedDate",
                table: "Contribution",
                newName: "AcceptedDate");
        }
    }
}
