using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Domain.Maintenance;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers;

[ApiController, Authorize, Route("api/railway/maintenance/inventory")]
public sealed class RailwayMaintenanceInventoryController(IRailwayScopeAccessor scopeAccessor, RailwayDbContext db) : ControllerBase
{
    [HttpGet("parts")]
    public async Task<IActionResult> Parts(CancellationToken token) { var s = Read(); return Ok(await db.MaintenanceParts.AsNoTracking().Where(x => x.DivisionId != null && s.DivisionIds.Contains(x.DivisionId.Value)).ToArrayAsync(token)); }
    [HttpPost("parts")]
    public async Task<IActionResult> CreatePart(CreatePartRequest r, CancellationToken token) { var s = Manage(r.DivisionId); var part = new MaintenancePart(Guid.NewGuid(), s.OrganizationId, r.DivisionId, r.Sku, r.Name, r.Unit, r.ReorderLevel); db.Add(part); await db.SaveChangesAsync(token); return Ok(part); }
    [HttpPost("reservations")]
    public async Task<IActionResult> Reserve(ReservePartRequest r, CancellationToken token) { var s = Manage(r.DivisionId); var part = await db.MaintenanceParts.SingleAsync(x => x.Id == r.PartId, token); part.Reserve(r.Quantity); var reservation = new PartReservation(Guid.NewGuid(), s.OrganizationId, r.DivisionId, r.PartId, r.WorkOrderId, r.Quantity, DateTimeOffset.UtcNow); db.Add(reservation); await db.SaveChangesAsync(token); return Ok(reservation); }
    [HttpGet("procurement")]
    public async Task<IActionResult> Requests(CancellationToken token) { var s = Read(); return Ok(await db.ProcurementRequests.AsNoTracking().Where(x => x.DivisionId != null && s.DivisionIds.Contains(x.DivisionId.Value)).ToArrayAsync(token)); }
    [HttpPost("procurement")]
    public async Task<IActionResult> CreateProcurementRequest(ProcurePartRequest r, CancellationToken token) { var s = Manage(r.DivisionId); var request = new ProcurementRequest(Guid.NewGuid(), s.OrganizationId, r.DivisionId, r.PartId, r.Quantity, s.UserId, DateTimeOffset.UtcNow); db.Add(request); await db.SaveChangesAsync(token); return Ok(request); }
    [HttpPost("procurement/{requestId:guid}/purchase-orders")]
    public async Task<IActionResult> Order(Guid requestId, CreatePurchaseOrderRequest r, CancellationToken token) { var s = Manage(r.DivisionId); var request = await db.ProcurementRequests.SingleAsync(x => x.Id == requestId, token); request.Approve(); request.MarkOrdered(); var order = new PurchaseOrder(Guid.NewGuid(), s.OrganizationId, r.DivisionId, requestId, r.VendorName, r.UnitPrice, DateTimeOffset.UtcNow); db.Add(order); await db.SaveChangesAsync(token); return Ok(order); }
    [HttpPost("receipts")]
    public async Task<IActionResult> Receive(ReceivePartRequest r, CancellationToken token) { var s = Manage(r.DivisionId); var order = await db.PurchaseOrders.SingleAsync(x => x.Id == r.PurchaseOrderId, token); var part = await db.MaintenanceParts.SingleAsync(x => x.Id == r.PartId, token); part.Receive(r.Quantity); order.Close(); var receipt = new GoodsReceipt(Guid.NewGuid(), s.OrganizationId, r.DivisionId, order.Id, part.Id, r.Quantity, s.UserId, DateTimeOffset.UtcNow); db.Add(receipt); await db.SaveChangesAsync(token); return Ok(receipt); }
    private RailwayScope Read() { var s = scopeAccessor.GetRequiredScope(); s.RequirePermission("railway.maintenance.read"); return s; }
    private RailwayScope Manage(Guid divisionId) { var s = scopeAccessor.GetRequiredScope(); s.RequirePermission("railway.maintenance.manage"); s.RequireDivision(divisionId); return s; }
}
public sealed record CreatePartRequest(Guid DivisionId, string Sku, string Name, string Unit, int ReorderLevel);
public sealed record ReservePartRequest(Guid DivisionId, Guid PartId, Guid WorkOrderId, int Quantity);
public sealed record ProcurePartRequest(Guid DivisionId, Guid PartId, int Quantity);
public sealed record CreatePurchaseOrderRequest(Guid DivisionId, string VendorName, decimal UnitPrice);
public sealed record ReceivePartRequest(Guid DivisionId, Guid PurchaseOrderId, Guid PartId, int Quantity);
