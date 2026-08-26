using System;
using System.Collections.Generic;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

namespace VSRSystemsBackend.Api.Domain.Inspection
{
    public class InspectionRun
    {
        public Guid Id { get; set; }
        public string TemplateVersion { get; set; } = "";
        public InspectionRunStatus Status { get; set; } = InspectionRunStatus.Draft;
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string AssignedInspector { get; set; } = "";
        public Guid StationId { get; set; }
        public int ExpectedVersion { get; set; } = 1;
        public int ActualVersion { get; set; }
        public List<Finding> Findings = new();
        public List<Defect> Defects = new();
        public List<InspectionAmendment> Amendments = new();
        public string CreatedBy { get; set; } = "";
        public Guid OrganizationId { get; set; }
        public Guid DivisionId { get; set; }
    }

    public class Finding
    {
        public string ItemId { get; set; } = "";
        public string Response { get; set; } = "";
        public string? Measurement { get; set; }
        public string? Unit { get; set; }
    }

    public class InspectionAmendment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ChangedBy { get; set; } = "";
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = "";
    }
}