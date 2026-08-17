using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Interior.DTOs;

public class InteriorProjectDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PropertyType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int TotalArea { get; set; }
    public decimal Budget { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateInteriorProjectDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PropertyType { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Location { get; set; } = string.Empty;

    public int TotalArea { get; set; }
    public decimal Budget { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "active";
}

public class UpdateInteriorProjectDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PropertyType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Location { get; set; } = string.Empty;

    public int TotalArea { get; set; }
    public decimal Budget { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class InteriorRoomDto
{
    public string Id { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public decimal Budget { get; set; }
    public string? Notes { get; set; }
    public string? Image { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateInteriorRoomDto
{
    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoomType { get; set; } = string.Empty;

    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public decimal Budget { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public string? Image { get; set; }
}

public class UpdateInteriorRoomDto
{
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string RoomType { get; set; } = string.Empty;

    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public decimal Budget { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public string? Image { get; set; }
}

public class InteriorDesignDto
{
    public string Id { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Style { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Favorite { get; set; }
    public bool Saved { get; set; }
    public int CurrentVersion { get; set; }
    public List<DesignVersionDto> Versions { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class DesignVersionDto
{
    public string Id { get; set; } = string.Empty;
    public string DesignId { get; set; } = string.Empty;
    public int Version { get; set; }
    public string Style { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public List<string> ProductIds { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateInteriorDesignDto
{
    [Required]
    [MaxLength(50)]
    public string ProjectId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoomId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Style { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    public decimal Budget { get; set; }
}

public class UpdateInteriorDesignDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Style { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Color { get; set; } = string.Empty;

    public decimal Budget { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Favorite { get; set; }
    public bool Saved { get; set; }
}

public class GenerateDesignDto
{
    [MaxLength(2000)]
    public string Prompt { get; set; } = string.Empty;
}

public class InteriorProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
    public string? Width { get; set; }
    public string? Depth { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateInteriorProductDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [MaxLength(100)]
    public string? Material { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    [MaxLength(50)]
    public string? Width { get; set; }

    [MaxLength(50)]
    public string? Depth { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}

public class UpdateInteriorProductDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [MaxLength(100)]
    public string? Material { get; set; }

    [MaxLength(50)]
    public string? Color { get; set; }

    [MaxLength(50)]
    public string? Width { get; set; }

    [MaxLength(50)]
    public string? Depth { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}