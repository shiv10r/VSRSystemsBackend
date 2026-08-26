using VSRSystemsBackend.Api.Domain.CrowdOperations;

namespace VSRSystemsBackend.Api.Application.CrowdOperations
{
    public class CrowdHandlers
    {
        public CrowdDataQuality CalculateQuality(NormalizedCrowdObservation obs, DateTimeOffset now)
        {
            var age = now - obs.WindowEnd;
            if (age.TotalSeconds > 300 || obs.Confidence < 0.5m)
            {
                return CrowdDataQuality.Degraded;
            }
            return CrowdDataQuality.Good;
        }

        public CrowdRiskLevel CalculateRisk(int count, int warningThreshold, int criticalThreshold)
        {
            if (count >= criticalThreshold) return CrowdRiskLevel.Critical;
            if (count >= warningThreshold) return CrowdRiskLevel.Warning;
            return CrowdRiskLevel.Normal;
        }

        /// <summary>
        /// Idempotent by SourceEventId - duplicate deliveries are ignored.
        /// </summary>
        private readonly HashSet<string> _processedSourceEvents = new();

        public bool TryIngest(NormalizedCrowdObservation observation)
        {
            return _processedSourceEvents.Add(observation.SourceEventId);
        }

        public CrowdAlert RaiseAlert(Guid zoneId, CrowdRiskLevel level)
        {
            return new CrowdAlert { StationZoneId = zoneId, Level = level };
        }

        public CrowdIncident OpenIncident(Guid stationId, string title)
        {
            return new CrowdIncident { StationId = stationId, Title = title };
        }
    }
}