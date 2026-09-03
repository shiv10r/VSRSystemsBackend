using VSRSystemsBackend.Api.Modules.Railway.Domain.Shared;

namespace VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;

public sealed class MaintenancePart : RailwayEntity
{
    private MaintenancePart() { }
    public MaintenancePart(Guid id, Guid organizationId, Guid divisionId, string sku, string name, string unit, int reorderLevel)
        : base(id, organizationId, divisionId) { Sku = sku.Trim(); Name = name.Trim(); Unit = unit.Trim(); ReorderLevel = reorderLevel; }
    public string Sku { get; private set; } = string.Empty; public string Name { get; private set; } = string.Empty;
    public string Unit { get; private set; } = string.Empty; public int OnHand { get; private set; } public int Reserved { get; private set; }
    public int ReorderLevel { get; private set; } public int Available => OnHand - Reserved;
    public void Receive(int quantity) { if (quantity <= 0) throw new ArgumentException(); OnHand += quantity; Version++; }
    public void Reserve(int quantity) { if (quantity <= 0 || quantity > Available) throw new InvalidOperationException("Insufficient available stock."); Reserved += quantity; Version++; }
    public void Consume(int quantity) { if (quantity <= 0 || quantity > Reserved) throw new InvalidOperationException(); Reserved -= quantity; OnHand -= quantity; Version++; }
}

public sealed class PartReservation : RailwayEntity
{
    private PartReservation() { }
    public PartReservation(Guid id, Guid organizationId, Guid divisionId, Guid partId, Guid workOrderId, int quantity, DateTimeOffset createdAt)
        : base(id, organizationId, divisionId) { PartId = partId; WorkOrderId = workOrderId; Quantity = quantity; CreatedAt = createdAt; }
    public Guid PartId { get; private set; } public Guid WorkOrderId { get; private set; } public int Quantity { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } public DateTimeOffset? ConsumedAt { get; private set; }
    public void Consume(DateTimeOffset now) { if (ConsumedAt.HasValue) throw new InvalidOperationException(); ConsumedAt = now; Version++; }
}

public sealed class ProcurementRequest : RailwayEntity
{
    private ProcurementRequest() { }
    public ProcurementRequest(Guid id, Guid organizationId, Guid divisionId, Guid partId, int quantity, Guid requestedBy, DateTimeOffset requestedAt)
        : base(id, organizationId, divisionId) { PartId = partId; Quantity = quantity; RequestedBy = requestedBy; RequestedAt = requestedAt; }
    public Guid PartId { get; private set; } public int Quantity { get; private set; } public Guid RequestedBy { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; } public string Status { get; private set; } = "Requested";
    public void Approve() { Status = "Approved"; Version++; } public void MarkOrdered() { Status = "Ordered"; Version++; }
}

public sealed class PurchaseOrder : RailwayEntity
{
    private PurchaseOrder() { }
    public PurchaseOrder(Guid id, Guid organizationId, Guid divisionId, Guid requestId, string vendorName, decimal unitPrice, DateTimeOffset createdAt)
        : base(id, organizationId, divisionId) { RequestId = requestId; VendorName = vendorName.Trim(); UnitPrice = unitPrice; CreatedAt = createdAt; }
    public Guid RequestId { get; private set; } public string VendorName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; } public DateTimeOffset CreatedAt { get; private set; } public string Status { get; private set; } = "Open";
    public void Close() { Status = "Received"; Version++; }
}

public sealed class GoodsReceipt : RailwayEntity
{
    private GoodsReceipt() { }
    public GoodsReceipt(Guid id, Guid organizationId, Guid divisionId, Guid purchaseOrderId, Guid partId, int quantity, Guid receivedBy, DateTimeOffset receivedAt)
        : base(id, organizationId, divisionId) { PurchaseOrderId = purchaseOrderId; PartId = partId; Quantity = quantity; ReceivedBy = receivedBy; ReceivedAt = receivedAt; }
    public Guid PurchaseOrderId { get; private set; } public Guid PartId { get; private set; } public int Quantity { get; private set; }
    public Guid ReceivedBy { get; private set; } public DateTimeOffset ReceivedAt { get; private set; }
}
