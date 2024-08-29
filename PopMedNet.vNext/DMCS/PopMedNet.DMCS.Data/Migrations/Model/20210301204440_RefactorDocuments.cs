using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PopMedNet.DMCS.Data.Migrations.Model
{
    public partial class RefactorDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM RequestDocuments");
            migrationBuilder.Sql("DELETE FROM Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestDocuments_Documents_DocumentID",
                table: "RequestDocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestDocuments",
                table: "RequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_RequestDocuments_ResponseID",
                table: "RequestDocuments");

            migrationBuilder.DropColumn(
                name: "DocumentID",
                table: "RequestDocuments");

            migrationBuilder.AddColumn<Guid>(
                name: "RevisionSetID",
                table: "RequestDocuments",
                nullable: false
                );

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Documents",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContentCreatedOn",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ContentModifiedOn",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Documents",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "ItemID",
                table: "Documents",
                nullable: false
                );

            migrationBuilder.AddColumn<string>(
                name: "Kind",
                table: "Documents",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UploadedByID",
                table: "Documents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Documents",
                maxLength: 50,
                nullable: true,
                defaultValue: "1.0.0.0");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestDocuments",
                table: "RequestDocuments",
                columns: new[] { "ResponseID", "RevisionSetID" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ID",
                table: "Documents",
                column: "ID")
                .Annotation("SqlServer:Include", new[] { "RevisionSetID", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UploadedByID",
                table: "Documents",
                column: "UploadedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Users_UploadedByID",
                table: "Documents",
                column: "UploadedByID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Users_UploadedByID",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestDocuments",
                table: "RequestDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Documents_ID",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_UploadedByID",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "RevisionSetID",
                table: "RequestDocuments");

            migrationBuilder.DropColumn(
                name: "ContentCreatedOn",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ContentModifiedOn",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "UploadedByID",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Documents");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentID",
                table: "RequestDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MimeType",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestDocuments",
                table: "RequestDocuments",
                columns: new[] { "DocumentID", "ResponseID" });

            migrationBuilder.CreateIndex(
                name: "IX_RequestDocuments_ResponseID",
                table: "RequestDocuments",
                column: "ResponseID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestDocuments_Documents_DocumentID",
                table: "RequestDocuments",
                column: "DocumentID",
                principalTable: "Documents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
