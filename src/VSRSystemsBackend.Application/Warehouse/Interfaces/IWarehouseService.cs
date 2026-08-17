using VSRSystemsBackend.Application.Warehouse.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Warehouse.Interfaces;

public interface IWarehouseService
{
    Task<Result<WarehouseDto>> CreateAsync(CreateWarehouseDto dto, CancellationToken cancellationToken = default);
    Task<Result<WarehouseDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<WarehouseDto>> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<WarehouseDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<WarehouseDto>> UpdateAsync(string id, UpdateWarehouseDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<WarehouseDto>>> GetActiveWarehousesAsync(CancellationToken cancellationToken = default);
}

public interface ILocationBinService
{
    Task<Result<LocationBinDto>> CreateAsync(CreateLocationBinDto dto, CancellationToken cancellationToken = default);
    Task<Result<LocationBinDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LocationBinDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<LocationBinDto>> UpdateAsync(string id, UpdateLocationBinDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<LocationBinDto>>> GetActiveByWarehouseIdAsync(string warehouseId, CancellationToken cancellationToken = default);
}

public interface IInventoryService
{
    Task<Result<InventoryItemDto>> CreateAsync(CreateInventoryItemDto dto, CancellationToken cancellationToken = default);
    Task<Result<InventoryItemDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InventoryItemDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<InventoryItemDto>> GetBySkuAsync(string sku, string warehouseId, CancellationToken cancellationToken = default);
    Task<Result<InventoryItemDto>> UpdateAsync(string id, UpdateInventoryItemDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InventoryItemDto>>> GetLowStockAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<InventoryItemDto>>> GetOutOfStockAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<int>> GetTotalStockValueAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<Result<Dictionary<string, int>>> GetStockByCategoryAsync(string warehouseId, CancellationToken cancellationToken = default);
    Task<Result> AdjustStockAsync(string itemId, int quantity, string reason, CancellationToken cancellationToken = default);
    Task<Result> ReserveStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default);
    Task<Result> ReleaseReservedStockAsync(string itemId, int quantity, CancellationToken cancellationToken = default);
}

public interface ISupplierService
{
    Task<Result<SupplierDto>> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<Result<SupplierDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<SupplierDto>> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SupplierDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<SupplierDto>> UpdateAsync(string id, UpdateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<SupplierDto>>> GetActiveSuppliersAsync(CancellationToken cancellationToken = default);
}

public interface ICustomerService
{
    Task<Result<CustomerDto>> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CustomerDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<CustomerDto>> UpdateAsync(string id, UpdateCustomerDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CustomerDto>>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
}

public interface IPurchaseOrderService
{
    Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> GetByPoNumberAsync(string poNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PurchaseOrderDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PurchaseOrderDto>>> GetBySupplierIdAsync(string supplierId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PurchaseOrderDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> UpdateAsync(string id, UpdatePurchaseOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> SubmitAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> ApproveAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> ReceiveAsync(string id, ReceivePurchaseOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PurchaseOrderDto>>> GetPendingReceivingAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IGrnService
{
    Task<Result<GrnRecordDto>> CreateAsync(CreateGrnRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<GrnRecordDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<GrnRecordDto>> GetByGrnNumberAsync(string grnNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GrnRecordDto>>> GetByPoIdAsync(string poId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<GrnRecordDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<GrnRecordDto>> UpdateAsync(string id, UpdateGrnRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ISalesOrderService
{
    Task<Result<SalesOrderDto>> CreateAsync(CreateSalesOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result<SalesOrderDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<SalesOrderDto>> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SalesOrderDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SalesOrderDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SalesOrderDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<SalesOrderDto>> UpdateAsync(string id, UpdateSalesOrderDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<SalesOrderDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<SalesOrderDto>> ReserveAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SalesOrderDto>>> GetOrdersForPickingAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SalesOrderDto>>> GetOrdersForDispatchAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IStockTransferService
{
    Task<Result<StockTransferDto>> CreateAsync(CreateStockTransferDto dto, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> GetByTransferNumberAsync(string transferNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockTransferDto>>> GetByFromWarehouseAsync(string fromWarehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockTransferDto>>> GetByToWarehouseAsync(string toWarehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockTransferDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> UpdateAsync(string id, UpdateStockTransferDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> DispatchAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> ReceiveAsync(string id, ReceiveStockTransferDto dto, CancellationToken cancellationToken = default);
    Task<Result<StockTransferDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IPickListService
{
    Task<Result<PickListDto>> CreateAsync(CreatePickListDto dto, CancellationToken cancellationToken = default);
    Task<Result<PickListDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PickListDto>> GetByPickNumberAsync(string pickNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PickListDto>>> GetByOrderIdAsync(string orderId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PickListDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PickListDto>> UpdateAsync(string id, UpdatePickListDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PickListDto>> StartPickingAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PickListDto>> CompletePickingAsync(string id, CancellationToken cancellationToken = default);
}

public interface IPackageService
{
    Task<Result<PackageDto>> CreateAsync(CreatePackageDto dto, CancellationToken cancellationToken = default);
    Task<Result<PackageDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PackageDto>> GetByPackageIdAsync(string packageId, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PackageDto>>> GetByOrderIdAsync(string orderId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PackageDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PackageDto>> UpdateAsync(string id, UpdatePackageDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IDispatchService
{
    Task<Result<DispatchDto>> CreateAsync(CreateDispatchDto dto, CancellationToken cancellationToken = default);
    Task<Result<DispatchDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DispatchDto>> GetByDispatchNumberAsync(string dispatchNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DispatchDto>>> GetByOrderIdAsync(string orderId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DispatchDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<DispatchDto>> UpdateAsync(string id, UpdateDispatchDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DispatchDto>> DispatchAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DispatchDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IReturnService
{
    Task<Result<ReturnRecordDto>> CreateAsync(CreateReturnRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> GetByReturnNumberAsync(string returnNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReturnRecordDto>>> GetByTypeAsync(string type, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReturnRecordDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> UpdateAsync(string id, UpdateReturnRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> ReceiveAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> InspectAsync(string id, InspectReturnDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReturnRecordDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IStockCountService
{
    Task<Result<StockCountDto>> CreateAsync(CreateStockCountDto dto, CancellationToken cancellationToken = default);
    Task<Result<StockCountDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StockCountDto>> GetByCountNumberAsync(string countNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockCountDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockCountDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StockCountDto>> UpdateAsync(string id, UpdateStockCountDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StockCountDto>> ApproveAsync(string id, CancellationToken cancellationToken = default);
}

public interface IStaffService
{
    Task<Result<StaffMemberDto>> CreateAsync(CreateStaffMemberDto dto, CancellationToken cancellationToken = default);
    Task<Result<StaffMemberDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StaffMemberDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StaffMemberDto>> UpdateAsync(string id, UpdateStaffMemberDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<StaffMemberDto>>> GetActiveStaffAsync(CancellationToken cancellationToken = default);
}

public interface IProjectService
{
    Task<Result<ProjectRecordDto>> CreateAsync(CreateProjectRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<ProjectRecordDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProjectRecordDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProjectRecordDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ProjectRecordDto>>> GetByWarehouseIdAsync(string warehouseId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProjectRecordDto>> UpdateAsync(string id, UpdateProjectRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IStockMovementService
{
    Task<Result<PagedResult<StockMovementDto>>> GetByItemIdAsync(string itemId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockMovementDto>>> GetByDateRangeAsync(DateTime from, DateTime to, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockMovementDto>>> GetByTypeAsync(string type, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IStockAdjustmentService
{
    Task<Result<StockAdjustmentDto>> CreateAsync(CreateStockAdjustmentDto dto, CancellationToken cancellationToken = default);
    Task<Result<StockAdjustmentDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockAdjustmentDto>>> GetByItemIdAsync(string itemId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockAdjustmentDto>>> GetByDateRangeAsync(DateTime from, DateTime to, PagedRequest request, CancellationToken cancellationToken = default);
}