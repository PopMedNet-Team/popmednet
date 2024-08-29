using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PopMedNet.DMCS.Data.Migrations.Model
{
    public partial class ModelInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Datamarts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Acronym = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AdapterID = table.Column<Guid>(nullable: true),
                    Adapter = table.Column<string>(nullable: true),
                    CacheDays = table.Column<int>(nullable: false),
                    EncryptCache = table.Column<bool>(nullable: false),
                    EnableExplictCacheRemoval = table.Column<bool>(nullable: false),
                    AutoProcess = table.Column<int>(nullable: false),
                    PmnTimestamp = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datamarts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    RevisionSetID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    Length = table.Column<long>(nullable: false),
                    PmnTimestamp = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Identifier = table.Column<long>(nullable: false),
                    MSRequestID = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    AdditionalInstructions = table.Column<string>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    ActivityDescription = table.Column<string>(nullable: true),
                    RequestType = table.Column<string>(nullable: true),
                    SubmittedOn = table.Column<DateTime>(nullable: false),
                    SubmittedBy = table.Column<string>(nullable: true),
                    Project = table.Column<string>(nullable: true),
                    PurposeOfUse = table.Column<string>(nullable: true),
                    PhiDisclosureLevel = table.Column<string>(nullable: true),
                    TaskOrder = table.Column<string>(nullable: true),
                    ActivityProject = table.Column<string>(nullable: true),
                    RequestorCenter = table.Column<string>(nullable: true),
                    WorkPlanType = table.Column<string>(nullable: true),
                    ReportAggregationLevel = table.Column<string>(nullable: true),
                    SourceActivity = table.Column<string>(nullable: true),
                    SourceActivityProject = table.Column<string>(nullable: true),
                    SourceTaskOrder = table.Column<string>(nullable: true),
                    PmnTimestamp = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RequestDataMarts",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    RequestID = table.Column<Guid>(nullable: false),
                    DataMartID = table.Column<Guid>(nullable: false),
                    ModelID = table.Column<Guid>(nullable: false),
                    ModelText = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: true),
                    RejectReason = table.Column<string>(nullable: true),
                    RoutingType = table.Column<int>(nullable: true),
                    PmnTimestamp = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDataMarts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestDataMarts_Datamarts_DataMartID",
                        column: x => x.DataMartID,
                        principalTable: "Datamarts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestDataMarts_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthenticationLogs",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false),
                    Success = table.Column<bool>(nullable: false),
                    IPAddress = table.Column<string>(maxLength: 40, nullable: true),
                    Details = table.Column<string>(maxLength: 500, nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationLogs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AuthenticationLogs_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDataMarts",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    DataMartID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDataMarts", x => new { x.UserID, x.DataMartID });
                    table.ForeignKey(
                        name: "FK_UserDataMarts_Datamarts_DataMartID",
                        column: x => x.DataMartID,
                        principalTable: "Datamarts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDataMarts_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    RequestDataMartID = table.Column<Guid>(nullable: false),
                    RespondedBy = table.Column<string>(nullable: true),
                    ResponseTime = table.Column<DateTime>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    SubmitMessage = table.Column<string>(nullable: true),
                    ResponseMessage = table.Column<string>(nullable: true),
                    PmnTimestamp = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Responses_RequestDataMarts_RequestDataMartID",
                        column: x => x.RequestDataMartID,
                        principalTable: "RequestDataMarts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestDocuments",
                columns: table => new
                {
                    ResponseID = table.Column<Guid>(nullable: false),
                    DocumentID = table.Column<Guid>(nullable: false),
                    DocumentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDocuments", x => new { x.DocumentID, x.ResponseID });
                    table.ForeignKey(
                        name: "FK_RequestDocuments_Documents_DocumentID",
                        column: x => x.DocumentID,
                        principalTable: "Documents",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestDocuments_Responses_ResponseID",
                        column: x => x.ResponseID,
                        principalTable: "Responses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationLogs_UserID",
                table: "AuthenticationLogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDataMarts_DataMartID",
                table: "RequestDataMarts",
                column: "DataMartID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDataMarts_RequestID",
                table: "RequestDataMarts",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDocuments_ResponseID",
                table: "RequestDocuments",
                column: "ResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_RequestDataMartID",
                table: "Responses",
                column: "RequestDataMartID");

            migrationBuilder.CreateIndex(
                name: "IX_UserDataMarts_DataMartID",
                table: "UserDataMarts",
                column: "DataMartID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationLogs");

            migrationBuilder.DropTable(
                name: "RequestDocuments");

            migrationBuilder.DropTable(
                name: "UserDataMarts");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "RequestDataMarts");

            migrationBuilder.DropTable(
                name: "Datamarts");

            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
