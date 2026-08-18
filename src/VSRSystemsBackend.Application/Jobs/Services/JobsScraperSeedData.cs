using System.Text.Json;
using VSRSystemsBackend.Domain.Jobs;

namespace VSRSystemsBackend.Application.Jobs.Services;

internal sealed class DemoJob
{
    public required string Slug { get; init; }
    public required string Title { get; init; }
    public required string CompanyName { get; init; }
    public required string CompanyInitials { get; init; }
    public required string Industry { get; init; }
    public required string Location { get; init; }
    public string ExperienceText { get; init; } = "";
    public string SalaryText { get; init; } = "";
    public string WorkMode { get; init; } = "On-site";
    public string EmploymentType { get; init; } = "Full-time";
    public string[] Skills { get; init; } = [];
    public string Summary { get; init; } = "";
    public string[] Responsibilities { get; init; } = [];
    public string[] Requirements { get; init; } = [];
    public string[] Benefits { get; init; } = [];
    public string ApplicationMode { get; init; } = "EasyApply";
    public string? ExternalApplyUrl { get; init; }
    public bool Featured { get; init; }
    public bool Verified { get; init; }
    public string PublishedAt { get; init; } = "";
}

internal static class JobsScraperSeedData
{
    public static readonly (string Name, string Slug, string Industry, string Size, string About)[] Companies =
    [
        ("Northstar Digital", "northstar-digital", "Software Products", "501-1,000 employees", "Northstar Digital builds secure workflow platforms for finance, retail and logistics teams operating at national scale."),
        ("Atlas Commerce", "atlas-commerce", "Retail Technology", "201-500 employees", "Atlas Commerce gives growing retailers practical tools for storefronts, inventory, payments and customer operations."),
        ("CloudCraft Systems", "cloudcraft-systems", "Cloud Services", "201-500 employees", "CloudCraft helps engineering organisations run dependable cloud platforms with strong automation and observability."),
        ("Saarthi Mobility", "saarthi-mobility", "Mobility", "1,001-5,000 employees", "Saarthi Mobility operates connected transport networks and builds data products for safer, more predictable journeys."),
        ("LedgerLane", "ledgerlane", "Financial Technology", "51-200 employees", "LedgerLane simplifies payment reconciliation and cash visibility for growing finance teams."),
        ("Veridian Health", "veridian-health", "Health Technology", "501-1,000 employees", "Veridian Health creates dependable digital tools for clinicians, care teams and patients."),
        ("Kite Labs", "kite-labs", "Developer Tools", "51-200 employees", "Kite Labs builds developer tools that make shipping software faster and more approachable."),
        ("StudioField", "studiofield", "Design Services", "11-50 employees", "StudioField is a product design studio helping software teams research, prototype and polish."),
        ("Matrix AI", "matrix-ai", "Artificial Intelligence", "201-500 employees", "Matrix AI applies machine learning to logistics planning and forecasting for large retail networks."),
        ("PixelWorks", "pixelworks", "Software Products", "51-200 employees", "PixelWorks crafts cross-platform mobile products for consumer teams in India and SE Asia."),
        ("InfoLytics", "infolytics", "Data & Analytics", "201-500 employees", "InfoLytics delivers data engineering and analytics platforms for mid-market enterprises."),
        ("NovaBank", "novabank", "Financial Technology", "1,001-5,000 employees", "NovaBank is a digital-first bank building modern lending and payments infrastructure."),
    ];

