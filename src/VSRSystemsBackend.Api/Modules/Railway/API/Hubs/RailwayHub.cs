using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VSRSystemsBackend.Api.Modules.Railway.Application.Shared;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Persistence;
using VSRSystemsBackend.Api.Modules.Railway.Infrastructure.Realtime;

namespace VSRSystemsBackend.Api.Modules.Railway.API.Hubs;

[Authorize]
public sealed class RailwayHub(
    IRailwayScopeAccessor scopeAccessor,
    RailwayDbContext dbContext) : Hub
{
    public async Task SubscribeToRailwayStation(Guid stationId)
    {
        var scope = scopeAccessor.GetRequiredScope();
        scope.RequirePermission("railway.crowd.read");
        var station = await dbContext.Stations.AsNoTracking().SingleOrDefaultAsync(item => item.Id == stationId);
        if (station?.DivisionId is not { } divisionId || !scope.DivisionIds.Contains(divisionId))
            throw new HubException("The Railway station is outside the authenticated scope.");
        await Groups.AddToGroupAsync(Context.ConnectionId, RailwayRealtimeGroups.Station(scope.OrganizationId, stationId));
    }

    public Task UnsubscribeFromRailwayStation(Guid stationId)
    {
        var scope = scopeAccessor.GetRequiredScope();
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, RailwayRealtimeGroups.Station(scope.OrganizationId, stationId));
    }
}
