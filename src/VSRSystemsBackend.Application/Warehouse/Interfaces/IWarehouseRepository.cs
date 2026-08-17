using VSRSystemsBackend.Core.Interfaces;
using DomainWarehouse = VSRSystemsBackend.Domain.Warehouse;

namespace VSRSystemsBackend.Application.Warehouse.Interfaces;

public interface IWarehouseRepository : IRepository<DomainWarehouse.Warehouse>
{
    Task<DomainWarehouse.Warehouse?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.Warehouse>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
}

public interface ILocationBinRepository : IRepository<DomainWarehouse.LocationBin>
{
    Task<IReadOnlyList<DomainWarehouse.LocationBin>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.LocationBin?> GetByCodeAsync(string warehouseId, string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.LocationBin>> GetActiveByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
}

public interface IInventoryRepository : IRepository<DomainWarehouse.InventoryItem>
{
    Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetByWarehouseAndLocationAsync(string warehouseId, string location, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.InventoryItem?> GetBySkuAsync(string sku, string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetLowStockAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.InventoryItem>> GetOutOfStockAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<int> GetTotalStockValueAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<Dictionary<string, int>> GetStockByCategoryAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task ReserveStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default);
    Task ReleaseReservedStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default);
}

public interface ISupplierRepository : IRepository<DomainWarehouse.Supplier>
{
    Task<DomainWarehouse.Supplier?> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.Supplier>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByGstinAsync(string gstin, CancellationToken cancellationToken = default);
}

public interface ICustomerRepository : IRepository<DomainWarehouse.Customer>
{
    Task<DomainWarehouse.Customer?> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByGstinAsync(string gstin, CancellationToken cancellationToken = default);
}

public interface IPurchaseOrderRepository : IRepository<DomainWarehouse.PurchaseOrder>
{
    Task<IReadOnlyList<DomainWarehouse.PurchaseOrder>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.PurchaseOrder>> GetBySupplierIdAsync(string supplierId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.PurchaseOrder>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.PurchaseOrder?> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.PurchaseOrder>> GetPendingReceivingAsync(string warehouseId, CancellationToken cancellationToken = default);
}

public interface IGrnRepository : IRepository<DomainWarehouse.GrnRecord>
{
    Task<IReadOnlyList<DomainWarehouse.GrnRecord>> GetByPoIdAsync(string poId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.GrnRecord>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.GrnRecord?> GetByGrnNumberAsync(string grnNumber, CancellationToken cancellationToken = default);
}

public interface ISalesOrderRepository : IRepository<DomainWarehouse.SalesOrder>
{
    Task<IReadOnlyList<DomainWarehouse.SalesOrder>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.SalesOrder>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.SalesOrder>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.SalesOrder?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.SalesOrder>> GetOrdersForPickingAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.SalesOrder>> GetOrdersForDispatchAsync(string warehouseId, CancellationToken cancellationToken = default);
}

public interface IStockTransferRepository : IRepository<DomainWarehouse.StockTransfer>
{
    Task<IReadOnlyList<DomainWarehouse.StockTransfer>> GetByFromWarehouseAsync(string fromWarehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockTransfer>> GetByToWarehouseAsync(string toWarehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockTransfer>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.StockTransfer?> GetByTransferNumberAsync(string transferNumber, CancellationToken cancellationToken = default);
}

public interface IPickListRepository : IRepository<DomainWarehouse.PickList>
{
    Task<IReadOnlyList<DomainWarehouse.PickList>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.PickList>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.PickList?> GetByPickNumberAsync(string pickNumber, CancellationToken cancellationToken = default);
}

public interface IPackageRepository : IRepository<DomainWarehouse.Package>
{
    Task<IReadOnlyList<DomainWarehouse.Package>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.Package>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.Package?> GetByPackageIdAsync(string packageId, CancellationToken cancellationToken = default);
}

public interface IDispatchRepository : IRepository<DomainWarehouse.Dispatch>
{
    Task<IReadOnlyList<DomainWarehouse.Dispatch>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.Dispatch>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.Dispatch?> GetByDispatchNumberAsync(string dispatchNumber, CancellationToken cancellationToken = default);
}

public interface IReturnRepository : IRepository<DomainWarehouse.ReturnRecord>
{
    Task<IReadOnlyList<DomainWarehouse.ReturnRecord>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.ReturnRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.ReturnRecord?> GetByReturnNumberAsync(string returnNumber, CancellationToken cancellationToken = default);
}

public interface IStockCountRepository : IRepository<DomainWarehouse.StockCount>
{
    Task<IReadOnlyList<DomainWarehouse.StockCount>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockCount>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<DomainWarehouse.StockCount?> GetByCountNumberAsync(string countNumber, CancellationToken cancellationToken = default);
}

public interface IStaffRepository : IRepository<DomainWarehouse.StaffMember>
{
    Task<IReadOnlyList<DomainWarehouse.StaffMember>> GetActiveStaffAsync(CancellationToken cancellationToken = default);
}

public interface IProjectRepository : IRepository<DomainWarehouse.ProjectRecord>
{
    Task<IReadOnlyList<DomainWarehouse.ProjectRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.ProjectRecord>> GetByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
}

public interface IStockMovementRepository : IRepository<DomainWarehouse.StockMovement>
{
    Task<IReadOnlyList<DomainWarehouse.StockMovement>> GetByItemIdAsync(string itemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockMovement>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockMovement>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
}

public interface IStockAdjustmentRepository : IRepository<DomainWarehouse.StockAdjustment>
{
    Task<IReadOnlyList<DomainWarehouse.StockAdjustment>> GetByItemIdAsync(string itemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainWarehouse.StockAdjustment>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}