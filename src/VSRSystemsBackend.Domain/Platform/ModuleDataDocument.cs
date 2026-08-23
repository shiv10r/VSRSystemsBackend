using System.ComponentModel.DataAnnotations;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Domain.Platform;

public sealed class ModuleDataDocument : BaseEntity<Guid>
{
    [MaxLength(50)]
    public string Module { get; set; } = string.Empty;

    [MaxLength(150)]
    public string Collection { get; set; } = string.Empty;

    public string Json { get; set; } = "[]";
}
