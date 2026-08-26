namespace VSRSystemsBackend.Api.Domain.Maintenance
{
    public class MaintenancePlan
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string RecurrenceRule { get; set; } = "";
        public int SlaDays { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? LastGeneratedAt { get; set; }
        public List<WorkOrder> GeneratedWorkOrders = new();
    }
}