using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Application.HomeServices.Services;

/// <summary>
/// Deterministic seed data for the Home Services marketplace (mirrors the JobsScraperSeedData pattern).
/// Generates the full §152/§153 dataset: 5 cities, 20+ categories, 100+ services, 200+ packages,
/// 80+ add-ons, 150 professionals, 500 customers, 500 bookings, 200 reviews, 30 coupons, membership plans.
/// Pure data generation - no DbContext dependency; persistence lives in Infrastructure (HomeServicesSeeder).
/// </summary>
public sealed record HomeServicesSeedBundle(
    List<City> Cities,
    List<Zone> Zones,
    List<Locality> Localities,
    List<Pincode> Pincodes,
    List<ServiceArea> ServiceAreas,
    List<ServiceAreaService> ServiceAreaServices,
    List<ServiceCategory> Categories,
    List<Service> Services,
    List<ServiceProblem> Problems,
    List<ServicePackage> Packages,
    List<ServiceAddOn> AddOns,
    List<ServicePackageAddOn> PackageAddOns,
    List<ServiceWarranty> Warranties,
    List<Role> Roles,
    List<Permission> Permissions,
    List<RolePermission> RolePermissions,
    List<MembershipPlan> MembershipPlans,
    List<User> Users,
    List<UserRole> UserRoles,
    List<Customer> Customers,
    List<CustomerAddress> CustomerAddresses,
    List<CustomerMembership> CustomerMemberships,
    List<Professional> Professionals,
    List<ProfessionalDocument> ProfessionalDocuments,
    List<ProfessionalSkill> ProfessionalSkills,
    List<ProfessionalServiceArea> ProfessionalServiceAreas,
    List<ProfessionalAvailability> ProfessionalAvailabilities,
    List<ProfessionalPerformance> ProfessionalPerformances,
    List<PriceRule> PriceRules,
    List<CommissionRule> CommissionRules,
    List<Coupon> Coupons,
    List<Booking> Bookings,
    List<BookingItem> BookingItems,
    List<BookingAddOn> BookingAddOns,
    List<BookingMaterial> BookingMaterials,
    List<BookingAssignment> BookingAssignments,
    List<BookingStatusHistory> BookingStatusHistories,
    List<BookingNote> BookingNotes,
    List<PriceQuote> PriceQuotes,
    List<QuoteRevision> QuoteRevisions,
    List<Payment> Payments,
    List<Refund> Refunds,
    List<CreditTransaction> CreditTransactions,
    List<ProfessionalEarning> ProfessionalEarnings,
    List<Payout> Payouts,
    List<CouponRedemption> CouponRedemptions,
    List<Review> Reviews,
    List<ReviewMedia> ReviewMediaItems,
    List<RecurringBooking> RecurringBookings,
    List<AmcContract> AmcContracts,
    List<SupportTicket> SupportTickets,
    List<Dispute> Disputes,
    List<Notification> Notifications,
    List<CmsPage> CmsPages,
    List<Banner> Banners,
    List<Faq> Faqs,
    List<AuditLog> AuditLogs,
    List<PaymentGatewaySetting> PaymentGatewaySettings,
    List<ProfessionalAdjustment> ProfessionalAdjustments,
    List<ProfessionalIncentive> ProfessionalIncentives);

public static class HomeServicesSeedData
{
    private static readonly Random Rng = new(20260818);

    private static string Slugify(string value) =>
        string.Join("-", value.ToLowerInvariant()
            .Split([' ', '&', ',', '/', '-'], StringSplitOptions.RemoveEmptyEntries)
            .Select(w => new string(w.Where(char.IsLetterOrDigit).ToArray())));

    private static readonly string[] FirstNames =
    [
        "Aarav", "Vivaan", "Aditya", "Vihaan", "Arjun", "Rohan", "Kabir", "Ayaan", "Ishaan", "Shaurya",
        "Dhruv", "Yash", "Reyansh", "Aryan", "Karthik", "Dev", "Om", "Aarav", "Sai", "Krishna",
        "Ananya", "Diya", "Aadhya", "Ira", "Saanvi", "Aarohi", "Myra", "Anika", "Navya", "Aisha",
        "Kiara", "Riya", "Tanvi", "Ishita", "Sara", "Aanya", "Kavya", "Meera", "Lakshmi", "Gita",
        "Sunita", "Rekha", "Priya", "Neha", "Pooja", "Kavita", "Sneha", "Ritu", "Anjali", "Shalini",
    ];

    private static readonly string[] LastNames =
    [
        "Sharma", "Verma", "Gupta", "Mehta", "Singh", "Kumar", "Patel", "Reddy", "Nair", "Iyer",
        "Das", "Chowdhury", "Mishra", "Tripathi", "Jain", "Agarwal", "Malhotra", "Kapoor", "Chopra", "Khanna",
        "Bhat", "Rao", "Joshi", "Kulkarni", "Deshmukh", "Yadav", "Chauhan", "Tiwari", "Pandey", "Dubey",
    ];

    private static readonly (string City, (string Zone, (string Locality, string Pincode)[])[] Areas)[] Geo =
    [
        ("Mumbai", [
            ("South Mumbai", [("Colaba", "400005"), ("Fort", "400001"), ("Marine Lines", "400020"), ("Worli", "400018")]),
            ("Western Suburbs", [("Bandra West", "400050"), ("Andheri East", "400069"), ("Powai", "400076"), ("Borivali", "400092")]),
            ("Eastern Suburbs", [("Chembur", "400071"), ("Ghatkopar", "400086"), ("Mulund", "400080")]),
            ("Navi Mumbai", [("Vashi", "400703"), ("Nerul", "400706"), ("Kharghar", "410210")]),
        ]),
        ("Delhi", [
            ("Central Delhi", [("Connaught Place", "110001"), ("Karol Bagh", "110005"), ("Chandni Chowk", "110006")]),
            ("South Delhi", [("Saket", "110017"), ("Hauz Khas", "110016"), ("Greater Kailash", "110048")]),
            ("West Delhi", [("Dwarka", "110075"), ("Janakpuri", "110058"), ("Rajouri Garden", "110027")]),
            ("Noida Region", [("Sector 18", "201301"), ("Sector 62", "201309"), ("Greater Noida", "201310")]),
        ]),
        ("Bengaluru", [
            ("Central", [("MG Road", "560001"), ("Shivajinagar", "560001"), ("Cubbon Park", "560001")]),
            ("South", [("Koramangala", "560034"), ("HSR Layout", "560102"), ("JP Nagar", "560078")]),
            ("East", [("Indiranagar", "560038"), ("Whitefield", "560066"), ("Marathahalli", "560037")]),
        ]),
        ("Hyderabad", [
            ("Central", [("Banjara Hills", "500034"), ("Jubilee Hills", "500033"), ("Secunderabad", "500003")]),
            ("West", [("Kukatpally", "500072"), ("Miyapur", "500049"), ("Madhapur", "500081")]),
            ("Tech Corridor", [("Gachibowli", "500032"), ("Kondapur", "500084"), ("HITEC City", "500081")]),
        ]),
        ("Pune", [
            ("Central", [("Kothrud", "411038"), ("Deccan", "411004"), ("Sadashiv Peth", "411030")]),
            ("West", [("Baner", "411045"), ("Aundh", "411007"), ("Pashan", "411021")]),
            ("IT Corridor", [("Hinjewadi", "411057"), ("Viman Nagar", "411014"), ("Hadapsar", "411028")]),
        ]),
    ];

