using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Domain.Travel;
using VSRSystemsBackend.Infrastructure.Persistence;

namespace VSRSystemsBackend.Infrastructure.Data.Seeds;

/// <summary>
/// Seeds the VSR Travel catalogue (destinations, packages, departures) so the
/// travel marketplace API returns real data. Idempotent: skips when the
/// Destinations table already has rows.
/// </summary>
public static class TravelSeeder
{
    private const string Bali = "https://images.unsplash.com/photo-1537996194471-e657df975ab4?auto=format&fit=crop&w=1200&q=82";
    private const string Vietnam = "https://images.unsplash.com/photo-1528127269322-539801943592?auto=format&fit=crop&w=1200&q=82";
    private const string Kashmir = "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?auto=format&fit=crop&w=1200&q=82";
    private const string Dubai = "https://images.unsplash.com/photo-1512453979798-5ea266f8880c?auto=format&fit=crop&w=1200&q=82";
    private const string Maldives = "https://images.unsplash.com/photo-1514282401047-d79a71a590e8?auto=format&fit=crop&w=1200&q=82";
    private const string Manali = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=82";

    public static async Task SeedAsync(AppDbContext context, CancellationToken ct = default)
    {
        if (await context.Destinations.AnyAsync(ct))
            return;

        var destinations = new List<Destination>
        {
            new() { Id = "dest-bali", Code = "bali", Name = "Bali", Country = "Indonesia", Description = "Temples, rice terraces and island sunsets", ImageUrls = new List<string> { Bali }, Status = "active" },
            new() { Id = "dest-vietnam", Code = "vietnam", Name = "Vietnam", Country = "Vietnam", Description = "Lantern towns, bays and vibrant cities", ImageUrls = new List<string> { Vietnam }, Status = "active" },
            new() { Id = "dest-kashmir", Code = "kashmir", Name = "Kashmir", Country = "India", Description = "Alpine valleys and peaceful houseboats", ImageUrls = new List<string> { Kashmir }, Status = "active" },
            new() { Id = "dest-dubai", Code = "dubai", Name = "Dubai", Country = "UAE", Description = "Desert adventures and iconic skylines", ImageUrls = new List<string> { Dubai }, Status = "active" },
            new() { Id = "dest-maldives", Code = "maldives", Name = "Maldives", Country = "Maldives", Description = "Private lagoons and overwater escapes", ImageUrls = new List<string> { Maldives }, Status = "active" },
            new() { Id = "dest-manali", Code = "manali", Name = "Manali", Country = "India", Description = "Snow peaks, rivers and cosy cafes", ImageUrls = new List<string> { Manali }, Status = "active" },
        };

        var packages = new List<TravelPackage>
        {
            new() { Id = "pkg-vietnam", Code = "vietnam-discovery", Name = "Vietnam Discovery", Description = "Hanoi · Da Nang · Ho Chi Minh", DestinationId = "dest-vietnam", Category = "Culture", DurationDays = 7, Price = 42999, DiscountedPrice = 47999, ImageUrls = new List<string> { Vietnam }, MaxGroupSize = 140, Inclusions = "Hotels\nBreakfast\nTransfers", Status = "active" },
            new() { Id = "pkg-bali", Code = "bali-island-retreat", Name = "Bali Island Retreat", Description = "Ubud · Nusa Dua · Seminyak", DestinationId = "dest-bali", Category = "Beach", DurationDays = 6, Price = 36999, ImageUrls = new List<string> { Bali }, MaxGroupSize = 96, Inclusions = "Resort\nBreakfast\nSightseeing", Status = "active" },
            new() { Id = "pkg-kashmir", Code = "kashmir-valley-escape", Name = "Kashmir Valley Escape", Description = "Srinagar · Gulmarg · Pahalgam", DestinationId = "dest-kashmir", Category = "Family", DurationDays = 5, Price = 24999, DiscountedPrice = 27999, ImageUrls = new List<string> { Kashmir }, MaxGroupSize = 188, Inclusions = "Hotels\nBreakfast\nCab", Status = "active" },
            new() { Id = "pkg-dubai", Code = "dazzling-dubai", Name = "Dazzling Dubai", Description = "Downtown · Marina · Desert", DestinationId = "dest-dubai", Category = "Family", DurationDays = 5, Price = 38999, ImageUrls = new List<string> { Dubai }, MaxGroupSize = 212, Inclusions = "Hotels\nVisa\nTransfers", Status = "active" },
            new() { Id = "pkg-maldives", Code = "maldives-for-two", Name = "Maldives for Two", Description = "Malé · Private Island Resort", DestinationId = "dest-maldives", Category = "Romantic", DurationDays = 5, Price = 58999, DiscountedPrice = 64999, ImageUrls = new List<string> { Maldives }, MaxGroupSize = 74, Inclusions = "Water villa\nMeals\nSpeedboat", Status = "active" },
            new() { Id = "pkg-manali", Code = "manali-weekend", Name = "Manali Long Weekend", Description = "Manali · Solang · Atal Tunnel", DestinationId = "dest-manali", Category = "Weekend", DurationDays = 4, Price = 16999, ImageUrls = new List<string> { Manali }, MaxGroupSize = 260, Inclusions = "Hotel\nBreakfast\nVolvo", Status = "active" },
        };

        var departures = new List<TravelDeparture>
        {
            new() { Id = "dep-1", Code = "dep-1", Title = "Vietnam Festive Escape", PackageId = "pkg-vietnam", DepartureCity = "Delhi", DepartureDate = new DateTime(2026, 12, 28, 0, 0, 0, DateTimeKind.Utc), AvailableSeats = 12, TotalSeats = 30, Price = 49999, ImageUrl = Vietnam, Status = "active" },
            new() { Id = "dep-2", Code = "dep-2", Title = "Dubai New Year Group", PackageId = "pkg-dubai", DepartureCity = "Mumbai", DepartureDate = new DateTime(2026, 12, 29, 0, 0, 0, DateTimeKind.Utc), AvailableSeats = 8, TotalSeats = 24, Price = 46999, ImageUrl = Dubai, Status = "active" },
            new() { Id = "dep-3", Code = "dep-3", Title = "Manali Snow Weekend", PackageId = "pkg-manali", DepartureCity = "Delhi", DepartureDate = new DateTime(2027, 1, 16, 0, 0, 0, DateTimeKind.Utc), AvailableSeats = 16, TotalSeats = 32, Price = 18999, ImageUrl = Manali, Status = "active" },
            new() { Id = "dep-4", Code = "dep-4", Title = "Bali Young Travellers", PackageId = "pkg-bali", DepartureCity = "Bengaluru", DepartureDate = new DateTime(2027, 2, 7, 0, 0, 0, DateTimeKind.Utc), AvailableSeats = 10, TotalSeats = 20, Price = 41999, ImageUrl = Bali, Status = "active" },
        };

        await context.Destinations.AddRangeAsync(destinations, ct);
        await context.TravelPackages.AddRangeAsync(packages, ct);
        await context.TravelDepartures.AddRangeAsync(departures, ct);
        await context.SaveChangesAsync(ct);
    }
}