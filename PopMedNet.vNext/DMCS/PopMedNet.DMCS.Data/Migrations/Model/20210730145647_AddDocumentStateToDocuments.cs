using Microsoft.EntityFrameworkCore.Migrations;

namespace PopMedNet.DMCS.Data.Migrations.Model
{
    public partial class AddDocumentStateToDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentState",
                table: "Documents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE Documents SET DocumentState = 1 WHERE NOT EXISTS(
SELECT NULL FROM Documents d
JOIN RequestDocuments rd ON d.RevisionSetID = d.RevisionSetID
JOIN Responses rsp ON rd.ResponseID = rsp.ID
WHERE rd.DocumentType = 1 AND rsp.ResponseTime IS NULL AND d.ID = Documents.ID)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentState",
                table: "Documents");
        }
    }
}
