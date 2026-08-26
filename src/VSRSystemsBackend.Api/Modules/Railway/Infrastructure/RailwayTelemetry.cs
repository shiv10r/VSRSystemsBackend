using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace VSRSystemsBackend.Api.Modules.Railway.Infrastructure;

public static class RailwayTelemetry
{
    public const string MeterName = "VSRSystemsBackend.Railway";
    public static readonly ActivitySource ActivitySource = new(MeterName);
    private static readonly Meter Meter = new(MeterName);
    public static readonly Counter<long> IngestionAccepted = Meter.CreateCounter<long>("railway.crowd.ingestion.accepted");
    public static readonly Counter<long> IngestionRejected = Meter.CreateCounter<long>("railway.crowd.ingestion.rejected");
    public static readonly Counter<long> OutboxDispatched = Meter.CreateCounter<long>("railway.outbox.dispatched");
    public static readonly Counter<long> OutboxFailed = Meter.CreateCounter<long>("railway.outbox.failed");
    public static readonly Counter<long> WorkerFailures = Meter.CreateCounter<long>("railway.worker.failures");
}
