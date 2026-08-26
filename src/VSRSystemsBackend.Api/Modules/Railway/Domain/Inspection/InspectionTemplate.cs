using System;
using System.Collections.Generic;

namespace VSRSystemsBackend.Api.Domain.Inspection
{
    public class InspectionTemplate
    {
        public Guid Id { get; set; }
        public string Version { get; set; } = "";
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public readonly List<SeverityLevel> SeverityLevels = new();
        public readonly List<string> RequiredEvidenceTypes = new();
        public readonly List<ChecklistItem> ChecklistItems = new();

        public class SeverityLevel
        {
            public string Id { get; set; } = "";
            public string Name { get; set; } = "";
            public string Color { get; set; } = "";
            public int MaxFindings { get; set; }
        }

        public class ChecklistItem
        {
            public string Id { get; set; } = "";
            public string Label { get; set; } = "";
            public string Category { get; set; } = "";
            public bool Required { get; set; }
            public string? MeasurementLimits { get; set; }
        }
    }
}