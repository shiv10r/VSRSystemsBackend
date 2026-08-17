using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Hotel.DTOs;

public class GuestDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public bool Vip { get; set; }
    public int Stays { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateGuestDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Nationality { get; set; }
    public bool Vip { get; set; }
}

public class UpdateGuestDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    public string? Email { get; set; }
    public string? Nationality { get; set; }
    public bool Vip { get; set; }
}

public class RoomDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateRoomDto
{
    [Required]
    [MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = "Standard";

    [MaxLength(20)]
    public string Status { get; set; } = "vacant-clean";

    public decimal Rate { get; set; }
}

public class UpdateRoomDto
{
    [MaxLength(20)]
    public string Number { get; set; } = string.Empty;

    public int Floor { get; set; }

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public decimal Rate { get; set; }
}

public class ReservationDto
{
    public string Id { get; set; } = string.Empty;
    public string Confirmation { get; set; } = string.Empty;
    public string GuestId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateReservationDto
{
    [Required]
    [MaxLength(50)]
    public string GuestId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public int Adults { get; set; } = 1;
    public int Children { get; set; } = 0;

    [MaxLength(20)]
    public string Source { get; set; } = "Direct";

    [MaxLength(20)]
    public string Status { get; set; } = "confirmed";
}

public class UpdateReservationDto
{
    [MaxLength(50)]
    public string GuestId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    public int Adults { get; set; }
    public int Children { get; set; }

    [MaxLength(20)]
    public string Source { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}

public class HousekeepingTaskDto
{
    public string Id { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string Assignee { get; set; } = string.Empty;
    public string Task { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Scheduled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateHousekeepingTaskDto
{
    [Required]
    [MaxLength(20)]
    public string RoomNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Assignee { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Task { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Priority { get; set; } = "normal";

    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    public DateTime Scheduled { get; set; } = DateTime.UtcNow;
}

public class UpdateHousekeepingTaskDto
{
    [MaxLength(100)]
    public string Assignee { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Task { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Priority { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime Scheduled { get; set; }
}