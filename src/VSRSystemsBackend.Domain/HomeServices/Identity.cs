using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class User : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(500)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active"; // active/suspended/blocked

    public DateTime? LastLoginAt { get; set; }

    // Navigation
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}

public class Role : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty; // customer/professional/ops_agent/support_agent/finance_agent/admin

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    // Navigation
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class UserRole : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoleId { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;
}

public class Permission : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Area { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    // Navigation
    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

public class RolePermission : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RoleId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PermissionId { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(RoleId))]
    public virtual Role Role { get; set; } = null!;

    [ForeignKey(nameof(PermissionId))]
    public virtual Permission Permission { get; set; } = null!;
}

public class Customer : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? DefaultAddressId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal WalletBalance { get; set; } = 0;

    [MaxLength(50)]
    public string? MembershipPlanId { get; set; }

    [MaxLength(50)]
    public string? ReferralCode { get; set; }

    [MaxLength(50)]
    public string? ReferredByCustomerId { get; set; }

    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
    public virtual ICollection<CreditTransaction> CreditTransactions { get; set; } = new List<CreditTransaction>();
    public virtual ICollection<CustomerMembership> Memberships { get; set; } = new List<CustomerMembership>();
}

public class CustomerAddress : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Label { get; set; } = string.Empty; // home/office/other

    [MaxLength(200)]
    public string Line1 { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Line2 { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CityId { get; set; }

    [MaxLength(50)]
    public string? ZoneId { get; set; }

    [MaxLength(50)]
    public string? LocalityId { get; set; }

    [MaxLength(20)]
    public string Pincode { get; set; } = string.Empty;

    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public bool IsDefault { get; set; } = false;

    [MaxLength(100)]
    public string? ContactPerson { get; set; }

    [MaxLength(20)]
    public string? ContactPhone { get; set; }

    [MaxLength(500)]
    public string? AccessInstructions { get; set; }

    // Navigation
    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;
}

public class SupportTicket : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TicketNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RaisedBy { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Role { get; set; } = "customer"; // customer/professional

    [MaxLength(50)]
    public string? BookingId { get; set; }

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(300)]
    public string Subject { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "open"; // open/in_progress/waiting_customer/waiting_professional/escalated/resolved/closed

    [MaxLength(10)]
    public string Priority { get; set; } = "medium"; // low/medium/high/critical

    [MaxLength(50)]
    public string? AssignedTo { get; set; }

    [MaxLength(1000)]
    public string? Resolution { get; set; }

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking? Booking { get; set; }
}

public class Dispute : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? TicketId { get; set; }

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string RaisedBy { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Reason { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Details { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "open"; // open/investigating/resolved/rejected

    [MaxLength(1000)]
    public string? Resolution { get; set; }

    [MaxLength(50)]
    public string? ResolvedBy { get; set; }

    public DateTime? ResolvedAt { get; set; }

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;
}

public class Conversation : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string BookingId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProfessionalId { get; set; } = string.Empty;

    public bool IsMasked { get; set; } = false;

    // Navigation
    [ForeignKey(nameof(BookingId))]
    public virtual Booking Booking { get; set; } = null!;

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey(nameof(ProfessionalId))]
    public virtual Professional Professional { get; set; } = null!;

    public virtual ICollection<ConversationMessage> Messages { get; set; } = new List<ConversationMessage>();
}

public class ConversationMessage : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ConversationId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SenderId { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Body { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? ImageUrl { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }

    // Navigation
    [ForeignKey(nameof(ConversationId))]
    public virtual Conversation Conversation { get; set; } = null!;
}

public class Notification : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(10)]
    public string Channel { get; set; } = "in_app"; // push/sms/email/in_app

    [MaxLength(100)]
    public string Template { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string PayloadJson { get; set; } = "{}";

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReadAt { get; set; }
}

public class CmsPage : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(10000)]
    public string Body { get; set; } = string.Empty;

    public bool IsPublished { get; set; } = false;
}

public class Banner : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string LinkUrl { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

public class Faq : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Question { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Answer { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
}

public class AuditLog : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? ActorId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    [MaxLength(50)]
    public string EntityId { get; set; } = string.Empty;

    [MaxLength(5000)]
    public string BeforeJson { get; set; } = "{}";

    [MaxLength(5000)]
    public string AfterJson { get; set; } = "{}";

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
