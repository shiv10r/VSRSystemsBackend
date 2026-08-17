namespace VSRSystemsBackend.Core.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public abstract class BaseEntity<TId> : BaseEntity
{
    public virtual TId Id { get; set; } = default!;
}

public abstract class AuditableEntity : BaseEntity
{
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

public abstract class AuditableEntity<TId> : AuditableEntity
{
    public virtual TId Id { get; set; } = default!;
}