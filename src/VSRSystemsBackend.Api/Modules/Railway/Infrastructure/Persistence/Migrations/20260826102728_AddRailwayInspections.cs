using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRailwayInspections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Defects",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionRunId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Severity = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RaisedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResolvedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssignedWorkOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionAssignments",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateVersion = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DueAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OccurrenceKey = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionAssignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionPlans",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateVersion = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Schedule = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    NextDueAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionRuns",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateVersion = table.Column<int>(type: "integer", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedInspectorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReviewedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    AmendsInspectionRunId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionTemplates",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TemplateVersion = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InspectionAnswers",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Response = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Measurement = table.Column<double>(type: "double precision", nullable: true),
                    EvidenceIdList = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    InspectionRunId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionAnswers_InspectionRuns_InspectionRunId",
                        column: x => x.InspectionRunId,
                        principalSchema: "railway",
                        principalTable: "InspectionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionRunRequirements",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    EvidenceRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Minimum = table.Column<double>(type: "double precision", nullable: true),
                    Maximum = table.Column<double>(type: "double precision", nullable: true),
                    InspectionRunId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionRunRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionRunRequirements_InspectionRuns_InspectionRunId",
                        column: x => x.InspectionRunId,
                        principalSchema: "railway",
                        principalTable: "InspectionRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionTemplateItems",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Label = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Required = table.Column<bool>(type: "boolean", nullable: false),
                    EvidenceRequired = table.Column<bool>(type: "boolean", nullable: false),
                    Minimum = table.Column<double>(type: "double precision", nullable: true),
                    Maximum = table.Column<double>(type: "double precision", nullable: true),
                    InspectionTemplateId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionTemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionTemplateItems_InspectionTemplates_InspectionTempl~",
                        column: x => x.InspectionTemplateId,
                        principalSchema: "railway",
                        principalTable: "InspectionTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Defects_OrganizationId_DivisionId_Status_Severity",
                schema: "railway",
                table: "Defects",
                columns: new[] { "OrganizationId", "DivisionId", "Status", "Severity" });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionAnswers_InspectionRunId",
                schema: "railway",
                table: "InspectionAnswers",
                column: "InspectionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionAssignments_OrganizationId_PlanId_TargetId_Occurr~",
                schema: "railway",
                table: "InspectionAssignments",
                columns: new[] { "OrganizationId", "PlanId", "TargetId", "OccurrenceKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionRunRequirements_InspectionRunId",
                schema: "railway",
                table: "InspectionRunRequirements",
                column: "InspectionRunId");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionTemplateItems_InspectionTemplateId_ItemId",
                schema: "railway",
                table: "InspectionTemplateItems",
                columns: new[] { "InspectionTemplateId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Defects",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionAnswers",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionAssignments",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionPlans",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionRunRequirements",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionTemplateItems",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionRuns",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "InspectionTemplates",
                schema: "railway");
        }
    }
}
