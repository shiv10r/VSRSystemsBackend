using Microsoft.AspNetCore.Mvc;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Controllers
{
    [ApiController]
    [Route("api/railway/work-orders")]
    public class RailwayWorkOrdersController : ControllerBase
    {
        [HttpGet]
        public IActionResult List([FromQuery] string? status) => Ok(new { status, items = Array.Empty<object>() });

        [HttpPost]
        public IActionResult Create([FromBody] CreateWorkOrderRequest request) => Ok(new { id = Guid.NewGuid(), request.Priority });

        [HttpPatch("{orderId}/approve")]
        public IActionResult Approve(Guid orderId) => Ok(new { orderId, approved = true });

        [HttpPatch("{orderId}/complete")]
        public IActionResult Complete(Guid orderId, [FromBody] CompleteRequest request) => Ok(new { orderId, request.Reason });
    }

    [ApiController]
    [Route("api/railway/maintenance/plans")]
    public class RailwayMaintenancePlansController : ControllerBase
    {
        [HttpGet]
        public IActionResult List() => Ok(Array.Empty<object>());

        [HttpPost]
        public IActionResult CreatePlan([FromBody] CreatePlanRequest request) => Ok(new { id = Guid.NewGuid(), request.Name });

        [HttpPost("{planId}/generate")]
        public IActionResult GenerateFromPlan(Guid planId, [FromQuery] int count = 1) => Ok(new { planId, count });
    }

    public class CreateWorkOrderRequest
    {
        public Guid SourceId { get; set; }
        public string SourceType { get; set; } = "";
        public string Priority { get; set; } = "Medium";
    }

    public class CompleteRequest
    {
        public string Reason { get; set; } = "";
    }

    public class CreatePlanRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int SlaDays { get; set; }
    }
}