    private static readonly (string Category, (string Service, bool Emergency, bool Inspection)[] Services)[] Catalog =
    [
        ("Electrical", [
            ("Fan Repair & Replacement", false, false), ("Light & Switch Installation", false, false),
            ("Wiring & Rewiring", false, true), ("Inverter Installation", false, true),
            ("MCB & Fuse Replacement", true, false), ("Home Electrical Audit", false, true),
        ]),
        ("Plumbing", [
            ("Leakage Repair", true, false), ("Tap & Mixer Installation", false, false),
            ("Toilet & Sanitary Fitting", false, false), ("Drain Cleaning", true, false),
            ("Water Tank Cleaning", false, false), ("Pipe Installation & Replacement", false, true),
        ]),
        ("Carpentry", [
            ("Furniture Repair", false, false), ("Door & Window Repair", false, false),
            ("Wardrobe Installation", false, true), ("Modular Kitchen Installation", false, true),
            ("Wooden Flooring Repair", false, true), ("Custom Shelving", false, true),
        ]),
        ("Painting", [
            ("Full Home Painting", false, true), ("Single Room Painting", false, false),
            ("Wall Texture & Design", false, false), ("Waterproofing", false, true),
            ("POP & False Ceiling", false, true),
        ]),
        ("Deep Cleaning", [
            ("Full Home Deep Cleaning", false, false), ("Kitchen Deep Cleaning", false, false),
            ("Bathroom Deep Cleaning", false, false), ("Sofa & Carpet Cleaning", false, false),
            ("Car Interior Cleaning", false, false), ("AC Vent Cleaning", false, false),
        ]),
        ("Appliance Repair", [
            ("Washing Machine Repair", false, false), ("Refrigerator Repair", false, false),
            ("Microwave Repair", false, false), ("Chimney & Hob Repair", false, false),
            ("Dishwasher Repair", false, false), ("Geyser Repair", false, false),
        ]),
        ("AC Services", [
            ("AC Installation", false, true), ("AC Repair", true, false),
            ("AC Service & Gas Refill", false, false), ("AC Uninstallation", false, false),
        ]),
        ("Water Purifier", [
            ("RO Installation", false, false), ("RO Service & Filter Change", false, false),
            ("RO Repair", false, false),
        ]),
        ("Pest Control", [
            ("Cockroach Treatment", false, false), ("Termite Treatment", false, true),
            ("Bed Bug Treatment", false, false), ("Rodent Control", false, false),
            ("Mosquito Control", false, false),
        ]),
        ("Bathroom & Kitchen", [
            ("Shower & Bath Fitting", false, false), ("Kitchen Sink Installation", false, false),
            ("Bathroom Accessory Installation", false, false), ("Tiling & Grouting", false, true),
        ]),
        ("Furniture Assembly", [
            ("Wardrobe Assembly", false, false), ("Bed Assembly", false, false),
            ("Office Desk Assembly", false, false), ("Shelf Assembly", false, false),
        ]),
        ("Moving & Shifting", [
            ("Local Home Shifting", false, true), ("Office Shifting", false, true),
            ("Car Transportation", false, true),
        ]),
        ("Home Automation", [
            ("Smart Switch Installation", false, true), ("CCTV Installation", false, true),
            ("Video Doorbell Installation", false, false), ("Wi-Fi Router Setup", false, false),
        ]),
        ("Masonry", [
            ("Wall Repair", false, true), ("Brickwork", false, true), ("Plastering", false, true),
        ]),
        ("Tiling", [
            ("Floor Tiling", false, true), ("Wall Tiling", false, true), ("Tile Repair", false, false),
        ]),
        ("Roofing", [
            ("Roof Leakage Repair", true, true), ("Waterproof Coating", false, true),
        ]),
        ("Locksmith", [
            ("Lock Installation", false, false), ("Lock Repair", false, false),
            ("Emergency Lockout Assistance", true, false),
        ]),
        ("Curtains & Blinds", [
            ("Curtain Installation", false, false), ("Blinds Installation", false, false),
        ]),
        ("Wallpaper", [
            ("Wallpaper Installation", false, true),
        ]),
        ("Gardening", [
            ("Lawn & Garden Maintenance", false, false), ("Balcony Garden Setup", false, false),
            ("Tree Trimming", false, true),
        ]),
        ("Solar Solutions", [
            ("Solar Panel Installation", false, true), ("Solar Panel Cleaning", false, false),
            ("Solar Inverter Repair", false, true),
        ]),
    ];

    private static readonly (string Code, string DiscountType, decimal Value, decimal MaxDiscount, decimal MinOrder, int UsageLimit, int PerCustomerLimit, string? TargetType, string? TargetValue)[] CouponDefs =
    [
        ("WELCOME10", "percent", 10, 300, 999, 1000, 1, "first_booking", null),
        ("FIRST50", "flat", 50, 50, 499, 500, 1, "first_booking", null),
        ("HOMESAVE15", "percent", 15, 500, 1499, 500, 2, "generic", null),
        ("CLEAN20", "percent", 20, 400, 999, 300, 2, "category", "deep-cleaning"),
        ("ELECTRIC10", "percent", 10, 200, 599, 300, 3, "category", "electrical"),
        ("PLUMB25", "flat", 125, 125, 799, 200, 2, "category", "plumbing"),
        ("ACSUMMER", "percent", 15, 450, 1199, 400, 2, "category", "ac-services"),
        ("PAINT200", "flat", 200, 200, 1999, 200, 1, "category", "painting"),
        ("REPAIR10", "percent", 10, 250, 699, 400, 3, "category", "appliance-repair"),
        ("PEST30", "flat", 150, 150, 899, 200, 2, "category", "pest-control"),
        ("MUMBAI5", "percent", 5, 150, 599, 500, 3, "city", "mumbai"),
        ("DELHI5", "percent", 5, 150, 599, 500, 3, "city", "delhi"),
        ("BLR10", "percent", 10, 300, 899, 400, 2, "city", "bengaluru"),
        ("HYD10", "percent", 10, 300, 899, 400, 2, "city", "hyderabad"),
        ("PUNE10", "percent", 10, 300, 899, 400, 2, "city", "pune"),
        ("AMC15", "percent", 15, 500, 2499, 200, 1, "service", "ac-service-gas-refill"),
        ("RO20", "percent", 20, 250, 799, 200, 2, "service", "ro-service-filter-change"),
        ("WC200", "flat", 200, 200, 1999, 200, 1, "service", "washing-machine-repair"),
        ("REF150", "flat", 150, 150, 1499, 200, 1, "service", "refrigerator-repair"),
        ("MEMB10", "percent", 10, 350, 799, 300, 1, "membership", null),
        ("REFER50", "flat", 50, 50, 499, 500, 5, "referral", null),
        ("DIWALI25", "percent", 25, 600, 1999, 500, 2, "generic", null),
        ("MONSOON15", "percent", 15, 400, 999, 500, 2, "generic", null),
        ("GEEKS10", "flat", 100, 100, 999, 300, 2, "category", "home-automation"),
        ("MOVING10", "percent", 10, 500, 2999, 200, 1, "category", "moving-shifting"),
        ("FURN10", "percent", 10, 200, 899, 300, 2, "category", "furniture-assembly"),
        ("KITCHEN15", "percent", 15, 300, 999, 300, 2, "category", "bathroom-kitchen"),
        ("CARPENT10", "percent", 10, 200, 699, 300, 2, "category", "carpentry"),
        ("TILING15", "percent", 15, 400, 1499, 200, 1, "category", "tiling"),
        ("SOLAR10", "percent", 10, 500, 4999, 100, 1, "category", "solar-solutions"),
    ];