    public static readonly (string Name, string Slug, string SourceType, string AdapterKey, string FeedUrl, int Interval, bool Enabled, bool Authorized, string Notes, string Health)[] Sources =
    [
        ("VSR Demo Careers Feed", "vsr-demo-careers", "JsonFeed", "Fixture", "fixture://vsr-demo-careers", 120, true, true, "Built-in demo fixture (permitted local data)", JobSourceHealth.Healthy),
        ("VSR Startup Jobs Feed", "vsr-startup-jobs", "JsonFeed", "Fixture", "fixture://vsr-startup-jobs", 120, true, true, "Built-in demo fixture (permitted local data)", JobSourceHealth.Healthy),
        ("Northstar Careers (Fixture)", "northstar-fixture", "AtsPublicEndpoint", "Fixture", "fixture://northstar-fixture", 60, false, true, "Demo fixture mirror of Northstar career site", JobSourceHealth.Paused),
        ("Atlas Commerce ATS (Fixture)", "atlas-ats-fixture", "AtsPublicEndpoint", "Fixture", "fixture://atlas-ats-fixture", 60, false, true, "Demo fixture mirror of Atlas ATS JSON endpoint", JobSourceHealth.Paused),
        ("CloudCraft API (Fixture)", "cloudcraft-api-fixture", "Api", "Fixture", "fixture://cloudcraft-api-fixture", 90, false, true, "Demo fixture mirror of CloudCraft public API", JobSourceHealth.Paused),
        ("LedgerLane RSS (Fixture)", "ledgerlane-rss-fixture", "Rss", "Fixture", "fixture://ledgerlane-rss-fixture", 180, false, true, "Demo fixture mirror of LedgerLane RSS feed", JobSourceHealth.Paused),
        ("Veridian XML Feed (Fixture)", "veridian-xml-fixture", "XmlFeed", "Fixture", "fixture://veridian-xml-fixture", 180, false, true, "Demo fixture mirror of Veridian XML feed", JobSourceHealth.Paused),
        ("Saarthi Sitemap (Fixture)", "saarthi-sitemap-fixture", "Sitemap", "Fixture", "fixture://saarthi-sitemap-fixture", 360, false, true, "Demo fixture mirror of Saarthi careers sitemap", JobSourceHealth.Paused),
        ("StudioField HTML Page (Fixture)", "studiofield-html-fixture", "HtmlCareerPage", "Fixture", "fixture://studiofield-html-fixture", 240, false, true, "Demo fixture mirror of StudioField career page (authorized)", JobSourceHealth.Paused),
        ("TechDigest Partner Feed (Fixture)", "techdigest-partner", "PartnerFeed", "Fixture", "fixture://techdigest-partner", 60, false, false, "Partner feed awaiting written permission", JobSourceHealth.Disabled),
    ];

    public static readonly (string Name, string[] Aliases)[] SkillCache =
    [
        ("C#", ["C#", "C Sharp"]), (".NET", [".NET", "dotnet", ".NET Core", "ASP.NET Core"]),
        ("ASP.NET Core", ["ASP.NET Core", "ASP.NET"]), ("SQL Server", ["SQL Server", "MSSQL"]),
        ("Azure", ["Azure"]), ("React", ["React", "ReactJS", "React.js"]),
        ("TypeScript", ["TypeScript"]), ("Vite", ["Vite"]), ("SQL", ["SQL"]),
        ("Power BI", ["Power BI", "PowerBI"]), ("Python", ["Python"]), ("Statistics", ["Statistics", "Stats"]),
        ("Kubernetes", ["Kubernetes", "K8s"]), ("Terraform", ["Terraform"]), ("Docker", ["Docker"]),
        ("GitHub Actions", ["GitHub Actions", "GH Actions"]), ("CI/CD", ["CI/CD", "CICD"]),
        ("Playwright", ["Playwright"]), ("API Testing", ["API Testing", "API test"]),
        ("JavaScript", ["JavaScript", "JS"]), ("Git", ["Git"]), ("Problem Solving", ["Problem Solving", "DSA", "Data Structures"]),
        ("Figma", ["Figma"]), ("User Research", ["User Research", "UX Research"]),
        ("Prototyping", ["Prototyping"]), ("Design Systems", ["Design Systems"]),
        ("React Native", ["React Native"]), ("Redux", ["Redux"]), ("REST", ["REST"]),
        ("Node.js", ["Node.js", "Node"]), ("AWS", ["AWS"]), ("PostgreSQL", ["PostgreSQL", "Postgres"]),
        ("Spark", ["Spark"]), ("Airflow", ["Airflow"]), ("Product Strategy", ["Product Strategy"]),
        ("Payments", ["Payments"]), ("Analytics", ["Analytics"]), ("B2B SaaS", ["B2B SaaS", "SaaS"]),
        ("System Design", ["System Design", "Architecture"]), ("Java", ["Java"]),
        ("Spring Boot", ["Spring Boot", "Spring"]), ("Go", ["Go", "Golang"]),
        ("Microservices", ["Microservices"]), ("MongoDB", ["MongoDB"]),
        ("Testing", ["Testing", "Automated testing"]), ("Accessibility", ["Accessibility", "a11y", "WCAG"]),
    ];

