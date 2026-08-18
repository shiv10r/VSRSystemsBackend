using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.HomeServices;

public class ServiceCategory : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Tagline { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}

public class Service : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CategoryId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(220)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string LongDescription { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string ImageUrl { get; set; } = string.Empty;

    public bool IsEmergency { get; set; } = false;
    public bool NeedsInspection { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")]
    public decimal InspectionFee { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(CategoryId))]
    public virtual ServiceCategory Category { get; set; } = null!;
    public virtual ICollection<ServiceProblem> Problems { get; set; } = new List<ServiceProblem>();
    public virtual ICollection<ServicePackage> Packages { get; set; } = new List<ServicePackage>();
    public virtual ICollection<ServiceAddOn> AddOns { get; set; } = new List<ServiceAddOn>();
    public virtual ICollection<ServiceWarranty> Warranties { get; set; } = new List<ServiceWarranty>();
}

public class ServiceProblem : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public int SortOrder { get; set; } = 0;

    // Navigation
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}

public class ServicePackage : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty; // Basic / Standard / Premium

    [MaxLength(500)]
    public string ShortDescription { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string DetailedDescription { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal BasePrice { get; set; } = 0;

    public int DurationMins { get; set; } = 60;

    [MaxLength(2000)]
    public string WhatIncluded { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string WhatExcluded { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Warranty { get; set; } = string.Empty;

    public bool InspectionRequired { get; set; } = false;
    public bool PartsIncluded { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumCharge { get; set; } = 0;

    [MaxLength(200)]
    public string CancellationRule { get; set; } = string.Empty;

    public bool IsPopular { get; set; } = false;
    public bool IsEmergencyEligible { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
    public virtual ICollection<ServicePackageAddOn> PackageAddOns { get; set; } = new List<ServicePackageAddOn>();
}

public class ServiceAddOn : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    public int DurationMins { get; set; } = 0;
    public bool IsActive { get; set; } = true;

    // Navigation
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
    public virtual ICollection<ServicePackageAddOn> PackageAddOns { get; set; } = new List<ServicePackageAddOn>();
}

public class ServicePackageAddOn : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string PackageId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string AddOnId { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(PackageId))]
    public virtual ServicePackage Package { get; set; } = null!;

    [ForeignKey(nameof(AddOnId))]
    public virtual ServiceAddOn AddOn { get; set; } = null!;
}

public class ServiceWarranty : AuditableEntity<string>
{
    [Key]
    [MaxLength(50)]
    public override string Id { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ServiceId { get; set; } = string.Empty;

    public int WarrantyDays { get; set; } = 0;

    [MaxLength(1000)]
    public string Terms { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(ServiceId))]
    public virtual Service Service { get; set; } = null!;
}