    public static HomeServicesSeedBundle Build()
    {
        var cities = new List<City>();
        var zones = new List<Zone>();
        var localities = new List<Locality>();
        var pincodes = new List<Pincode>();
        var serviceAreas = new List<ServiceArea>();
        var serviceAreaServices = new List<ServiceAreaService>();

        foreach (var (cityName, areaDefs) in Geo)
        {
            var cityId = "city-" + Slugify(cityName);
            cities.Add(new City { Id = cityId, Name = cityName, IsActive = true, LaunchedAt = DateTime.UtcNow.AddDays(-365) });
            foreach (var (zoneName, localityDefs) in areaDefs)
            {
                var zoneId = $"zone-{Slugify(cityName)}-{Slugify(zoneName)}";
                zones.Add(new Zone { Id = zoneId, CityId = cityId, Name = zoneName });
                foreach (var (localityName, pincodeCode) in localityDefs)
                {
                    localities.Add(new Locality { Id = $"loc-{Slugify(zoneId)}-{Slugify(localityName)}", ZoneId = zoneId, Name = localityName, Pincode = pincodeCode });
                    if (!pincodes.Any(p => p.Code == pincodeCode))
                    {
                        pincodes.Add(new Pincode { Id = $"pin-{pincodeCode}", Code = pincodeCode, CityId = cityId, IsServiceable = true });
                    }
                }
                serviceAreas.Add(new ServiceArea { Id = $"area-{zoneId}", CityId = cityId, ZoneId = zoneId, IsActive = true });
            }
        }

        var categories = new List<ServiceCategory>();
        var services = new List<Service>();
        var problems = new List<ServiceProblem>();
        var packages = new List<ServicePackage>();
        var addOns = new List<ServiceAddOn>();
        var packageAddOns = new List<ServicePackageAddOn>();
        var warranties = new List<ServiceWarranty>();

        var catIndex = 0;
        foreach (var (categoryName, serviceDefs) in Catalog)
        {
            catIndex++;
            var categoryId = "cat-" + Slugify(categoryName);
            categories.Add(new ServiceCategory
            {
                Id = categoryId,
                Name = categoryName,
                Slug = Slugify(categoryName),
                Tagline = $"{categoryName} services at your doorstep",
                ImageUrl = $"/images/categories/{Slugify(categoryName)}.jpg",
                SortOrder = catIndex,
                IsActive = true,
            });

            var svcIndex = 0;
            foreach (var (serviceName, isEmergency, needsInspection) in serviceDefs)
            {
                svcIndex++;
                var serviceSlug = Slugify(serviceName);
                var serviceId = $"svc-{serviceSlug}";
                services.Add(new Service
                {
                    Id = serviceId,
                    CategoryId = categoryId,
                    Name = serviceName,
                    Slug = serviceSlug,
                    ShortDescription = $"Expert {serviceName.ToLowerInvariant()} with trained, verified professionals.",
                    LongDescription = $"Professional {serviceName.ToLowerInvariant()} delivered by background-verified experts. Transparent pricing, doorstep service, and a service warranty on eligible work.",
                    ImageUrl = $"/images/services/{serviceSlug}.jpg",
                    IsEmergency = isEmergency,
                    NeedsInspection = needsInspection,
                    InspectionFee = needsInspection ? 199 : 0,
                    IsActive = true,
                });

                problems.Add(new ServiceProblem { Id = $"prob-{serviceId}-1", ServiceId = serviceId, Name = $"General {serviceName}", Description = $"Standard {serviceName.ToLowerInvariant()} requirement", SortOrder = 1 });
                problems.Add(new ServiceProblem { Id = $"prob-{serviceId}-2", ServiceId = serviceId, Name = $"{serviceName} - Advanced", Description = $"Complex {serviceName.ToLowerInvariant()} requiring expert diagnosis", SortOrder = 2 });

                var basePrice = 199m + (Rng.Next(0, 600) / 5) * 5;
                var tiers = new[] { "Basic", "Standard", "Premium" };
                var tierCount = 2 + (svcIndex % 2 == 0 ? 1 : 0);
                for (var t = 0; t < tierCount; t++)
                {
                    var tierPrice = basePrice + t * (80 + (Rng.Next(0, 40) / 5) * 5);
                    var packageId = $"pkg-{serviceSlug}-{Slugify(tiers[t])}";
                    packages.Add(new ServicePackage
                    {
                        Id = packageId,
                        ServiceId = serviceId,
                        Name = tiers[t],
                        ShortDescription = $"{tiers[t]} package for {serviceName}",
                        DetailedDescription = $"{tiers[t]} tier: standard {serviceName.ToLowerInvariant()} scope with professional-grade tools and {tiers[t].ToLowerInvariant()} parts.",
                        BasePrice = tierPrice,
                        DurationMins = 60 + t * 30,
                        WhatIncluded = $"Standard service, basic consumables, {tiers[t].ToLowerInvariant()} workmanship",
                        WhatExcluded = "Premium replacement parts, structural repairs",
                        Warranty = t > 0 ? "30-day service warranty" : "7-day service warranty",
                        InspectionRequired = needsInspection,
                        PartsIncluded = t == 2,
                        MinimumCharge = Math.Min(tierPrice, 149),
                        CancellationRule = "Free cancellation up to 4 hours before the scheduled slot",
                        IsPopular = t == 1,
                        IsEmergencyEligible = isEmergency,
                        IsActive = true,
                    });
                }

                var addOnCount = 1 + (Rng.Next(0, 3));
                for (var a = 0; a < addOnCount; a++)
                {
                    var addOnName = a == 0 ? "Express 60-minute service" : a == 1 ? "Premium parts & consumables" : "Weekend slot priority";
                    var addOnId = $"addon-{serviceSlug}-{a + 1}";
                    addOns.Add(new ServiceAddOn
                    {
                        Id = addOnId,
                        ServiceId = serviceId,
                        Name = addOnName,
                        Price = 99m + a * 50,
                        DurationMins = a == 0 ? 30 : 0,
                        IsActive = true,
                    });
                    foreach (var pkg in packages.Where(p => p.ServiceId == serviceId && p.Id.EndsWith("-Premium")))
                    {
                        packageAddOns.Add(new ServicePackageAddOn { Id = $"po-{pkg.Id}-{a + 1}", PackageId = pkg.Id, AddOnId = addOnId });
                    }
                }

                warranties.Add(new ServiceWarranty { Id = $"wr-{serviceSlug}", ServiceId = serviceId, WarrantyDays = needsInspection ? 90 : 30, Terms = "Warranty covers workmanship defects; parts warranty as per manufacturer terms." });
            }
        }

        var allServices = services;
        var serviceBySlug = allServices.ToDictionary(s => s.Slug, s => s);
        var areaIds = serviceAreas.Select(a => a.Id).ToList();
        var sasCounter = 0;
        foreach (var areaId in areaIds)
        {
            foreach (var svc in allServices.Take(40))
            {
                sasCounter++;
                serviceAreaServices.Add(new ServiceAreaService { Id = $"sas-{sasCounter}", ServiceAreaId = areaId, ServiceId = svc.Id, IsActive = true });
            }
        }

        // Identity base: roles + permissions
        var roles = new List<Role>
        {
            new() { Id = "role-customer", Name = "customer", Description = "Home services customer" },
            new() { Id = "role-professional", Name = "professional", Description = "Verified service professional" },
            new() { Id = "role-ops-agent", Name = "ops_agent", Description = "Operations agent handling live bookings" },
            new() { Id = "role-support-agent", Name = "support_agent", Description = "Support agent for tickets & disputes" },
            new() { Id = "role-finance-agent", Name = "finance_agent", Description = "Finance agent for refunds & payouts" },
            new() { Id = "role-admin", Name = "admin", Description = "Platform administrator" },
        };
        var permissionDefs = new[]
        {
            ("catalog:read", "catalog"), ("catalog:write", "catalog"),
            ("booking:read", "booking"), ("booking:write", "booking"),
            ("professional:read", "professional"), ("professional:write", "professional"),
            ("payment:read", "payment"), ("payment:write", "payment"),
            ("earning:read", "earning"), ("payout:write", "payout"),
            ("analytics:read", "analytics"), ("review:read", "review"), ("review:write", "review"),
            ("support:read", "support"), ("support:write", "support"),
            ("finance:read", "finance"), ("finance:write", "finance"),
            ("admin:all", "admin"),
        };
        var permissions = permissionDefs.Select((p, i) => new Permission { Id = $"perm-{p.Item1}", Code = p.Item1, Area = p.Item2, Description = p.Item1 }).ToList();
        var rolePermissions = new List<RolePermission>();
        void Grant(string roleId, params string[] codes)
        {
            foreach (var code in codes)
            {
                rolePermissions.Add(new RolePermission { Id = $"rp-{roleId}-{code}", RoleId = roleId, PermissionId = $"perm-{code}" });
            }
        }
        Grant("role-customer", "catalog:read", "booking:read", "booking:write", "payment:read", "review:read", "review:write", "support:read", "support:write");
        Grant("role-professional", "catalog:read", "booking:read", "booking:write", "professional:read", "professional:write", "earning:read", "support:read", "support:write");
        Grant("role-ops-agent", "catalog:read", "booking:read", "booking:write", "professional:read", "support:read", "support:write");
        Grant("role-support-agent", "booking:read", "professional:read", "support:read", "support:write");
        Grant("role-finance-agent", "payment:read", "payment:write", "earning:read", "payout:write", "finance:read", "finance:write");
        Grant("role-admin", "admin:all", "catalog:read", "catalog:write", "booking:read", "booking:write", "professional:read", "professional:write", "payment:read", "payment:write", "earning:read", "payout:write", "analytics:read", "review:read", "review:write", "support:read", "support:write", "finance:read", "finance:write");

        // Membership plans
        var membershipPlans = new List<MembershipPlan>
        {
            new() { Id = "plan-silver", Name = "Silver", Price = 499, DurationDays = 365, BenefitsJson = """[{"label":"5% off every booking","maxDiscount":200},{"label":"Priority customer support"}]""" },
            new() { Id = "plan-gold", Name = "Gold", Price = 999, DurationDays = 365, BenefitsJson = """[{"label":"10% off every booking","maxDiscount":500},{"label":"Free annual AC service"},{"label":"Priority customer support"}]""" },
            new() { Id = "plan-platinum", Name = "Platinum", Price = 1999, DurationDays = 365, BenefitsJson = """[{"label":"15% off every booking","maxDiscount":1000},{"label":"Free quarterly deep clean"},{"label":"Free annual AC service"},{"label":"Dedicated relationship manager"}]""" },
        };

        // Users: 6 platform users + customers + professionals
        var users = new List<User>
        {
            new() { Id = "user-admin", Email = "admin.portal@vsrsystems.com", Phone = "9999999901", PasswordHash = "DEMO", FullName = "Platform Admin", Status = "active" },
            new() { Id = "user-ops", Email = "ops@vsrsystems.com", Phone = "9999999902", PasswordHash = "DEMO", FullName = "Ops Agent", Status = "active" },
            new() { Id = "user-support", Email = "support@vsrsystems.com", Phone = "9999999903", PasswordHash = "DEMO", FullName = "Support Agent", Status = "active" },
            new() { Id = "user-finance", Email = "finance@vsrsystems.com", Phone = "9999999904", PasswordHash = "DEMO", FullName = "Finance Agent", Status = "active" },
        };
        var userRoles = new List<UserRole>
        {
            new() { Id = "ur-admin", UserId = "user-admin", RoleId = "role-admin" },
            new() { Id = "ur-ops", UserId = "user-ops", RoleId = "role-ops-agent" },
            new() { Id = "ur-support", UserId = "user-support", RoleId = "role-support-agent" },
            new() { Id = "ur-finance", UserId = "user-finance", RoleId = "role-finance-agent" },
        };

        // Customers (500)
        var customers = new List<Customer>();
        var customerAddresses = new List<CustomerAddress>();
        var customerMemberships = new List<CustomerMembership>();
        var creditTransactions = new List<CreditTransaction>();

        for (var i = 0; i < 500; i++)
        {
            var userId = $"user-cust-{i + 1}";
            var customerId = $"cust-{i + 1}";
            var firstName = FirstNames[Rng.Next(FirstNames.Length)];
            var lastName = LastNames[Rng.Next(LastNames.Length)];
            var displayName = $"{firstName} {lastName}";
            var email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}{i + 1}@example.com";
            var phone = $"9{Rng.Next(100000000, 999999999)}";
            var city = cities[i % cities.Count];
            var zone = zones.First(z => z.CityId == city.Id);
            var locality = localities.First(l => l.ZoneId == zone.Id);

            users.Add(new User { Id = userId, Email = email, Phone = phone, PasswordHash = "DEMO", FullName = displayName, Status = "active", LastLoginAt = DateTime.UtcNow.AddDays(-Rng.Next(0, 30)) });
            userRoles.Add(new UserRole { Id = $"ur-{userId}", UserId = userId, RoleId = "role-customer" });

            var addressId = $"addr-{i + 1}";
            customers.Add(new Customer
            {
                Id = customerId,
                UserId = userId,
                DisplayName = displayName,
                DefaultAddressId = addressId,
                WalletBalance = Rng.Next(0, 20) == 0 ? Rng.Next(50, 500) : 0,
                ReferralCode = $"VSR{i + 1:D4}",
                Phone = phone,
                Email = email,
            });
            customerAddresses.Add(new CustomerAddress
            {
                Id = addressId,
                CustomerId = customerId,
                Label = i % 4 == 0 ? "office" : "home",
                Line1 = $"{Rng.Next(1, 999)}, {locality.Name}",
                Line2 = $"{zone.Name}, {city.Name}",
                CityId = city.Id,
                ZoneId = zone.Id,
                LocalityId = locality.Id,
                Pincode = locality.Pincode,
                Lat = 12.97 + Rng.NextDouble(),
                Lng = 77.59 + Rng.NextDouble(),
                IsDefault = true,
                ContactPerson = displayName,
                ContactPhone = phone,
            });

            if (Rng.Next(0, 5) == 0)
            {
                var plan = membershipPlans[Rng.Next(membershipPlans.Count)];
                customerMemberships.Add(new CustomerMembership
                {
                    Id = $"cm-{i + 1}",
                    CustomerId = customerId,
                    PlanId = plan.Id,
                    StartedAt = DateTime.UtcNow.AddDays(-Rng.Next(30, 300)),
                    ExpiresAt = DateTime.UtcNow.AddDays(Rng.Next(10, 300)),
                    Status = "active",
                });
                if (Rng.Next(0, 3) == 0)
                {
                    creditTransactions.Add(new CreditTransaction { Id = $"ct-{i + 1}", CustomerId = customerId, Amount = Rng.Next(50, 300), Type = "credit", Reason = "Membership welcome bonus", BalanceAfter = Rng.Next(50, 300) });
                }
            }
        }