    public static List<DemoJob> DemoJobs()
    {
        return
        [
            new() { Slug = "senior-dotnet-developer-northstar", Title = "Senior .NET Developer", CompanyName = "Northstar Digital", CompanyInitials = "ND", Industry = "Software Products", Location = "Gurugram", ExperienceText = "4-7 years", SalaryText = "18-25 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["C#", "ASP.NET Core", "SQL Server", "Azure", "React"], Summary = "Build secure, high-volume business platforms with a product engineering team focused on measurable customer outcomes.", Responsibilities = ["Design and ship ASP.NET Core services", "Review architecture and production performance", "Mentor engineers through delivery"], Requirements = ["Strong C# and .NET experience", "Production SQL Server knowledge", "Experience with cloud delivery and automated testing"], Benefits = ["Flexible hybrid schedule", "Learning budget", "Comprehensive health cover"], Featured = true, Verified = true, PublishedAt = "2026-08-18T04:00:00Z" },
            new() { Slug = "frontend-engineer-atlas", Title = "Frontend Engineer - React", CompanyName = "Atlas Commerce", CompanyInitials = "AC", Industry = "Retail Technology", Location = "Bengaluru", ExperienceText = "3-6 years", SalaryText = "16-22 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["React", "TypeScript", "Vite", "Accessibility", "Testing"], Summary = "Create fast, accessible merchant experiences used by growing retail teams across India.", Responsibilities = ["Own React features from discovery to release", "Improve design-system primitives", "Measure and improve runtime performance"], Requirements = ["Advanced React and TypeScript", "Strong CSS and accessibility fundamentals", "Experience testing user-facing applications"], Benefits = ["Flexible hours", "Home-office allowance", "Quarterly learning days"], Verified = true, PublishedAt = "2026-08-18T02:00:00Z" },
            new() { Slug = "data-analyst-saarthi", Title = "Data Analyst", CompanyName = "Saarthi Mobility", CompanyInitials = "SM", Industry = "Mobility", Location = "Pune", ExperienceText = "2-4 years", SalaryText = "10-15 LPA", WorkMode = "On-site", EmploymentType = "Full-time", Skills = ["SQL", "Power BI", "Python", "Statistics"], Summary = "Turn operational and customer data into clear decisions for a rapidly expanding mobility network.", Responsibilities = ["Build decision-ready dashboards", "Investigate performance trends", "Partner with operations leaders"], Requirements = ["Strong SQL and Power BI", "Clear analytical communication", "Working knowledge of Python"], Benefits = ["Performance bonus", "Commuter support", "Health insurance"], PublishedAt = "2026-08-18T00:00:00Z" },
            new() { Slug = "devops-engineer-cloudcraft", Title = "DevOps Engineer", CompanyName = "CloudCraft Systems", CompanyInitials = "CS", Industry = "Cloud Services", Location = "Hyderabad", ExperienceText = "4-8 years", SalaryText = "20-28 LPA", WorkMode = "Remote", EmploymentType = "Full-time", Skills = ["Azure", "Kubernetes", "Terraform", "Docker", "GitHub Actions"], Summary = "Improve platform reliability and developer velocity across multi-region cloud environments.", Responsibilities = ["Operate Kubernetes platforms", "Automate infrastructure delivery", "Lead reliability reviews"], Requirements = ["Production Kubernetes experience", "Infrastructure-as-code expertise", "Strong incident response practice"], Benefits = ["Remote-first team", "Certification support", "On-call allowance"], Featured = true, Verified = true, ApplicationMode = "External", ExternalApplyUrl = "https://cloudcraft.example.com/careers/apply/devops-engineer", PublishedAt = "2026-08-17T12:00:00Z" },
            new() { Slug = "product-manager-ledgerlane", Title = "Product Manager - Fintech", CompanyName = "LedgerLane", CompanyInitials = "LL", Industry = "Financial Technology", Location = "Mumbai", ExperienceText = "5-8 years", SalaryText = "24-32 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["Product Strategy", "Payments", "Analytics", "B2B SaaS"], Summary = "Lead payment and reconciliation workflows for finance teams managing complex business operations.", Responsibilities = ["Set measurable product outcomes", "Prioritise customer and compliance needs", "Coordinate design and engineering delivery"], Requirements = ["B2B product management experience", "Understanding of payment systems", "Strong written product communication"], Benefits = ["Employee stock options", "Flexible leave", "Wellness allowance"], Verified = true, ApplicationMode = "External", ExternalApplyUrl = "https://ledgerlane.example.com/careers/pm-fintech", PublishedAt = "2026-08-17T08:00:00Z" },
            new() { Slug = "qa-automation-engineer-veridian", Title = "QA Automation Engineer", CompanyName = "Veridian Health", CompanyInitials = "VH", Industry = "Health Technology", Location = "Chennai", ExperienceText = "3-5 years", SalaryText = "12-18 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["Playwright", "TypeScript", "API Testing", "CI/CD"], Summary = "Build reliable automated quality gates for patient and clinician applications.", Responsibilities = ["Develop browser and API test suites", "Improve release confidence metrics", "Partner with engineers on testability"], Requirements = ["Hands-on Playwright experience", "Strong API testing fundamentals", "Comfort with CI pipelines"], Benefits = ["Medical cover", "Flexible work week", "Conference allowance"], PublishedAt = "2026-08-17T06:00:00Z" },
            new() { Slug = "graduate-software-engineer-kite", Title = "Graduate Software Engineer", CompanyName = "Kite Labs", CompanyInitials = "KL", Industry = "Developer Tools", Location = "Noida", ExperienceText = "0-1 years", SalaryText = "6-9 LPA", WorkMode = "On-site", EmploymentType = "Full-time", Skills = ["JavaScript", "TypeScript", "Git", "Problem Solving"], Summary = "Join a structured engineering programme covering frontend, backend and cloud delivery fundamentals.", Responsibilities = ["Ship scoped product improvements", "Participate in code reviews", "Learn production support practices"], Requirements = ["Computer science fundamentals", "One completed software project", "Clear learning mindset"], Benefits = ["Six-month mentorship", "Certification budget", "Relocation support"], Verified = true, PublishedAt = "2026-08-16T12:00:00Z" },
            new() { Slug = "ux-design-intern-studiofield", Title = "Product Design Intern", CompanyName = "StudioField", CompanyInitials = "SF", Industry = "Design Services", Location = "Remote - India", ExperienceText = "0 years", SalaryText = "30,000/month", WorkMode = "Remote", EmploymentType = "Internship", Skills = ["Figma", "User Research", "Prototyping", "Design Systems"], Summary = "Support research, interaction design and prototyping for practical business software.", Responsibilities = ["Prepare prototypes and user flows", "Document research findings", "Contribute to shared design components"], Requirements = ["A focused design portfolio", "Strong visual hierarchy", "Comfort receiving detailed critique"], Benefits = ["Remote schedule", "Senior designer mentorship", "Completion certificate"], PublishedAt = "2026-08-16T08:00:00Z" },
            new() { Slug = "backend-engineer-matrix-ai", Title = "Backend Engineer - .NET", CompanyName = "Matrix AI", CompanyInitials = "MA", Industry = "Artificial Intelligence", Location = "Bengaluru", ExperienceText = "3-6 years", SalaryText = "20-28 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["C#", ".NET", "Azure", "PostgreSQL", "Microservices"], Summary = "Build high-throughput scoring and logistics APIs powering AI planning for retail networks.", Responsibilities = ["Design resilient backend services", "Own data pipelines end to end", "Collaborate with ML engineers"], Requirements = ["Strong C#/.NET with async patterns", "Production PostgreSQL experience", "Microservices and event-driven design"], Benefits = ["Equity options", "Hybrid workspace", "Learning budget"], Verified = true, ApplicationMode = "External", ExternalApplyUrl = "https://matrixai.example.com/jobs/backend-engineer", PublishedAt = "2026-08-16T04:00:00Z" },
            new() { Slug = "react-native-developer-pixelworks", Title = "React Native Developer", CompanyName = "PixelWorks", CompanyInitials = "PW", Industry = "Software Products", Location = "Gurugram", ExperienceText = "2-5 years", SalaryText = "12-18 LPA", WorkMode = "On-site", EmploymentType = "Full-time", Skills = ["React Native", "TypeScript", "Redux", "REST"], Summary = "Ship polished cross-platform mobile products for consumer teams across India and SE Asia.", Responsibilities = ["Own mobile feature delivery", "Improve app performance", "Work closely with product design"], Requirements = ["Production React Native experience", "Strong TypeScript fundamentals", "REST and offline-first design"], Benefits = ["Flexible hours", "Snacks and meals", "Health cover"], PublishedAt = "2026-08-15T10:00:00Z" },
            new() { Slug = "data-engineer-infolytics", Title = "Data Engineer", CompanyName = "InfoLytics", CompanyInitials = "IL", Industry = "Data & Analytics", Location = "Pune", ExperienceText = "4-7 years", SalaryText = "18-26 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["Python", "Spark", "Airflow", "SQL"], Summary = "Design scalable data platforms and pipelines for mid-market enterprises.", Responsibilities = ["Build ingestion and transformation pipelines", "Own warehouse modelling", "Partner with analytics teams"], Requirements = ["Strong Python and SQL", "Spark and Airflow in production", "Data warehouse modelling experience"], Benefits = ["Certification budget", "Hybrid schedule", "Wellness allowance"], Verified = true, ApplicationMode = "External", ExternalApplyUrl = "https://infolytics.example.com/careers/data-engineer", PublishedAt = "2026-08-15T06:00:00Z" },
            new() { Slug = "fullstack-developer-novabank", Title = "Fullstack Developer", CompanyName = "NovaBank", CompanyInitials = "NB", Industry = "Financial Technology", Location = "Mumbai", ExperienceText = "5-8 years", SalaryText = "25-35 LPA", WorkMode = "Hybrid", EmploymentType = "Full-time", Skills = ["React", "Node.js", "TypeScript", "AWS"], Summary = "Build customer-facing lending and payments experiences on modern cloud infrastructure.", Responsibilities = ["Ship fullstack banking features", "Ensure compliance and auditability", "Mentor mid-level engineers"], Requirements = ["Fullstack React + Node.js experience", "AWS production expertise", "Banking or payments domain knowledge"], Benefits = ["ESOPs", "Premium health cover", "Hybrid workspace"], Featured = true, Verified = true, ApplicationMode = "External", ExternalApplyUrl = "https://novabank.example.com/careers/fullstack", PublishedAt = "2026-08-14T12:00:00Z" },
        ];
    }

    public static List<DemoJob> DemoJobsForSource(string sourceSlug)
    {
        var jobs = DemoJobs();
        return sourceSlug switch
        {
            "vsr-demo-careers" => jobs.Where(j => j.CompanyName is "Northstar Digital" or "Atlas Commerce" or "Saarthi Mobility" or "CloudCraft Systems").ToList(),
            "vsr-startup-jobs" => jobs.Where(j => j.CompanyName is "LedgerLane" or "Veridian Health" or "Kite Labs" or "StudioField" or "Matrix AI" or "PixelWorks" or "InfoLytics" or "NovaBank").ToList(),
            "northstar-fixture" => jobs.Where(j => j.CompanyName == "Northstar Digital").ToList(),
            "atlas-ats-fixture" => jobs.Where(j => j.CompanyName == "Atlas Commerce").ToList(),
            "cloudcraft-api-fixture" => jobs.Where(j => j.CompanyName == "CloudCraft Systems").ToList(),
            "ledgerlane-rss-fixture" => jobs.Where(j => j.CompanyName == "LedgerLane").ToList(),
            "veridian-xml-fixture" => jobs.Where(j => j.CompanyName == "Veridian Health").ToList(),
            "saarthi-sitemap-fixture" => jobs.Where(j => j.CompanyName == "Saarthi Mobility").ToList(),
            "studiofield-html-fixture" => jobs.Where(j => j.CompanyName == "StudioField").ToList(),
            "techdigest-partner" => jobs.Where(j => j.CompanyName is "Matrix AI" or "PixelWorks" or "InfoLytics" or "NovaBank").ToList(),
            _ => jobs.Take(3).ToList(),
        };
    }

    public static string RawPayload(DemoJob j)
    {
        return JsonSerializer.Serialize(new
        {
            j.Slug, j.Title, j.CompanyName, j.Location, j.Skills, j.Summary, j.SalaryText, j.WorkMode, j.EmploymentType, j.PublishedAt,
        });
    }
}