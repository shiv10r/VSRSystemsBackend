using VSRSystemsBackend.Api.Domain.Inspection;
using VSRSystemsBackend.Api.Domain.Inspection.Enums;

namespace VSRSystemsBackend.Api.Application.Inspection
{
    public class InspectionHandlers
    {
        public InspectionRun CreateInspectionRun(Guid organizationId, Guid divisionId, string templateVersion, Guid stationId)
        {
            return new InspectionRun
            {
                Id = Guid.NewGuid(),
                TemplateVersion = templateVersion,
                Status = InspectionRunStatus.Draft,
                StartedAt = DateTime.UtcNow,
                StationId = stationId,
                ExpectedVersion = 1,
                OrganizationId = organizationId,
                DivisionId = divisionId
            };
        }

        public InspectionRun SubmitFinding(InspectionRun run, string itemId, string response)
        {
            run.ActualVersion++;
            run.Findings.Add(new Finding { ItemId = itemId, Response = response });
            return run;
        }

        public InspectionRun Submit(InspectionRun run, string submittedBy, DateTime now)
        {
            run.Status = InspectionRunStatus.Submitted;
            run.AssignedInspector = submittedBy;
            run.CompletedAt = now;
            return run;
        }

        public Defect RaiseDefect(InspectionRun run, string description, DefectSeverity severity)
        {
            var defect = new Defect
            {
                Id = Guid.NewGuid(),
                InspectionRunId = run.Id,
                Description = description,
                Severity = severity,
                Status = DefectStatus.Open,
                RaisedAt = DateTime.UtcNow
            };
            run.Defects.Add(defect);
            return defect;
        }

        public Defect ResolveDefect(Defect defect, bool accepted, string reason)
        {
            defect.Status = accepted ? DefectStatus.Resolved : DefectStatus.Rejected;
            defect.ResolvedAt = DateTime.UtcNow;
            return defect;
        }

        public InspectionRun AmendRun(InspectionRun run, string description, string changedBy)
        {
            run.Status = InspectionRunStatus.Amended;
            run.Amendments.Add(new InspectionAmendment
            {
                ChangedBy = changedBy,
                Description = description
            });
            return run;
        }
    }
}