        // Professionals (150)
        var professionals = new List<Professional>();
        var professionalDocuments = new List<ProfessionalDocument>();
        var professionalSkills = new List<ProfessionalSkill>();
        var professionalServiceAreas = new List<ProfessionalServiceArea>();
        var professionalAvailabilities = new List<ProfessionalAvailability>();
        var professionalPerformances = new List<ProfessionalPerformance>();

        for (var i = 0; i < 150; i++)
        {
            var professionalId = $"pro-{i + 1}";
            var userId = $"user-pro-{i + 1}";
            var firstName = FirstNames[Rng.Next(FirstNames.Length)];
            var lastName = LastNames[Rng.Next(LastNames.Length)];
            var displayName = $"{firstName} {lastName}";
            var phone = $"9{Rng.Next(100000000, 999999999)}";
            var city = cities[i % cities.Count];
            var zone = zones.First(z => z.CityId == city.Id);
            var skillsCount = 1 + Rng.Next(0, 4);
            var assignedServices = allServices.OrderBy(_ => Rng.Next()).Take(skillsCount).ToList();

            users.Add(new User { Id = userId, Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}.pro{i + 1}@vsrpro.example.com", Phone = phone, PasswordHash = "DEMO", FullName = displayName, Status = "active", LastLoginAt = DateTime.UtcNow.AddDays(-Rng.Next(0, 7)) });
            userRoles.Add(new UserRole { Id = $"ur-{userId}", UserId = userId, RoleId = "role-professional" });

            professionals.Add(new Professional
            {
                Id = professionalId,
                UserId = userId,
                DisplayName = displayName,
                Gender = i % 2 == 0 ? "Male" : "Female",
                Dob = DateTime.UtcNow.AddYears(-(25 + Rng.Next(0, 20))).AddDays(-Rng.Next(0, 300)),
                OnboardingStatus = Rng.Next(0, 20) == 0 ? "submitted" : "verified",
                QualityScore = Rng.Next(70, 99),
                Tier = new[] { "bronze", "silver", "gold", "platinum" }[Rng.Next(0, 4)],
                JoinedAt = DateTime.UtcNow.AddDays(-Rng.Next(30, 500)),
                Phone = phone,
                Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}.pro{i + 1}@vsrpro.example.com",
            });

            professionalDocuments.Add(new ProfessionalDocument { Id = $"pd-{professionalId}-1", ProfessionalId = professionalId, DocType = "id_proof", FileUrl = $"/docs/{professionalId}/aadhaar.jpg", Status = "approved", ReviewedBy = "user-ops", ReviewedAt = DateTime.UtcNow.AddDays(-20) });
            professionalDocuments.Add(new ProfessionalDocument { Id = $"pd-{professionalId}-2", ProfessionalId = professionalId, DocType = "address_proof", FileUrl = $"/docs/{professionalId}/address.jpg", Status = "approved", ReviewedBy = "user-ops", ReviewedAt = DateTime.UtcNow.AddDays(-20) });
            professionalDocuments.Add(new ProfessionalDocument { Id = $"pd-{professionalId}-3", ProfessionalId = professionalId, DocType = "police_verification", FileUrl = $"/docs/{professionalId}/police.jpg", Status = "approved", ReviewedBy = "user-ops", ReviewedAt = DateTime.UtcNow.AddDays(-15) });

            foreach (var svc in assignedServices)
            {
                professionalSkills.Add(new ProfessionalSkill { Id = $"psk-{professionalId}-{svc.Id}", ProfessionalId = professionalId, ServiceId = svc.Id, SkillLevel = new[] { "standard", "expert" }[Rng.Next(0, 2)] });
            }

            professionalServiceAreas.Add(new ProfessionalServiceArea { Id = $"psa-{professionalId}-1", ProfessionalId = professionalId, CityId = city.Id, ZoneId = zone.Id, IsActive = true });

            for (var d = 1; d <= 6; d++)
            {
                professionalAvailabilities.Add(new ProfessionalAvailability
                {
                    Id = $"pav-{professionalId}-{d}",
                    ProfessionalId = professionalId,
                    DayOfWeek = d,
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(19, 0, 0),
                    IsRecurring = true,
                });
            }

            var jobsCompleted = Rng.Next(10, 400);
            professionalPerformances.Add(new ProfessionalPerformance
            {
                Id = $"pperf-{professionalId}",
                ProfessionalId = professionalId,
                PeriodStart = DateTime.UtcNow.AddMonths(-1),
                PeriodEnd = DateTime.UtcNow,
                JobsCompleted = jobsCompleted,
                JobsCancelled = Rng.Next(0, Math.Max(1, jobsCompleted / 10)),
                AvgRating = 3.5 + (Rng.Next(0, 20) / 10.0),
                OnTimeRate = 85 + Rng.Next(0, 15),
                AcceptanceRate = 80 + Rng.Next(0, 20),
            });
        }

