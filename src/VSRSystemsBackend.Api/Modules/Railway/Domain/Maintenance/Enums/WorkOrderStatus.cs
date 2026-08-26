namespace VSRSystemsBackend.Api.Domain.Maintenance
{
    public enum WorkOrderStatus
    {
        Draft,
        Triaged,
        Approved,
        Scheduled,
        InProgress,
        Blocked,
        AwaitingVerification,
        Completed,
        Cancelled
    }
}