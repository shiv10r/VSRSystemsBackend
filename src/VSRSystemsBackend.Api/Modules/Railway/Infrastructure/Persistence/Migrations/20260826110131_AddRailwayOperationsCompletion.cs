using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRailwayOperationsCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeadLetteredAt",
                schema: "platform",
                table: "OutboxMessages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InspectorId",
                schema: "railway",
                table: "InspectionPlans",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AuditRecords",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    ResourceType = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    ResourceId = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    BeforeJson = table.Column<string>(type: "jsonb", nullable: true),
                    AfterJson = table.Column<string>(type: "jsonb", nullable: true),
                    CorrelationId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdAlerts",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StationZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    IsOpen = table.Column<bool>(type: "boolean", nullable: false),
                    RaisedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AcknowledgedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AcknowledgedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdIncidents",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OpenedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OpenedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdIncidents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdIngestionNonces",
                schema: "railway",
                columns: table => new
                {
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nonce = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdIngestionNonces", x => new { x.SourceId, x.Nonce });
                });

            migrationBuilder.CreateTable(
                name: "CrowdObservations",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StationZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceEventId = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    WindowStart = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WindowEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Inflow = table.Column<int>(type: "integer", nullable: true),
                    Outflow = table.Column<int>(type: "integer", nullable: true),
                    Confidence = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    QualityFlags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdObservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdQuarantine",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PayloadHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdQuarantine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdSources",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StationZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AdapterType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    SigningSecretCiphertext = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PreviousSigningSecretCiphertext = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PreviousSecretValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    LastObservationAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CrowdThresholdPolicies",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StationZoneId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalWarningThreshold = table.Column<int>(type: "integer", nullable: false),
                    OriginalCriticalThreshold = table.Column<int>(type: "integer", nullable: false),
                    OverrideWarningThreshold = table.Column<int>(type: "integer", nullable: true),
                    OverrideCriticalThreshold = table.Column<int>(type: "integer", nullable: true),
                    OverrideReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    OverriddenBy = table.Column<Guid>(type: "uuid", nullable: true),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EffectiveUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrowdThresholdPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceipts",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    ReceivedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceipts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceParts",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Unit = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    OnHand = table.Column<int>(type: "integer", nullable: false),
                    Reserved = table.Column<int>(type: "integer", nullable: false),
                    ReorderLevel = table.Column<int>(type: "integer", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceParts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartReservations",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConsumedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementRequests",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditRecords_OrganizationId_OccurredAt",
                schema: "railway",
                table: "AuditRecords",
                columns: new[] { "OrganizationId", "OccurredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdAlerts_OrganizationId_StationZoneId_IsOpen",
                schema: "railway",
                table: "CrowdAlerts",
                columns: new[] { "OrganizationId", "StationZoneId", "IsOpen" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdIncidents_OrganizationId_StationId_Status",
                schema: "railway",
                table: "CrowdIncidents",
                columns: new[] { "OrganizationId", "StationId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdIngestionNonces_AcceptedAt",
                schema: "railway",
                table: "CrowdIngestionNonces",
                column: "AcceptedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CrowdObservations_OrganizationId_SourceId_SourceEventId",
                schema: "railway",
                table: "CrowdObservations",
                columns: new[] { "OrganizationId", "SourceId", "SourceEventId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CrowdObservations_OrganizationId_StationZoneId_WindowEnd",
                schema: "railway",
                table: "CrowdObservations",
                columns: new[] { "OrganizationId", "StationZoneId", "WindowEnd" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdQuarantine_OrganizationId_SourceId_CreatedAt",
                schema: "railway",
                table: "CrowdQuarantine",
                columns: new[] { "OrganizationId", "SourceId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdSources_OrganizationId_StationId_StationZoneId",
                schema: "railway",
                table: "CrowdSources",
                columns: new[] { "OrganizationId", "StationId", "StationZoneId" });

            migrationBuilder.CreateIndex(
                name: "IX_CrowdThresholdPolicies_OrganizationId_StationZoneId_Effecti~",
                schema: "railway",
                table: "CrowdThresholdPolicies",
                columns: new[] { "OrganizationId", "StationZoneId", "EffectiveFrom" });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceipts_OrganizationId_PurchaseOrderId",
                schema: "railway",
                table: "GoodsReceipts",
                columns: new[] { "OrganizationId", "PurchaseOrderId" });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceParts_OrganizationId_DivisionId_Sku",
                schema: "railway",
                table: "MaintenanceParts",
                columns: new[] { "OrganizationId", "DivisionId", "Sku" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartReservations_OrganizationId_WorkOrderId_PartId",
                schema: "railway",
                table: "PartReservations",
                columns: new[] { "OrganizationId", "WorkOrderId", "PartId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditRecords",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdAlerts",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdIncidents",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdIngestionNonces",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdObservations",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdQuarantine",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdSources",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "CrowdThresholdPolicies",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "GoodsReceipts",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "MaintenanceParts",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "PartReservations",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "ProcurementRequests",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "PurchaseOrders",
                schema: "railway");

            migrationBuilder.DropColumn(
                name: "DeadLetteredAt",
                schema: "platform",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "InspectorId",
                schema: "railway",
                table: "InspectionPlans");
        }
    }
}