        // Pricing rules
        var priceRules = new List<PriceRule>();
        foreach (var svc in allServices.Take(20))
        {
            priceRules.Add(new PriceRule { Id = $"pr-{svc.Id}-summer", ServiceId = svc.Id, RuleType = "discount", Value = 10, ValidFrom = DateTime.UtcNow.AddDays(-30), ValidTo = DateTime.UtcNow.AddDays(60), IsActive = true });
        }
        var commissionRules = new List<CommissionRule>
        {
            new() { Id = "com-default", RatePercent = 20, FlatFee = 0, IsActive = true, ProfessionalTier = null },
            new() { Id = "com-silver", ProfessionalTier = "silver", RatePercent = 18, FlatFee = 0, IsActive = true },
            new() { Id = "com-gold", ProfessionalTier = "gold", RatePercent = 15, FlatFee = 0, IsActive = true },
            new() { Id = "com-platinum", ProfessionalTier = "platinum", RatePercent = 12, FlatFee = 0, IsActive = true },
        };

        // Coupons
        var coupons = CouponDefs.Select((c, i) => new Coupon
        {
            Id = $"cpn-{Slugify(c.Code)}",
            Code = c.Code,
            DiscountType = c.DiscountType,
            Value = c.Value,
            MaxDiscount = c.MaxDiscount,
            MinOrderValue = c.MinOrder,
            ValidFrom = DateTime.UtcNow.AddDays(-30),
            ValidTo = DateTime.UtcNow.AddDays(180),
            UsageLimit = c.UsageLimit,
            PerCustomerLimit = c.PerCustomerLimit,
            TargetType = c.TargetType,
            TargetValue = c.TargetValue,
            IsActive = true,
        }).ToList();

        // Bookings (500)
        var bookings = new List<Booking>();
        var bookingItems = new List<BookingItem>();
        var bookingAddOns = new List<BookingAddOn>();
        var bookingMaterials = new List<BookingMaterial>();
        var bookingAssignments = new List<BookingAssignment>();
        var bookingStatusHistories = new List<BookingStatusHistory>();
        var bookingNotes = new List<BookingNote>();
        var priceQuotes = new List<PriceQuote>();
        var quoteRevisions = new List<QuoteRevision>();
        var payments = new List<Payment>();
        var refunds = new List<Refund>();
        var professionalEarnings = new List<ProfessionalEarning>();
        var payouts = new List<Payout>();
        var couponRedemptions = new List<CouponRedemption>();
        var reviews = new List<Review>();
        var reviewMediaItems = new List<ReviewMedia>();
        var recurringBookings = new List<RecurringBooking>();
        var amcContracts = new List<AmcContract>();
        var supportTickets = new List<SupportTicket>();
        var disputes = new List<Dispute>();
        var notifications = new List<Notification>();
        var professionalAdjustments = new List<ProfessionalAdjustment>();
        var professionalIncentives = new List<ProfessionalIncentive>();

