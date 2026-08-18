using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VSRSystemsBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeServicesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationMode",
                table: "jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "EasyApply");

            migrationBuilder.AddColumn<string>(
                name: "BenefitsJson",
                table: "jobs",
                type: "text",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "CanonicalFingerprint",
                table: "jobs",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyInitials",
                table: "jobs",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "jobs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "jobs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "India");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Full-time");

            migrationBuilder.AddColumn<string>(
                name: "ExperienceText",
                table: "jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExternalApplyUrl",
                table: "jobs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalJobId",
                table: "jobs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Featured",
                table: "jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAggregated",
                table: "jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeenAtSource",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxExperience",
                table: "jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinExperience",
                table: "jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OriginalSourceUrl",
                table: "jobs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostedAtSource",
                table: "jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryJobSourceId",
                table: "jobs",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibilitiesJson",
                table: "jobs",
                type: "text",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SalaryText",
                table: "jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SalaryVisible",
                table: "jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SkillsJson",
                table: "jobs",
                type: "text",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "jobs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "jobs",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "jobs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WorkMode",
                table: "jobs",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "On-site");

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActorId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BeforeJson = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false, defaultValue: "{}"),
                    AfterJson = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false, defaultValue: "{}"),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "banners",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    LinkUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    LaunchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "cms_pages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Title = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Body = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cms_pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "commission_rules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProfessionalTier = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RatePercent = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    FlatFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_commission_rules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DiscountType = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false, defaultValue: "flat"),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    MaxDiscount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    MinOrderValue = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsageLimit = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    PerCustomerLimit = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    TargetType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TargetValue = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "duplicate_candidates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobIdA = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobIdB = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_duplicate_candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "faqs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Question = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Answer = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faqs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ingestion_errors",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RawExternalJobId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    JobSourceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ErrorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    NextRetryAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingestion_errors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_source_configs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobSourceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConfigJson = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_source_configs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_source_mappings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobSourceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExternalJobId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourceUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ApplyUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    PayloadHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_source_mappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "job_sources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    CompanyId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SourceType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "JsonFeed"),
                    BaseUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FeedUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CareersUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AdapterKey = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsAuthorized = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AuthorizationNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RequestIntervalMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 120),
                    MaxRequestsPerMinute = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    DefaultCountry = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "India"),
                    DefaultCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "INR"),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LastSuccessfulRunAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastFailedRunAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConsecutiveFailures = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    HealthStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Healthy"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "membership_plans",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DurationDays = table.Column<int>(type: "integer", nullable: false, defaultValue: 365),
                    BenefitsJson = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false, defaultValue: "[]"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_membership_plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Channel = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "in_app"),
                    Template = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayloadJson = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false, defaultValue: "{}"),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payment_gateway_settings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Provider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "razorpay"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Mode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "test"),
                    KeyId = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    KeySecretRef = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WebhookSecretRef = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_gateway_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payment_gateway_webhook_events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Provider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "razorpay"),
                    EventType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PayloadJson = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false, defaultValue: "{}"),
                    SignatureValid = table.Column<bool>(type: "boolean", nullable: false),
                    Processed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProcessingError = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_gateway_webhook_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Area = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "professionals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Dob = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OnboardingStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "draft"),
                    QualityScore = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Tier = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "bronze"),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Phone = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professionals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "raw_external_jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobSourceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExternalJobId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SourceUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ApplyUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RawTitle = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RawCompany = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    RawLocation = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    RawDescription = table.Column<string>(type: "text", nullable: true),
                    RawSalary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RawPostedDate = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RawEmploymentType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RawWorkMode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RawSkills = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RawIndustry = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PayloadHash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RawPayload = table.Column<string>(type: "text", nullable: true),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ProcessingStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "New"),
                    ProcessingError = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_raw_external_jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scrape_logs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScrapeRunId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Level = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "Info"),
                    EventType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Generic"),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExternalJobId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: true),
                    ExceptionType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MetadataJson = table.Column<string>(type: "text", nullable: true),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scrape_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scrape_runs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    JobSourceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "Queued"),
                    TriggeredBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "Scheduler"),
                    JobsDiscovered = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsFetched = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsCreated = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsUpdated = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsUnchanged = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsDuplicate = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsRejected = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsClosed = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    HttpRequests = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    HttpErrors = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ParseErrors = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    ErrorSummary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scrape_runs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "service_categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    Tagline = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pincodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsServiceable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pincodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pincodes_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "zones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_zones_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "payouts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payouts_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_adjustments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_adjustments_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_availabilities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false, defaultValue: new TimeSpan(0, 9, 0, 0, 0)),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false, defaultValue: new TimeSpan(0, 19, 0, 0, 0)),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_availabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_availabilities_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DocType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    ReviewedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RejectionReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_documents_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_incentives",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IncentiveType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "accrued"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_incentives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_incentives_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_performances",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    JobsCompleted = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    JobsCancelled = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AvgRating = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    OnTimeRate = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    AcceptanceRate = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_performances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_performances_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_time_offs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_time_offs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_time_offs_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PermissionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LongDescription = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsEmergency = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    NeedsInspection = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    InspectionFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_services_service_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "service_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "home_service_customers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DefaultAddressId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    WalletBalance = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    MembershipPlanId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReferralCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ReferredByCustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_home_service_customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_home_service_customers_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "localities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ZoneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Pincode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_localities_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "professional_service_areas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ZoneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_service_areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_service_areas_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_professional_service_areas_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_professional_service_areas_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_areas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ZoneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_areas_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_areas_zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_rules",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackageId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    RuleType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "discount"),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_price_rules_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_skills",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SkillLevel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "standard"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_skills_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_professional_skills_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_add_ons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DurationMins = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_add_ons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_add_ons_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_packages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DetailedDescription = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DurationMins = table.Column<int>(type: "integer", nullable: false, defaultValue: 60),
                    WhatIncluded = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    WhatExcluded = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Warranty = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InspectionRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PartsIncluded = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MinimumCharge = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CancellationRule = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsPopular = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsEmergencyEligible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_packages_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_problems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_problems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_problems_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_warranties",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WarrantyDays = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Terms = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_warranties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_warranties_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "amc_contracts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VisitsPerYear = table.Column<int>(type: "integer", nullable: false, defaultValue: 2),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    CoveredServices = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ExcludedParts = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_amc_contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_amc_contracts_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_amc_contracts_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupon_redemptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CouponId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountApplied = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon_redemptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coupon_redemptions_coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_coupon_redemptions_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "credit_transactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "credit"),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ReferenceBookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BalanceAfter = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_credit_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_credit_transactions_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "home"),
                    Line1 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Line2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ZoneId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LocalityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Pincode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Lat = table.Column<double>(type: "double precision", nullable: true),
                    Lng = table.Column<double>(type: "double precision", nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    ContactPerson = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    AccessInstructions = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_addresses_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer_memberships",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PlanId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_memberships_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customer_memberships_membership_plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "membership_plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recurring_bookings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackageId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Frequency = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "monthly"),
                    NextRunAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PreferredProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recurring_bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recurring_bookings_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recurring_bookings_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "referrals",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReferrerCustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RefereeCustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RewardAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_referrals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_referrals_home_service_customers_RefereeCustomerId",
                        column: x => x.RefereeCustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_home_service_customers_ReferrerCustomerId",
                        column: x => x.ReferrerCustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_area_services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceAreaId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_area_services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_area_services_service_areas_ServiceAreaId",
                        column: x => x.ServiceAreaId,
                        principalTable: "service_areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_area_services_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "home_service_bookings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackageId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "scheduled"),
                    ScheduledStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "draft"),
                    AssignedProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PriceQuoteId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CurrentQuoteId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "pending"),
                    CustomerNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    OpsNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ActualStartAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelReason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsRework = table.Column<bool>(type: "boolean", nullable: false),
                    OriginalBookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_home_service_bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_home_service_bookings_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_home_service_bookings_service_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "service_packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_home_service_bookings_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "price_quotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    QuoteNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackageId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    AddOnsTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    MaterialsTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    FeesTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TravelCharge = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    UrgentCharge = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    PlatformFee = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    DiscountTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TaxTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    GrandTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CouponId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CouponCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "active"),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_quotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_price_quotes_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_price_quotes_service_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "service_packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_price_quotes_services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_package_add_ons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackageId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddOnId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_service_package_add_ons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_service_package_add_ons_service_add_ons_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "service_add_ons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_service_package_add_ons_service_packages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "service_packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_add_ons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddOnId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_add_ons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_add_ons_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_add_ons_service_add_ons_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "service_add_ons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_assignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OfferedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Response = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    DeclineReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_assignments_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_assignments_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LineTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_items_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_materials",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    LineTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    PhotoUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ApprovedByCustomer = table.Column<bool>(type: "boolean", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_materials_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_notes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AuthorId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Visibility = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "internal"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_notes_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking_status_history",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PreviousStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NewStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ChangedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MetadataJson = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false, defaultValue: "{}"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking_status_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_booking_status_history_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsMasked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversations_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_conversations_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_conversations_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "disputes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TicketId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RaisedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValue: "open"),
                    Resolution = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ResolvedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_disputes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_disputes_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "home_service_reviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    Comment = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    TagsJson = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false, defaultValue: "[]"),
                    Quality = table.Column<int>(type: "integer", nullable: true),
                    Professionalism = table.Column<int>(type: "integer", nullable: true),
                    Punctuality = table.Column<int>(type: "integer", nullable: true),
                    Cleanliness = table.Column<int>(type: "integer", nullable: true),
                    Communication = table.Column<int>(type: "integer", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_home_service_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_home_service_reviews_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_home_service_reviews_home_service_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "home_service_customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_home_service_reviews_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Method = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "upi"),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "initiated"),
                    GatewayRef = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GatewayProvider = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    GatewayOrderId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GatewayPaymentId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    GatewaySignature = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WebhookVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "professional_earnings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProfessionalId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    GrossAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    MaterialsExcludedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CommissionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    AdjustmentAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    TaxWithheldAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    NetAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "pending"),
                    SettledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_professional_earnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_professional_earnings_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_professional_earnings_professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "support_tickets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RaisedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "customer"),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Subject = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "open"),
                    Priority = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "medium"),
                    AssignedTo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Resolution = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_support_tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_support_tickets_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "quote_revisions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PriceQuoteId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RevisionNumber = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    PreviousTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    NewTotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    CreatedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quote_revisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quote_revisions_price_quotes_PriceQuoteId",
                        column: x => x.PriceQuoteId,
                        principalTable: "price_quotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversation_messages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ConversationId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SenderId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Body = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conversation_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_conversation_messages_conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review_media",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReviewId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MediaUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MediaType = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "image"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_media", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_media_home_service_reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "home_service_reviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refunds",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PaymentId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "requested"),
                    ProcessedBy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    GatewayRefundId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refunds_home_service_bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "home_service_bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refunds_payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_jobs_CanonicalFingerprint",
                table: "jobs",
                column: "CanonicalFingerprint",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_PrimaryJobSourceId",
                table: "jobs",
                column: "PrimaryJobSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_amc_contracts_CustomerId",
                table: "amc_contracts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_amc_contracts_EndDate",
                table: "amc_contracts",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_amc_contracts_ServiceId",
                table: "amc_contracts",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_amc_contracts_Status",
                table: "amc_contracts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_Action",
                table: "audit_logs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_CreatedAt",
                table: "audit_logs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_EntityId",
                table: "audit_logs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_EntityType",
                table: "audit_logs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_banners_IsActive",
                table: "banners",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_banners_SortOrder",
                table: "banners",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_booking_add_ons_AddOnId",
                table: "booking_add_ons",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_add_ons_BookingId_AddOnId",
                table: "booking_add_ons",
                columns: new[] { "BookingId", "AddOnId" });

            migrationBuilder.CreateIndex(
                name: "IX_booking_assignments_BookingId_ProfessionalId",
                table: "booking_assignments",
                columns: new[] { "BookingId", "ProfessionalId" },
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_booking_assignments_ProfessionalId",
                table: "booking_assignments",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_assignments_Response",
                table: "booking_assignments",
                column: "Response");

            migrationBuilder.CreateIndex(
                name: "IX_booking_items_BookingId",
                table: "booking_items",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_materials_BookingId",
                table: "booking_materials",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_notes_BookingId",
                table: "booking_notes",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_history_BookingId",
                table: "booking_status_history",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_history_ChangedAt",
                table: "booking_status_history",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_booking_status_history_NewStatus",
                table: "booking_status_history",
                column: "NewStatus");

            migrationBuilder.CreateIndex(
                name: "IX_cities_IsActive",
                table: "cities",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_cities_Name",
                table: "cities",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_cms_pages_IsPublished",
                table: "cms_pages",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_cms_pages_Slug",
                table: "cms_pages",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_commission_rules_CategoryId",
                table: "commission_rules",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_commission_rules_IsActive",
                table: "commission_rules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_commission_rules_ServiceId",
                table: "commission_rules",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_messages_ConversationId",
                table: "conversation_messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_messages_SenderId",
                table: "conversation_messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_messages_SentAt",
                table: "conversation_messages",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_BookingId_CustomerId_ProfessionalId",
                table: "conversations",
                columns: new[] { "BookingId", "CustomerId", "ProfessionalId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_CustomerId",
                table: "conversations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_conversations_ProfessionalId",
                table: "conversations",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_coupon_redemptions_BookingId",
                table: "coupon_redemptions",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_coupon_redemptions_CouponId_CustomerId_BookingId",
                table: "coupon_redemptions",
                columns: new[] { "CouponId", "CustomerId", "BookingId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_coupon_redemptions_CustomerId",
                table: "coupon_redemptions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_Code",
                table: "coupons",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_IsActive",
                table: "coupons",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_coupons_TargetType",
                table: "coupons",
                column: "TargetType");

            migrationBuilder.CreateIndex(
                name: "IX_credit_transactions_CustomerId",
                table: "credit_transactions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_credit_transactions_ReferenceBookingId",
                table: "credit_transactions",
                column: "ReferenceBookingId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_addresses_CustomerId",
                table: "customer_addresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_customer_addresses_Pincode",
                table: "customer_addresses",
                column: "Pincode");

            migrationBuilder.CreateIndex(
                name: "IX_customer_memberships_CustomerId_Status",
                table: "customer_memberships",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_customer_memberships_ExpiresAt",
                table: "customer_memberships",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_customer_memberships_PlanId",
                table: "customer_memberships",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_disputes_BookingId",
                table: "disputes",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_disputes_RaisedBy",
                table: "disputes",
                column: "RaisedBy");

            migrationBuilder.CreateIndex(
                name: "IX_disputes_Status",
                table: "disputes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_duplicate_candidates_JobIdA_JobIdB",
                table: "duplicate_candidates",
                columns: new[] { "JobIdA", "JobIdB" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_duplicate_candidates_Status",
                table: "duplicate_candidates",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_faqs_Category",
                table: "faqs",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_AssignedProfessionalId",
                table: "home_service_bookings",
                column: "AssignedProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_BookingNumber",
                table: "home_service_bookings",
                column: "BookingNumber",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_CustomerId",
                table: "home_service_bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_PackageId",
                table: "home_service_bookings",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_PaymentStatus",
                table: "home_service_bookings",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_ScheduledStart",
                table: "home_service_bookings",
                column: "ScheduledStart");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_ServiceId",
                table: "home_service_bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_bookings_Status",
                table: "home_service_bookings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_customers_Email",
                table: "home_service_customers",
                column: "Email",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_customers_Phone",
                table: "home_service_customers",
                column: "Phone",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_customers_ReferralCode",
                table: "home_service_customers",
                column: "ReferralCode",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_customers_UserId",
                table: "home_service_customers",
                column: "UserId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_reviews_BookingId",
                table: "home_service_reviews",
                column: "BookingId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_reviews_CustomerId",
                table: "home_service_reviews",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_reviews_ProfessionalId",
                table: "home_service_reviews",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_home_service_reviews_Rating",
                table: "home_service_reviews",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_ingestion_errors_ErrorCode",
                table: "ingestion_errors",
                column: "ErrorCode");

            migrationBuilder.CreateIndex(
                name: "IX_ingestion_errors_JobSourceId",
                table: "ingestion_errors",
                column: "JobSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ingestion_errors_ResolvedAt",
                table: "ingestion_errors",
                column: "ResolvedAt");

            migrationBuilder.CreateIndex(
                name: "IX_job_source_configs_JobSourceId",
                table: "job_source_configs",
                column: "JobSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_job_source_configs_JobSourceId_Version",
                table: "job_source_configs",
                columns: new[] { "JobSourceId", "Version" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_job_source_mappings_JobId",
                table: "job_source_mappings",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_job_source_mappings_JobId_JobSourceId",
                table: "job_source_mappings",
                columns: new[] { "JobId", "JobSourceId" },
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_job_source_mappings_JobSourceId_ExternalJobId",
                table: "job_source_mappings",
                columns: new[] { "JobSourceId", "ExternalJobId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_job_sources_HealthStatus",
                table: "job_sources",
                column: "HealthStatus");

            migrationBuilder.CreateIndex(
                name: "IX_job_sources_IsEnabled",
                table: "job_sources",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_job_sources_Slug",
                table: "job_sources",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_job_sources_SourceType",
                table: "job_sources",
                column: "SourceType");

            migrationBuilder.CreateIndex(
                name: "IX_localities_Pincode",
                table: "localities",
                column: "Pincode");

            migrationBuilder.CreateIndex(
                name: "IX_localities_ZoneId_Name",
                table: "localities",
                columns: new[] { "ZoneId", "Name" },
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_membership_plans_IsActive",
                table: "membership_plans",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_membership_plans_Name",
                table: "membership_plans",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_ReadAt",
                table: "notifications",
                column: "ReadAt");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_SentAt",
                table: "notifications",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_UserId",
                table: "notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_settings_IsActive",
                table: "payment_gateway_settings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_settings_Provider_Mode",
                table: "payment_gateway_settings",
                columns: new[] { "Provider", "Mode" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_webhook_events_BookingId",
                table: "payment_gateway_webhook_events",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_webhook_events_Processed",
                table: "payment_gateway_webhook_events",
                column: "Processed");

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_webhook_events_Provider_EventType",
                table: "payment_gateway_webhook_events",
                columns: new[] { "Provider", "EventType" });

            migrationBuilder.CreateIndex(
                name: "IX_payment_gateway_webhook_events_SignatureValid",
                table: "payment_gateway_webhook_events",
                column: "SignatureValid");

            migrationBuilder.CreateIndex(
                name: "IX_payments_BookingId",
                table: "payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_GatewayOrderId",
                table: "payments",
                column: "GatewayOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_GatewayPaymentId",
                table: "payments",
                column: "GatewayPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_PaymentNumber",
                table: "payments",
                column: "PaymentNumber",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_payments_Status",
                table: "payments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_PeriodStart",
                table: "payouts",
                column: "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_ProfessionalId",
                table: "payouts",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_payouts_Status",
                table: "payouts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_Code",
                table: "permissions",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_pincodes_CityId",
                table: "pincodes",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_pincodes_Code",
                table: "pincodes",
                column: "Code",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_CustomerId",
                table: "price_quotes",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_ExpiresAt",
                table: "price_quotes",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_PackageId",
                table: "price_quotes",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_QuoteNumber",
                table: "price_quotes",
                column: "QuoteNumber",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_ServiceId",
                table: "price_quotes",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_price_quotes_Status",
                table: "price_quotes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_price_rules_IsActive",
                table: "price_rules",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_price_rules_RuleType",
                table: "price_rules",
                column: "RuleType");

            migrationBuilder.CreateIndex(
                name: "IX_price_rules_ServiceId",
                table: "price_rules",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_adjustments_BookingId",
                table: "professional_adjustments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_adjustments_ProfessionalId",
                table: "professional_adjustments",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_availabilities_DayOfWeek_IsRecurring",
                table: "professional_availabilities",
                columns: new[] { "DayOfWeek", "IsRecurring" });

            migrationBuilder.CreateIndex(
                name: "IX_professional_availabilities_ProfessionalId",
                table: "professional_availabilities",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_documents_ProfessionalId_DocType",
                table: "professional_documents",
                columns: new[] { "ProfessionalId", "DocType" });

            migrationBuilder.CreateIndex(
                name: "IX_professional_documents_Status",
                table: "professional_documents",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_professional_earnings_BookingId",
                table: "professional_earnings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_earnings_ProfessionalId_BookingId",
                table: "professional_earnings",
                columns: new[] { "ProfessionalId", "BookingId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_professional_earnings_SettledAt",
                table: "professional_earnings",
                column: "SettledAt");

            migrationBuilder.CreateIndex(
                name: "IX_professional_earnings_Status",
                table: "professional_earnings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_professional_incentives_ProfessionalId",
                table: "professional_incentives",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_incentives_Status",
                table: "professional_incentives",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_professional_performances_ProfessionalId_PeriodStart_Period~",
                table: "professional_performances",
                columns: new[] { "ProfessionalId", "PeriodStart", "PeriodEnd" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_professional_service_areas_CityId",
                table: "professional_service_areas",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_service_areas_ProfessionalId_CityId_ZoneId",
                table: "professional_service_areas",
                columns: new[] { "ProfessionalId", "CityId", "ZoneId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_professional_service_areas_ZoneId",
                table: "professional_service_areas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_skills_ProfessionalId_ServiceId",
                table: "professional_skills",
                columns: new[] { "ProfessionalId", "ServiceId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_professional_skills_ServiceId",
                table: "professional_skills",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_time_offs_ProfessionalId",
                table: "professional_time_offs",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_professional_time_offs_StartAt",
                table: "professional_time_offs",
                column: "StartAt");

            migrationBuilder.CreateIndex(
                name: "IX_professionals_OnboardingStatus",
                table: "professionals",
                column: "OnboardingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_professionals_Tier",
                table: "professionals",
                column: "Tier");

            migrationBuilder.CreateIndex(
                name: "IX_professionals_UserId",
                table: "professionals",
                column: "UserId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_quote_revisions_PriceQuoteId",
                table: "quote_revisions",
                column: "PriceQuoteId");

            migrationBuilder.CreateIndex(
                name: "IX_raw_external_jobs_FetchedAt",
                table: "raw_external_jobs",
                column: "FetchedAt");

            migrationBuilder.CreateIndex(
                name: "IX_raw_external_jobs_JobSourceId",
                table: "raw_external_jobs",
                column: "JobSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_raw_external_jobs_JobSourceId_ExternalJobId",
                table: "raw_external_jobs",
                columns: new[] { "JobSourceId", "ExternalJobId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_raw_external_jobs_PayloadHash",
                table: "raw_external_jobs",
                column: "PayloadHash");

            migrationBuilder.CreateIndex(
                name: "IX_raw_external_jobs_ProcessingStatus",
                table: "raw_external_jobs",
                column: "ProcessingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_bookings_CustomerId",
                table: "recurring_bookings",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_bookings_IsActive",
                table: "recurring_bookings",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_bookings_NextRunAt",
                table: "recurring_bookings",
                column: "NextRunAt");

            migrationBuilder.CreateIndex(
                name: "IX_recurring_bookings_ServiceId",
                table: "recurring_bookings",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_RefereeCustomerId",
                table: "referrals",
                column: "RefereeCustomerId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_ReferrerCustomerId",
                table: "referrals",
                column: "ReferrerCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_Status",
                table: "referrals",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_refunds_BookingId",
                table: "refunds",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_refunds_PaymentId",
                table: "refunds",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_refunds_Status",
                table: "refunds",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_review_media_ReviewId",
                table: "review_media",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_PermissionId",
                table: "role_permissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_RoleId_PermissionId",
                table: "role_permissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_logs_CreatedAt",
                table: "scrape_logs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_logs_EventType",
                table: "scrape_logs",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_logs_Level",
                table: "scrape_logs",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_logs_ScrapeRunId",
                table: "scrape_logs",
                column: "ScrapeRunId");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_runs_CorrelationId",
                table: "scrape_runs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_runs_JobSourceId",
                table: "scrape_runs",
                column: "JobSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_runs_StartedAt",
                table: "scrape_runs",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_scrape_runs_Status",
                table: "scrape_runs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_service_add_ons_IsActive",
                table: "service_add_ons",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_service_add_ons_ServiceId",
                table: "service_add_ons",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_service_area_services_ServiceAreaId_ServiceId",
                table: "service_area_services",
                columns: new[] { "ServiceAreaId", "ServiceId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_area_services_ServiceId",
                table: "service_area_services",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_service_areas_CityId_ZoneId",
                table: "service_areas",
                columns: new[] { "CityId", "ZoneId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_areas_IsActive",
                table: "service_areas",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_service_areas_ZoneId",
                table: "service_areas",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_IsActive",
                table: "service_categories",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_Slug",
                table: "service_categories",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_categories_SortOrder",
                table: "service_categories",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_service_package_add_ons_AddOnId",
                table: "service_package_add_ons",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_service_package_add_ons_PackageId_AddOnId",
                table: "service_package_add_ons",
                columns: new[] { "PackageId", "AddOnId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_packages_IsActive",
                table: "service_packages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_service_packages_ServiceId",
                table: "service_packages",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_service_packages_ServiceId_Name",
                table: "service_packages",
                columns: new[] { "ServiceId", "Name" },
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_service_problems_ServiceId",
                table: "service_problems",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_service_warranties_ServiceId",
                table: "service_warranties",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_services_CategoryId",
                table: "services",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_services_IsActive",
                table: "services",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_services_IsEmergency",
                table: "services",
                column: "IsEmergency");

            migrationBuilder.CreateIndex(
                name: "IX_services_Slug",
                table: "services",
                column: "Slug",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_BookingId",
                table: "support_tickets",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_Priority",
                table: "support_tickets",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_RaisedBy",
                table: "support_tickets",
                column: "RaisedBy");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_Status",
                table: "support_tickets",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_support_tickets_TicketNumber",
                table: "support_tickets",
                column: "TicketNumber",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_RoleId",
                table: "user_roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_UserId_RoleId",
                table: "user_roles",
                columns: new[] { "UserId", "RoleId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_users_Phone",
                table: "users",
                column: "Phone",
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_users_Status",
                table: "users",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_zones_CityId_Name",
                table: "zones",
                columns: new[] { "CityId", "Name" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "amc_contracts");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "banners");

            migrationBuilder.DropTable(
                name: "booking_add_ons");

            migrationBuilder.DropTable(
                name: "booking_assignments");

            migrationBuilder.DropTable(
                name: "booking_items");

            migrationBuilder.DropTable(
                name: "booking_materials");

            migrationBuilder.DropTable(
                name: "booking_notes");

            migrationBuilder.DropTable(
                name: "booking_status_history");

            migrationBuilder.DropTable(
                name: "cms_pages");

            migrationBuilder.DropTable(
                name: "commission_rules");

            migrationBuilder.DropTable(
                name: "conversation_messages");

            migrationBuilder.DropTable(
                name: "coupon_redemptions");

            migrationBuilder.DropTable(
                name: "credit_transactions");

            migrationBuilder.DropTable(
                name: "customer_addresses");

            migrationBuilder.DropTable(
                name: "customer_memberships");

            migrationBuilder.DropTable(
                name: "disputes");

            migrationBuilder.DropTable(
                name: "duplicate_candidates");

            migrationBuilder.DropTable(
                name: "faqs");

            migrationBuilder.DropTable(
                name: "ingestion_errors");

            migrationBuilder.DropTable(
                name: "job_source_configs");

            migrationBuilder.DropTable(
                name: "job_source_mappings");

            migrationBuilder.DropTable(
                name: "job_sources");

            migrationBuilder.DropTable(
                name: "localities");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "payment_gateway_settings");

            migrationBuilder.DropTable(
                name: "payment_gateway_webhook_events");

            migrationBuilder.DropTable(
                name: "payouts");

            migrationBuilder.DropTable(
                name: "pincodes");

            migrationBuilder.DropTable(
                name: "price_rules");

            migrationBuilder.DropTable(
                name: "professional_adjustments");

            migrationBuilder.DropTable(
                name: "professional_availabilities");

            migrationBuilder.DropTable(
                name: "professional_documents");

            migrationBuilder.DropTable(
                name: "professional_earnings");

            migrationBuilder.DropTable(
                name: "professional_incentives");

            migrationBuilder.DropTable(
                name: "professional_performances");

            migrationBuilder.DropTable(
                name: "professional_service_areas");

            migrationBuilder.DropTable(
                name: "professional_skills");

            migrationBuilder.DropTable(
                name: "professional_time_offs");

            migrationBuilder.DropTable(
                name: "quote_revisions");

            migrationBuilder.DropTable(
                name: "raw_external_jobs");

            migrationBuilder.DropTable(
                name: "recurring_bookings");

            migrationBuilder.DropTable(
                name: "referrals");

            migrationBuilder.DropTable(
                name: "refunds");

            migrationBuilder.DropTable(
                name: "review_media");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "scrape_logs");

            migrationBuilder.DropTable(
                name: "scrape_runs");

            migrationBuilder.DropTable(
                name: "service_area_services");

            migrationBuilder.DropTable(
                name: "service_package_add_ons");

            migrationBuilder.DropTable(
                name: "service_problems");

            migrationBuilder.DropTable(
                name: "service_warranties");

            migrationBuilder.DropTable(
                name: "support_tickets");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "membership_plans");

            migrationBuilder.DropTable(
                name: "price_quotes");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "home_service_reviews");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "service_areas");

            migrationBuilder.DropTable(
                name: "service_add_ons");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "home_service_bookings");

            migrationBuilder.DropTable(
                name: "professionals");

            migrationBuilder.DropTable(
                name: "zones");

            migrationBuilder.DropTable(
                name: "home_service_customers");

            migrationBuilder.DropTable(
                name: "service_packages");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "service_categories");

            migrationBuilder.DropIndex(
                name: "IX_jobs_CanonicalFingerprint",
                table: "jobs");

            migrationBuilder.DropIndex(
                name: "IX_jobs_PrimaryJobSourceId",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "ApplicationMode",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "BenefitsJson",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "CanonicalFingerprint",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "City",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "CompanyInitials",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "ExperienceText",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "ExternalApplyUrl",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "ExternalJobId",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Featured",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Industry",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "IsAggregated",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "LastSeenAtSource",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "MaxExperience",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "MinExperience",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "OriginalSourceUrl",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "PostedAtSource",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "PrimaryJobSourceId",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "ResponsibilitiesJson",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "SalaryText",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "SalaryVisible",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "SkillsJson",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "State",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "WorkMode",
                table: "jobs");
        }
    }
}
