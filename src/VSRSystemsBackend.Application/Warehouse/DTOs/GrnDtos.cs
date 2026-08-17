using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Warehouse.DTOs;

public class GrnRecordDto
{
    public string Id { get; set; } = string.Empty;
    public string GrnNumber { get; set; } = string.Empty;
    public string PoId { get; set; } = string.Empty;
    public string PoNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public List<GrnLineDto> Lines { get; set; } = new();
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class GrnLineDto
{
    public int Id { get; set; }
    public string ItemId { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public int OrderedQty { get; set; }
    public int ReceivedQty { get; set; }
    public int DamagedQty { get; set; }
    public int RejectedQty { get; set; }
    public int AcceptedQty { get; set; }
    public List<PutawayBinDto> Putaway { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateGrnRecordDto
{
    [Required]
    [MaxLength(50)]
    public string PoId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string GrnNumber { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<CreateGrnLineDto> Lines { get; set; } = new();
}

public class CreateGrnLineDto
{
    [Required]
    [MaxLength(50)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ItemName { get; set; } = string.Empty;

    public int OrderedQty { get; set; }
    public int ReceivedQty { get; set; }
    public int DamagedQty { get; set; }
    public int RejectedQty { get; set; }
    public List<PutawayBinDto> Putaway { get; set; } = new();
}

public class UpdateGrnRecordDto
{
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public List<UpdateGrnLineDto> Lines { get; set; } = new();
}

public class UpdateGrnLineDto
{
    public int ReceivedQty { get; set; }
    public int DamagedQty { get; set; }
    public int RejectedQty { get; set; }
    public List<PutawayBinDto> Putaway { get; set; } = new();
}