        var statusWeights = new[]
        {
            ("completed", 200), ("service_completed", 20), ("in_service", 25), ("assigned", 20),
            ("provider_accepted", 15), ("on_the_way", 10), ("arrived", 10), ("confirmed", 35),
            ("searching_provider", 20), ("awaiting_payment", 35), ("draft", 10),
            ("cancelled", 45), ("refund_pending", 15), ("refunded", 20), ("disputed", 10), ("closed", 10),
        };
        var statusPool = new List<string>();
        foreach (var (status, weight) in statusWeights)
        {
            for (var w = 0; w < weight; w++) statusPool.Add(status);
        }

        var invoiceCounter = 1;
        for (var i = 0; i < 500; i++)
        {
            var customer = customers[i % customers.Count];
            var address = customerAddresses.First(a => a.CustomerId == customer.Id);
            var svc = allServices[i % allServices.Count];
            var package = packages.First(p => p.ServiceId == svc.Id && p.Name == "Standard");
            var bookingId = $"bk-{i + 1}";
            var status = statusPool[Rng.Next(statusPool.Count)];
            var bookingNumber = $"VSR{i + 1:D6}";
            var createdAt = DateTime.UtcNow.AddDays(-Rng.Next(1, 180)).AddMinutes(-Rng.Next(0, 1200));

            var paid = status is "completed" or "service_completed" or "in_service" or "assigned" or "provider_accepted" or "on_the_way" or "arrived" or "confirmed" or "refund_pending" or "refunded" or "disputed" or "closed";
            var professional = professionals[(i * 7) % professionals.Count];

            bookings.Add(new Booking
            {
                Id = bookingId,
                BookingNumber = bookingNumber,
                CustomerId = customer.Id,
                AddressId = address.Id,
                ServiceId = svc.Id,
                PackageId = package.Id,
                BookingType = Rng.Next(0, 10) == 0 ? "emergency" : Rng.Next(0, 10) == 0 ? "recurring" : "scheduled",
                ScheduledStart = createdAt.AddDays(1 + Rng.Next(0, 30)).AddHours(Rng.Next(9, 19)),
                ExpectedEnd = createdAt.AddDays(1 + Rng.Next(0, 30)).AddHours(Rng.Next(10, 20)),
                Status = status,
                AssignedProfessionalId = status is "assigned" or "provider_accepted" or "on_the_way" or "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed" ? professional.Id : null,
                PaymentStatus = paid ? "paid" : status is "refunded" ? "refunded" : status is "refund_pending" ? "partial_refund" : "pending",
                CustomerNotes = Rng.Next(0, 3) == 0 ? "Please call before arriving." : string.Empty,
                OpsNotes = Rng.Next(0, 10) == 0 ? "Customer requested prior notification." : string.Empty,
                ActualStartAt = status is "in_service" or "service_completed" or "completed" or "disputed" or "closed" ? createdAt.AddDays(1 + Rng.Next(0, 30)) : null,
                ActualEndAt = status is "service_completed" or "completed" or "disputed" or "closed" ? createdAt.AddDays(1 + Rng.Next(0, 30)).AddHours(2) : null,
                CancelledAt = status is "cancelled" or "refund_pending" or "refunded" ? createdAt.AddDays(1 + Rng.Next(0, 10)) : null,
                CancelReason = status is "cancelled" or "refund_pending" or "refunded" ? (Rng.Next(0, 2) == 0 ? "Customer changed plans" : "Professional unavailable") : null,
                CreatedAt = createdAt,
            });

            bookingItems.Add(new BookingItem { Id = $"bi-{bookingId}-1", BookingId = bookingId, Description = package.Name + " package - " + svc.Name, Quantity = 1, UnitPrice = package.BasePrice, LineTotal = package.BasePrice, CreatedAt = createdAt });

            if (Rng.Next(0, 3) == 0)
            {
                var addOn = addOns.First(a => a.ServiceId == svc.Id);
                bookingAddOns.Add(new BookingAddOn { Id = $"ba-{bookingId}-1", BookingId = bookingId, AddOnId = addOn.Id, Name = addOn.Name, Price = addOn.Price, CreatedAt = createdAt });
            }

            // Status history chain (draft -> ... -> status)
            var chain = new List<string>();
            if (status != "draft") chain.Add("draft");
            if (status == "awaiting_payment") chain.Add("awaiting_payment");
            if (status == "confirmed") { chain.Add("awaiting_payment"); chain.Add("confirmed"); }
            if (status == "searching_provider") { chain.Add("awaiting_payment"); chain.Add("confirmed"); chain.Add("searching_provider"); }
            if (status is "assigned" or "provider_accepted" or "on_the_way" or "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed")
            {
                chain.Add("awaiting_payment"); chain.Add("confirmed"); chain.Add("searching_provider"); chain.Add("assigned");
            }
            if (status is "provider_accepted" or "on_the_way" or "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed") chain.Add("provider_accepted");
            if (status is "on_the_way" or "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed") chain.Add("on_the_way");
            if (status is "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed") chain.Add("arrived");
            if (status is "in_service" or "service_completed" or "completed" or "disputed" or "closed") chain.Add("in_service");
            if (status is "service_completed" or "completed" or "disputed" or "closed") chain.Add("service_completed");
            if (status == "completed") chain.Add("completed");
            if (status == "disputed") chain.Add("disputed");
            if (status == "closed") { chain.Add("disputed"); chain.Add("closed"); }
            if (status == "cancelled") { chain.Add("confirmed"); chain.Add("cancelled"); }
            if (status == "refund_pending") { chain.Add("confirmed"); chain.Add("cancelled"); chain.Add("refund_pending"); }
            if (status == "refunded") { chain.Add("confirmed"); chain.Add("cancelled"); chain.Add("refund_pending"); chain.Add("refunded"); }

            for (var c = 0; c < chain.Count; c++)
            {
                bookingStatusHistories.Add(new BookingStatusHistory
                {
                    Id = $"bh-{bookingId}-{c + 1}",
                    BookingId = bookingId,
                    PreviousStatus = c == 0 ? string.Empty : chain[c - 1],
                    NewStatus = chain[c],
                    ChangedBy = c == 0 ? "system" : "user",
                    ChangedAt = createdAt.AddHours(c * 4 + 1),
                    Reason = c == 0 ? "Booking created" : "Status advanced",
                    MetadataJson = "{}",
                    CreatedAt = createdAt.AddHours(c * 4 + 1),
                });
            }

            if (status is "assigned" or "provider_accepted" or "on_the_way" or "arrived" or "in_service" or "service_completed" or "completed" or "disputed" or "closed")
            {
                bookingAssignments.Add(new BookingAssignment
                {
                    Id = $"bas-{bookingId}-1",
                    BookingId = bookingId,
                    ProfessionalId = professional.Id,
                    OfferedAt = createdAt.AddHours(30),
                    RespondedAt = createdAt.AddHours(31),
                    Response = "accepted",
                    CreatedAt = createdAt.AddHours(30),
                });
            }

            if (Rng.Next(0, 3) == 0)
            {
                bookingNotes.Add(new BookingNote { Id = $"bn-{bookingId}-1", BookingId = bookingId, AuthorId = "user-ops", Note = "Handled by ops team.", Visibility = "internal", CreatedAt = createdAt.AddHours(5) });
            }

            // Price quote for paid bookings
            var grandTotal = package.BasePrice;
            if (paid)
            {
                var quoteId = $"pq-{bookingId}";
                var tax = Math.Round(grandTotal * 0.18m, 2);
                var platformFee = 49m;
                var finalTotal = grandTotal + platformFee + tax;
                priceQuotes.Add(new PriceQuote
                {
                    Id = quoteId,
                    QuoteNumber = $"Q{i + 1:D6}",
                    CustomerId = customer.Id,
                    ServiceId = svc.Id,
                    PackageId = package.Id,
                    AddressId = address.Id,
                    BasePrice = grandTotal,
                    AddOnsTotal = 0,
                    MaterialsTotal = 0,
                    FeesTotal = platformFee,
                    TravelCharge = 0,
                    UrgentCharge = 0,
                    PlatformFee = platformFee,
                    DiscountTotal = 0,
                    TaxTotal = tax,
                    GrandTotal = finalTotal,
                    ExpiresAt = createdAt.AddDays(7),
                    Version = 1,
                    Status = "accepted",
                    Notes = "Auto-generated seed quote",
                    CreatedAt = createdAt,
                });
                quoteRevisions.Add(new QuoteRevision { Id = $"qr-{quoteId}-1", PriceQuoteId = quoteId, RevisionNumber = 1, Reason = "Initial quote", PreviousTotal = finalTotal, NewTotal = finalTotal, CreatedBy = "system", CreatedAt = createdAt });

                payments.Add(new Payment
                {
                    Id = $"pay-{bookingId}",
                    BookingId = bookingId,
                    PaymentNumber = $"PAY{i + 1:D6}",
                    Amount = finalTotal,
                    Method = new[] { "upi", "card", "netbanking", "wallet" }[Rng.Next(0, 4)],
                    Status = status == "refunded" ? "refunded" : status == "refund_pending" ? "captured" : "captured",
                    GatewayRef = $"rzp_demo_{i + 1:D6}",
                    PaidAt = createdAt.AddDays(1),
                    GatewayProvider = "razorpay",
                    GatewayOrderId = $"order_{i + 1:D6}",
                    GatewayPaymentId = $"pay_{i + 1:D6}",
                    GatewaySignature = $"sig_{i + 1:D6}",
                    WebhookVerified = true,
                    CreatedAt = createdAt.AddDays(1),
                });

                if (status is "refunded" or "refund_pending")
                {
                    refunds.Add(new Refund
                    {
                        Id = $"ref-{bookingId}",
                        PaymentId = $"pay-{bookingId}",
                        BookingId = bookingId,
                        Amount = status == "refund_pending" ? finalTotal / 2 : finalTotal,
                        Reason = "Booking cancelled - customer request",
                        Status = status == "refund_pending" ? "approved" : "processed",
                        ProcessedBy = "user-finance",
                        ProcessedAt = createdAt.AddDays(3),
                        GatewayRefundId = $"rfnd_{i + 1:D6}",
                        CreatedAt = createdAt.AddDays(2),
                    });
                }

                if (status is "completed" or "service_completed" or "disputed" or "closed")
                {
                    var commissionRate = 20m;
                    var commission = Math.Round(grandTotal * commissionRate / 100m, 2);
                    var taxWithheld = Math.Round(commission * 0.10m, 2);
                    var net = grandTotal - commission - taxWithheld;
                    professionalEarnings.Add(new ProfessionalEarning
                    {
                        Id = $"earn-{bookingId}",
                        ProfessionalId = professional.Id,
                        BookingId = bookingId,
                        GrossAmount = grandTotal,
                        MaterialsExcludedAmount = 0,
                        CommissionAmount = commission,
                        AdjustmentAmount = 0,
                        TaxWithheldAmount = taxWithheld,
                        NetAmount = net,
                        Status = Rng.Next(0, 4) == 0 ? "settled" : "pending",
                        SettledAt = Rng.Next(0, 4) == 0 ? createdAt.AddDays(14) : null,
                        CreatedAt = createdAt.AddDays(1),
                    });
                }

                if (Rng.Next(0, 8) == 0)
                {
                    var coupon = coupons[Rng.Next(coupons.Count)];
                    var discount = coupon.DiscountType == "percent" ? Math.Min(grandTotal * coupon.Value / 100m, coupon.MaxDiscount > 0 ? coupon.MaxDiscount : grandTotal) : Math.Min(coupon.Value, grandTotal);
                    couponRedemptions.Add(new CouponRedemption { Id = $"cr-{bookingId}", CouponId = coupon.Id, CustomerId = customer.Id, BookingId = bookingId, DiscountApplied = Math.Round(discount, 2), CreatedAt = createdAt });
                }
            }

            if (status == "completed" && i % 2 == 0)
            {
                var rating = 3 + Rng.Next(0, 3);
                reviews.Add(new Review
                {
                    Id = $"rev-{bookingId}",
                    BookingId = bookingId,
                    CustomerId = customer.Id,
                    ProfessionalId = professional.Id,
                    Rating = rating,
                    Comment = new[] { "Great service, very professional.", "Satisfied with the work.", "Good value for money.", "Very punctual and clean.", "Would recommend to friends." }[Rng.Next(0, 5)],
                    TagsJson = """["punctual","professional"]""",
                    Quality = rating, Professionalism = rating, Punctuality = rating, Cleanliness = rating, Communication = rating, Value = rating,
                    CreatedAt = createdAt.AddDays(2),
                });
                if (Rng.Next(0, 3) == 0)
                {
                    reviewMediaItems.Add(new ReviewMedia { Id = $"rm-{bookingId}-1", ReviewId = $"rev-{bookingId}", MediaUrl = $"/images/reviews/{bookingId}.jpg", MediaType = "image", CreatedAt = createdAt.AddDays(2) });
                }
            }

            if (Rng.Next(0, 20) == 0 && paid)
            {
                notifications.Add(new Notification { Id = $"notif-{bookingId}", UserId = customer.UserId, Channel = "in_app", Template = "booking_confirmed", PayloadJson = $$"""{"bookingId":"{{bookingId}}","number":"{{bookingNumber}}"}""", SentAt = createdAt.AddHours(1) });
            }

            if (status is "disputed" or "closed")
            {
                disputes.Add(new Dispute
                {
                    Id = $"disp-{bookingId}",
                    BookingId = bookingId,
                    RaisedBy = customer.Id,
                    Reason = "service_quality",
                    Details = "Customer reported unsatisfactory service quality.",
                    Status = status == "closed" ? "resolved" : "investigating",
                    Resolution = status == "closed" ? "Resolved after partial refund" : null,
                    ResolvedBy = status == "closed" ? "user-support" : null,
                    ResolvedAt = status == "closed" ? createdAt.AddDays(5) : null,
                    CreatedAt = createdAt.AddDays(3),
                });
            }

            if (Rng.Next(0, 15) == 0)
            {
                supportTickets.Add(new SupportTicket
                {
                    Id = $"tkt-{bookingId}",
                    TicketNumber = $"TK{i + 1:D6}",
                    RaisedBy = customer.Id,
                    Role = "customer",
                    BookingId = bookingId,
                    Category = Rng.Next(0, 2) == 0 ? "billing" : "scheduling",
                    Subject = "Need help with my booking",
                    Description = "Customer raised a query about their recent booking.",
                    Status = new[] { "open", "in_progress", "resolved", "closed" }[Rng.Next(0, 4)],
                    Priority = new[] { "low", "medium", "high" }[Rng.Next(0, 3)],
                    AssignedTo = "user-support",
                    Resolution = "Issue addressed.",
                    CreatedAt = createdAt.AddDays(3),
                });
            }
        }

