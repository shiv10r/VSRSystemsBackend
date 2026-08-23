using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VSRSystemsBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleDataDocuments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS "ModuleDataDocuments" (
                    "Id" uuid CONSTRAINT "PK_ModuleDataDocuments" PRIMARY KEY,
                    "Module" character varying(50) NOT NULL,
                    "Collection" character varying(150) NOT NULL,
                    "Json" text NOT NULL,
                    "CreatedAt" timestamp with time zone NOT NULL,
                    "UpdatedAt" timestamp with time zone NULL,
                    "IsDeleted" boolean NOT NULL DEFAULT FALSE
                );
                CREATE UNIQUE INDEX IF NOT EXISTS "IX_ModuleDataDocuments_Module_Collection"
                    ON "ModuleDataDocuments" ("Module", "Collection");
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS \"ModuleDataDocuments\";");
        }
    }
}
