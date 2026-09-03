using VSRSystemsBackend.Api.Platform.Storage;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

public enum RailwayEvidenceScanStatus { PendingUpload, Quarantined, Clean, Rejected }

public sealed class RailwayEvidence : RailwayEntity
{
    private RailwayEvidence() { }

    public RailwayEvidence(
        Guid id,
        Guid organizationId,
        Guid divisionId,
        Guid ownerRecordId,
        string category,
        string bucket,
        string path,
        string contentType,
        long sizeBytes,
        string sha256)
        : base(id, organizationId, divisionId)
    {
        OwnerRecordId = ownerRecordId;
        Category = category;
        Bucket = bucket;
        Path = path;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        Sha256 = sha256;
    }

    public Guid OwnerRecordId { get; private set; }
    public string Category { get; private set; } = string.Empty;
    public string Bucket { get; private set; } = string.Empty;
    public string Path { get; private set; } = string.Empty;
    public string ContentType { get; private set; } = string.Empty;
    public long SizeBytes { get; private set; }
    public string Sha256 { get; private set; } = string.Empty;
    public RailwayEvidenceScanStatus ScanStatus { get; private set; } = RailwayEvidenceScanStatus.PendingUpload;
    public DateTimeOffset? FinalizedAt { get; private set; }
    public DateTimeOffset? ScannedAt { get; private set; }
    public string? ScanDetail { get; private set; }

    public void FinalizeUpload(string sha256, DateTimeOffset now)
    {
        if (!string.Equals(Sha256, sha256, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Evidence checksum does not match the initiated upload.");
        if (ScanStatus != RailwayEvidenceScanStatus.PendingUpload)
            throw new InvalidOperationException("Evidence upload has already been finalized.");
        ScanStatus = RailwayEvidenceScanStatus.Quarantined;
        FinalizedAt = now;
        Version++;
    }

    public void RecordScan(MalwareScanVerdict verdict, string? detail, DateTimeOffset now)
    {
        ScanStatus = verdict switch
        {
            MalwareScanVerdict.Clean => RailwayEvidenceScanStatus.Clean,
            MalwareScanVerdict.Infected => RailwayEvidenceScanStatus.Rejected,
            _ => RailwayEvidenceScanStatus.Quarantined,
        };
        ScanDetail = detail;
        ScannedAt = now;
        Version++;
    }
}