        // Payouts: aggregate settled earnings by professional
        foreach (var pro in professionals)
        {
            var settled = professionalEarnings.Where(e => e.ProfessionalId == pro.Id && e.Status == "settled").Sum(e => e.NetAmount);
            if (settled > 0)
            {
                payouts.Add(new Payout
                {
                    Id = $"po-{pro.Id}",
                    ProfessionalId = pro.Id,
                    PeriodStart = DateTime.UtcNow.AddDays(-30),
                    PeriodEnd = DateTime.UtcNow,
                    TotalAmount = Math.Round(settled, 2),
                    Status = Rng.Next(0, 3) == 0 ? "paid" : "pending",
                    PaidAt = Rng.Next(0, 3) == 0 ? DateTime.UtcNow.AddDays(-2) : null,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                });
            }
        }

        // Recurring + AMC
        for (var i = 0; i < 12; i++)
        {
            var customer = customers[i * 7 % customers.Count];
            var address = customerAddresses.First(a => a.CustomerId == customer.Id);
            var svc = allServices[(i * 5) % allServices.Count];
            var package = packages.First(p => p.ServiceId == svc.Id);
            recurringBookings.Add(new RecurringBooking
            {
                Id = $"rb-{i + 1}",
                CustomerId = customer.Id,
                ServiceId = svc.Id,
                PackageId = package.Id,
                AddressId = address.Id,
                Frequency = new[] { "weekly", "biweekly", "monthly" }[i % 3],
                NextRunAt = DateTime.UtcNow.AddDays(7 * (i % 4 + 1)),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
            });
        }
        for (var i = 0; i < 8; i++)
        {
            var customer = customers[i * 13 % customers.Count];
            var address = customerAddresses.First(a => a.CustomerId == customer.Id);
            var svc = allServices[(i * 11) % allServices.Count];
            amcContracts.Add(new AmcContract
            {
                Id = $"amc-{i + 1}",
                CustomerId = customer.Id,
                ServiceId = svc.Id,
                AddressId = address.Id,
                VisitsPerYear = 2,
                StartDate = DateTime.UtcNow.AddDays(-120),
                EndDate = DateTime.UtcNow.AddDays(245),
                Price = 2999 + i * 500,
                Status = "active",
                CoveredServices = "Annual servicing, priority response",
                ExcludedParts = "Replacement parts billed separately",
                CreatedAt = DateTime.UtcNow.AddDays(-120),
            });
        }

