namespace VSRSystemsBackend.Shared.Constants;

public static class AppConstants
{
    public const string DefaultPageSize = "20";
    public const int MaxPageSize = 100;
    public const string DefaultSortDirection = "asc";
    
    public static class CacheKeys
    {
        public const string Warehouse = "warehouse:";
        public const string Inventory = "inventory:";
        public const string Supplier = "supplier:";
        public const string Customer = "customer:";
        public const string Product = "product:";
    }

    public static class ClaimTypes
    {
        public const string ServiceAccess = "service_access";
        public const string WarehouseAccess = "warehouse_access";
        public const string Role = "role";
        public const string UserId = "user_id";
    }
}

public static class ErrorMessages
{
    public const string NotFound = "Resource not found";
    public const string Unauthorized = "Unauthorized access";
    public const string Forbidden = "Access forbidden";
    public const string ValidationFailed = "Validation failed";
    public const string InternalError = "An internal error occurred";
    public const string ConcurrencyConflict = "The record was modified by another user";
}

public static class SuccessMessages
{
    public const string Created = "Resource created successfully";
    public const string Updated = "Resource updated successfully";
    public const string Deleted = "Resource deleted successfully";
    public const string Retrieved = "Resource retrieved successfully";
}