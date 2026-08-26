using System;
using System.Collections.Generic;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

namespace VSRSystemsBackend.Api.Domain.Inspection
{
    public class Defect
    {
        public Guid Id { get; set; }
        public Guid InspectionRunId { get; set; }
        public string Description { get; set; } = "";
        public DefectSeverity Severity { get; set; } = DefectSeverity.Low;
        public DefectStatus Status { get; set; } = DefectStatus.Open;
        public DateTime RaisedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public Guid? AssignedWorkOrderId { get; set; }
        public virtual InspectionRun? InspectionRun { get; set; }
    }
}