        // Adjustments + incentives
        foreach (var pro in professionals.Take(15))
        {
            professionalAdjustments.Add(new ProfessionalAdjustment { Id = $"padj-{pro.Id}", ProfessionalId = pro.Id, Amount = 50 + Rng.Next(0, 200), Reason = "Bonus for high customer rating", CreatedBy = "user-ops", CreatedAt = DateTime.UtcNow.AddDays(-5) });
        }
        foreach (var pro in professionals.Take(10))
        {
            professionalIncentives.Add(new ProfessionalIncentive { Id = $"pin-{pro.Id}", ProfessionalId = pro.Id, IncentiveType = "completion_bonus", Amount = 200 + Rng.Next(0, 300), PeriodStart = DateTime.UtcNow.AddDays(-30), PeriodEnd = DateTime.UtcNow, Status = "paid", CreatedAt = DateTime.UtcNow.AddDays(-2) });
        }

        // CMS, banners, FAQs, audit logs
        var cmsPages = new List<CmsPage>
        {
            new() { Id = "cms-about", Slug = "about", Title = "About VSR Home Services", Body = "VSR Home Services connects you with verified professionals for all your home needs.", IsPublished = true },
            new() { Id = "cms-terms", Slug = "terms", Title = "Terms of Service", Body = "Standard marketplace terms.", IsPublished = true },
            new() { Id = "cms-privacy", Slug = "privacy", Title = "Privacy Policy", Body = "Your data is protected.", IsPublished = true },
            new() { Id = "cms-refund", Slug = "refund-policy", Title = "Refund Policy", Body = "Refunds processed within 5-7 business days.", IsPublished = true },
        };
        var banners = new List<Banner>
        {
            new() { Id = "banner-1", Title = "Monsoon Home Care", ImageUrl = "/images/banners/monsoon.jpg", LinkUrl = "/home-services/categories/deep-cleaning", SortOrder = 1, IsActive = true },
            new() { Id = "banner-2", Title = "AC Ready for Summer", ImageUrl = "/images/banners/ac.jpg", LinkUrl = "/home-services/categories/ac-services", SortOrder = 2, IsActive = true },
            new() { Id = "banner-3", Title = "Plumbing Emergency? We're on it", ImageUrl = "/images/banners/plumbing.jpg", LinkUrl = "/home-services/categories/plumbing", SortOrder = 3, IsActive = true },
        };
        var faqs = new List<Faq>
        {
            new() { Id = "faq-1", Category = "booking", Question = "How do I book a service?", Answer = "Choose a category, pick a package, schedule a slot, and pay securely.", SortOrder = 1 },
            new() { Id = "faq-2", Category = "payment", Question = "What payment methods are supported?", Answer = "UPI, cards, netbanking and wallets via Razorpay.", SortOrder = 2 },
            new() { Id = "faq-3", Category = "refund", Question = "How do refunds work?", Answer = "Refunds are processed within 5-7 business days to the original payment method.", SortOrder = 3 },
            new() { Id = "faq-4", Category = "professional", Question = "Are professionals verified?", Answer = "Yes, every professional passes ID, address and police verification.", SortOrder = 4 },
        };
        var auditLogs = new List<AuditLog>
        {
            new() { Id = "audit-1", ActorId = "user-admin", Action = "seed.created", EntityType = "HomeServicesModule", EntityId = "seed", AfterJson = """{"version":"1.0"}""", CreatedAtUtc = DateTime.UtcNow },
        };

        var paymentGatewaySettings = new List<PaymentGatewaySetting>
        {
            new() { Id = "pgs-razorpay", Provider = "razorpay", IsActive = false, Mode = "test", KeyId = "rzp_test_demo", KeySecretRef = "secrets:razorpay:test:key_secret", WebhookSecretRef = "secrets:razorpay:test:webhook_secret" },
        };

        return new HomeServicesSeedBundle(
            cities, zones, localities, pincodes, serviceAreas, serviceAreaServices,
            categories, services, problems, packages, addOns, packageAddOns, warranties,
            roles, permissions, rolePermissions, membershipPlans,
            users, userRoles, customers, customerAddresses, customerMemberships,
            professionals, professionalDocuments, professionalSkills, professionalServiceAreas,
            professionalAvailabilities, professionalPerformances,
            priceRules, commissionRules, coupons,
            bookings, bookingItems, bookingAddOns, bookingMaterials, bookingAssignments,
            bookingStatusHistories, bookingNotes, priceQuotes, quoteRevisions,
            payments, refunds, creditTransactions, professionalEarnings, payouts, couponRedemptions,
            reviews, reviewMediaItems, recurringBookings, amcContracts, supportTickets, disputes,
            notifications, cmsPages, banners, faqs, auditLogs, paymentGatewaySettings,
            professionalAdjustments, professionalIncentives);
    }
}