using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRailwayMasterData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "railway");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AssetTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Criticality = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Location = table.Column<Point>(type: "geometry (point, 4326)", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetTypes",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corridors",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corridors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platforms",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CorridorId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginStationId = table.Column<Guid>(type: "uuid", nullable: true),
                    DestinationStationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Location = table.Column<Point>(type: "geometry (point, 4326)", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StationZones",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StationZones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimetableServices",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    EffectiveFrom = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DepartureWindowStart = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    DepartureWindowEnd = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: true),
                    OperatingStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimetableServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackSegments",
                schema: "railway",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RetiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Geometry = table.Column<LineString>(type: "geometry (linestring, 4326)", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackSegments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Assets",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Assets",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "AssetTypes",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetTypes_OrganizationId_RetiredAt",
                schema: "railway",
                table: "AssetTypes",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Corridors_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Corridors",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corridors_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Corridors",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Divisions",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Divisions",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Platforms",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Platforms",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Routes",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Routes",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "Stations",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stations_OrganizationId_RetiredAt",
                schema: "railway",
                table: "Stations",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_StationZones_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "StationZones",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StationZones_OrganizationId_RetiredAt",
                schema: "railway",
                table: "StationZones",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TimetableServices_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "TimetableServices",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimetableServices_OrganizationId_RetiredAt",
                schema: "railway",
                table: "TimetableServices",
                columns: new[] { "OrganizationId", "RetiredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_TrackSegments_OrganizationId_DivisionId_Code",
                schema: "railway",
                table: "TrackSegments",
                columns: new[] { "OrganizationId", "DivisionId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackSegments_OrganizationId_RetiredAt",
                schema: "railway",
                table: "TrackSegments",
                columns: new[] { "OrganizationId", "RetiredAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assets",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "AssetTypes",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "Corridors",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "Divisions",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "Platforms",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "Routes",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "Stations",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "StationZones",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "TimetableServices",
                schema: "railway");

            migrationBuilder.DropTable(
                name: "TrackSegments",
                schema: "railway");
        }
    }
}
