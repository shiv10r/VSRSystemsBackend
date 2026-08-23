using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VSRSystemsBackend.Application.Platform.ModuleData;

namespace VSRSystemsBackend.Api.Platform.ModuleData;

[ApiController]
public abstract class ModuleDataControllerBase(IModuleDataService service, string module) : ControllerBase
{
    [HttpGet("data/{collection}")]
    public async Task<IActionResult> Get(string collection, CancellationToken cancellationToken)
    {
        var json = await service.GetAsync(module, collection, cancellationToken);
        return json is null ? NotFound() : Content(json, "application/json");
    }

    [HttpPut("data/{collection}")]
    public async Task<IActionResult> Put(string collection, [FromBody] JsonElement value, CancellationToken cancellationToken)
    {
        if (collection.Length is 0 or > 150) return BadRequest();
        await service.SaveAsync(module, collection, value.GetRawText(), cancellationToken);
        return NoContent();
    }
}

[Route("api/school")]
public sealed class SchoolDataController(IModuleDataService service) : ModuleDataControllerBase(service, "school");

[Route("api/hotel")]
public sealed class HotelDataController(IModuleDataService service) : ModuleDataControllerBase(service, "hotel");

[Route("api/news")]
public sealed class NewsDataController(IModuleDataService service) : ModuleDataControllerBase(service, "news");

[Route("api/commerce")]
public sealed class CommerceDataController(IModuleDataService service) : ModuleDataControllerBase(service, "commerce");

[Route("api/bank")]
public sealed class BankDataController(IModuleDataService service) : ModuleDataControllerBase(service, "bank");

[Route("api/medical")]
public sealed class MedicalDataController(IModuleDataService service) : ModuleDataControllerBase(service, "medical");

[Route("api/interior")]
public sealed class InteriorDataController(IModuleDataService service) : ModuleDataControllerBase(service, "interior");

[Route("api/warehouse")]
public sealed class WarehouseDataController(IModuleDataService service) : ModuleDataControllerBase(service, "warehouse");

[Route("api/home-services")]
public sealed class HomeServicesDataController(IModuleDataService service) : ModuleDataControllerBase(service, "home-services");

[Route("api/jobs")]
public sealed class JobsDataController(IModuleDataService service) : ModuleDataControllerBase(service, "jobs");

[Route("api/travel")]
public sealed class TravelDataController(IModuleDataService service) : ModuleDataControllerBase(service, "travel");
