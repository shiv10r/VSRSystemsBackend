using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRailwayIncidentResponseAndHardening : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ClosedAt",
                schema: "railway",
                table: "CrowdIncidents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedBy",
                schema: "railway",
                table: "CrowdIncidents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseLog",
                schema: "railway",
                table: "CrowdIncidents",
                type: "character varying(12000)",
                maxLength: 12000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedAt",
                schema: "railway",
                table: "CrowdIncidents");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                schema: "railway",
                table: "CrowdIncidents");

            migrationBuilder.DropColumn(
                name: "ResponseLog",
                schema: "railway",
                table: "CrowdIncidents");
        }
    }
}
