namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

public abstract class RailwayEntity
{
    protected RailwayEntity()
    {
    }

    protected RailwayEntity(Guid id, Guid organizationId, Guid? divisionId = null)
    {
        Id = id;
        OrganizationId = organizationId;
        DivisionId = divisionId;
        ValidateOwnership();
    }

    public Guid Id { get; protected init; }
    public Guid OrganizationId { get; protected init; }
    public Guid? DivisionId { get; protected set; }
    public long Version { get; protected set; }

    public void ValidateOwnership()
    {
        if (Id == Guid.Empty)
            throw new ArgumentException("A Railway entity requires a non-empty identifier.", nameof(Id));

        if (OrganizationId == Guid.Empty)
            throw new ArgumentException("A Railway entity requires organization ownership.", nameof(OrganizationId));
    }
}
