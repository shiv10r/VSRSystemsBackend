namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

public enum WorkOrderStatus { Draft, Triaged, Approved, Scheduled, InProgress, Blocked, AwaitingVerification, Completed, Cancelled }
public enum WorkOrderPriority { Low, Medium, High, Critical }
