using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class ReturnRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string ReturnNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string PartyName { get; set; } = string.Empty;
    public string OriginalRef { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<ReturnLineDto> Items { get; set; } = new();
    public string Status { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ReturnLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateReturnRecordDto
{
    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "customer";

    [Required]
    [MaxLength(200)]
    public string PartyName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string OriginalRef { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public List<CreateReturnLineDto> Items { get; set; } = new();
}

public class CreateReturnLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Condition { get; set; } = "good";

    [Required]
    [MaxLength(30)]
    public string Action { get; set; } = "restock";
}

public class UpdateReturnRecordDto
{
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(200)]
    public string PartyName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string OriginalRef { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Remarks { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public List<UpdateReturnLineDto> Items { get; set; } = new();
}

public class UpdateReturnLineDto
{
    public int Qty { get; set; } = 1;

    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Condition { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Action { get; set; } = string.Empty;
}

public class InspectReturnDto
{
    public string Status { get; set; } = "inspected";
    public List<InspectReturnLineDto> Items { get; set; } = new();
}

public class InspectReturnLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    public int Qty { get; set; } = 1;

    [MaxLength(20)]
    public string Condition { get; set; } = "good";

    [MaxLength(30)]
    public string Action { get; set; } = "restock";
}