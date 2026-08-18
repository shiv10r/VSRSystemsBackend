using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class City : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public DateTime? LaunchedAt { get; set; }

    // Navigation
    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}

public class Zone : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CityId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(CityId))]
    public virtual City City { get; set; } = null!;
    public virtual ICollection<Locality> Localities { get; set; } = new List<Locality>();
}

public class Locality : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ZoneId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Pincode { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(ZoneId))]
    public virtual Zone Zone { get; set; } = null!;
}

public class Pincode : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CityId { get; set; } = string.Empty;

    public bool IsServiceable { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(CityId))]
    public virtual City City { get; set; } = null!;
}

public class ServiceArea : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CityId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ZoneId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(CityId))]
    public virtual City City { get; set; } = null!;

    [ForeignKey(nameof(ZoneId))]
    public virtual Zone Zone { get; set; } = null!;
    public virtual ICollection<ServiceAreaService> AreaServices { get; set; } = new List<ServiceAreaService>();
}

public class ServiceAreaService : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceAreaId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ServiceAreaId))]
    public virtual ServiceArea ServiceArea { get; set; } = null!;

    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}
