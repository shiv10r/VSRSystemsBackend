using VSRSystemsBackend.Core.Interfaces;
using DomainSchool = VSRSystemsBackend.Domain.School;

namespace VSRSystemsBackend.Application.School.Interfaces;

public interface IStudentRepository : IRepository<DomainSchool.Student>
{
    Task<IReadOnlyList<DomainSchool.Student>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<DomainSchool.Student?> GetByAdmissionNoAsync(string admissionNo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Student>> GetActiveStudentsAsync(CancellationToken cancellationToken = default);
}

public interface ISchoolClassRepository : IRepository<DomainSchool.SchoolClass>
{
    Task<IReadOnlyList<DomainSchool.SchoolClass>> GetByTeacherAsync(string teacher, CancellationToken cancellationToken = default);
}

public interface IStaffRepository : IRepository<DomainSchool.SchoolStaffMember>
{
    Task<IReadOnlyList<DomainSchool.SchoolStaffMember>> GetByDepartmentAsync(string department, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.SchoolStaffMember>> GetActiveStaffAsync(CancellationToken cancellationToken = default);
}

public interface IParentRepository : IRepository<DomainSchool.ParentRecord>
{
    Task<DomainSchool.ParentRecord?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
}

public interface ISchoolProjectRepository : IRepository<DomainSchool.SchoolProject>
{
    Task<IReadOnlyList<DomainSchool.SchoolProject>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IStockItemRepository : IRepository<DomainSchool.StockItem>
{
    Task<IReadOnlyList<DomainSchool.StockItem>> GetLowStockAsync(CancellationToken cancellationToken = default);
}

public interface IAdmissionLeadRepository : IRepository<DomainSchool.AdmissionLead>
{
    Task<IReadOnlyList<DomainSchool.AdmissionLead>> GetByStageAsync(string stage, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AdmissionLead>> GetByFollowUpDateAsync(DateTime date, CancellationToken cancellationToken = default);
}

public interface IAcademicSessionRepository : IRepository<DomainSchool.AcademicSession>
{
    Task<DomainSchool.AcademicSession?> GetCurrentSessionAsync(CancellationToken cancellationToken = default);
}

public interface ISubjectRepository : IRepository<DomainSchool.Subject>
{
    Task<IReadOnlyList<DomainSchool.Subject>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Subject>> GetByTeacherIdAsync(string teacherId, CancellationToken cancellationToken = default);
}

public interface ITimetableRepository : IRepository<DomainSchool.TimetableSlot>
{
    Task<IReadOnlyList<DomainSchool.TimetableSlot>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.TimetableSlot>> GetByTeacherAsync(string teacher, CancellationToken cancellationToken = default);
}

public interface IHomeworkRepository : IRepository<DomainSchool.Homework>
{
    Task<IReadOnlyList<DomainSchool.Homework>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Homework>> GetPublishedHomeworkAsync(CancellationToken cancellationToken = default);
}

public interface ICourseRepository : IRepository<DomainSchool.Course>
{
    Task<IReadOnlyList<DomainSchool.Course>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Course>> GetPublishedCoursesAsync(CancellationToken cancellationToken = default);
}

public interface ILessonRepository : IRepository<DomainSchool.Lesson>
{
    Task<IReadOnlyList<DomainSchool.Lesson>> GetByCourseIdAsync(string courseId, CancellationToken cancellationToken = default);
}

public interface IAttendanceRepository : IRepository<DomainSchool.AttendanceRecord>
{
    Task<IReadOnlyList<DomainSchool.AttendanceRecord>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AttendanceRecord>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AttendanceRecord>> GetByClassAndDateAsync(string className, DateTime date, CancellationToken cancellationToken = default);
}

public interface ILeaveRepository : IRepository<DomainSchool.LeaveRequest>
{
    Task<IReadOnlyList<DomainSchool.LeaveRequest>> GetByPersonAsync(string personType, string personId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.LeaveRequest>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IQuestionRepository : IRepository<DomainSchool.Question>
{
    Task<IReadOnlyList<DomainSchool.Question>> GetBySubjectAsync(string subject, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Question>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
}

public interface IOnlineExamRepository : IRepository<DomainSchool.OnlineExam>
{
    Task<IReadOnlyList<DomainSchool.OnlineExam>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.OnlineExam>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ITestAttemptRepository : IRepository<DomainSchool.TestAttempt>
{
    Task<IReadOnlyList<DomainSchool.TestAttempt>> GetByExamIdAsync(string examId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.TestAttempt>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
}

public interface IExamScheduleRepository : IRepository<DomainSchool.ExamSchedule>
{
    Task<IReadOnlyList<DomainSchool.ExamSchedule>> GetByClassIdAsync(string classId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.ExamSchedule>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
}

public interface IMarksRepository : IRepository<DomainSchool.MarksEntry>
{
    Task<IReadOnlyList<DomainSchool.MarksEntry>> GetByExamIdAsync(string examId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.MarksEntry>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
}

public interface IResultRepository : IRepository<DomainSchool.ResultRecord>
{
    Task<IReadOnlyList<DomainSchool.ResultRecord>> GetByExamIdAsync(string examId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.ResultRecord>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
}

public interface IFeeRecordRepository : IRepository<DomainSchool.FeeRecord>
{
    Task<IReadOnlyList<DomainSchool.FeeRecord>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.FeeRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.FeeRecord>> GetOverdueFeesAsync(CancellationToken cancellationToken = default);
}

public interface IFeeStructureRepository : IRepository<DomainSchool.FeeStructure>
{
    Task<IReadOnlyList<DomainSchool.FeeStructure>> GetByClassNameAsync(string className, CancellationToken cancellationToken = default);
}

public interface IReceiptRepository : IRepository<DomainSchool.Receipt>
{
    Task<IReadOnlyList<DomainSchool.Receipt>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<DomainSchool.Receipt?> GetByReceiptNoAsync(string receiptNo, CancellationToken cancellationToken = default);
}

public interface IExpenseRepository : IRepository<DomainSchool.ExpenseRecord>
{
    Task<IReadOnlyList<DomainSchool.ExpenseRecord>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.ExpenseRecord>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface IPayrollRepository : IRepository<DomainSchool.PayrollRecord>
{
    Task<IReadOnlyList<DomainSchool.PayrollRecord>> GetByStaffIdAsync(string staffId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.PayrollRecord>> GetByMonthAsync(string month, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.PayrollRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IJobOpeningRepository : IRepository<DomainSchool.JobOpening>
{
    Task<IReadOnlyList<DomainSchool.JobOpening>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IApplicantRepository : IRepository<DomainSchool.Applicant>
{
    Task<IReadOnlyList<DomainSchool.Applicant>> GetByJobIdAsync(string jobId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Applicant>> GetByStageAsync(string stage, CancellationToken cancellationToken = default);
}

public interface IPerformanceReviewRepository : IRepository<DomainSchool.PerformanceReview>
{
    Task<IReadOnlyList<DomainSchool.PerformanceReview>> GetByStaffIdAsync(string staffId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.PerformanceReview>> GetByPeriodAsync(string period, CancellationToken cancellationToken = default);
}

public interface ITrainingRepository : IRepository<DomainSchool.TrainingProgram>
{
    Task<IReadOnlyList<DomainSchool.TrainingProgram>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IVehicleRepository : IRepository<DomainSchool.Vehicle>
{
    Task<IReadOnlyList<DomainSchool.Vehicle>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ITransportRouteRepository : IRepository<DomainSchool.TransportRoute>
{
    Task<IReadOnlyList<DomainSchool.TransportRoute>> GetByVehicleIdAsync(string vehicleId, CancellationToken cancellationToken = default);
}

public interface ILibraryBookRepository : IRepository<DomainSchool.LibraryBook>
{
    Task<DomainSchool.LibraryBook?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default);
}

public interface ILibraryIssueRepository : IRepository<DomainSchool.LibraryIssue>
{
    Task<IReadOnlyList<DomainSchool.LibraryIssue>> GetByMemberAsync(string memberType, string memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.LibraryIssue>> GetOverdueIssuesAsync(CancellationToken cancellationToken = default);
}

public interface IVendorRepository : IRepository<DomainSchool.Vendor>
{
    Task<DomainSchool.Vendor?> GetByGstAsync(string gst, CancellationToken cancellationToken = default);
}

public interface ISchoolPurchaseOrderRepository : IRepository<DomainSchool.SchoolPurchaseOrder>
{
    Task<IReadOnlyList<DomainSchool.SchoolPurchaseOrder>> GetByVendorIdAsync(string vendorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.SchoolPurchaseOrder>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IAssetRepository : IRepository<DomainSchool.AssetRecord>
{
    Task<IReadOnlyList<DomainSchool.AssetRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AssetRecord>> GetByLocationAsync(string location, CancellationToken cancellationToken = default);
}

public interface IVisitorLogRepository : IRepository<DomainSchool.VisitorLog>
{
    Task<IReadOnlyList<DomainSchool.VisitorLog>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.VisitorLog>> GetActiveVisitorsAsync(CancellationToken cancellationToken = default);
}

public interface IHostelRoomRepository : IRepository<DomainSchool.HostelRoom>
{
    Task<IReadOnlyList<DomainSchool.HostelRoom>> GetByHostelAsync(string hostel, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.HostelRoom>> GetAvailableRoomsAsync(CancellationToken cancellationToken = default);
}

public interface IHostelAllocationRepository : IRepository<DomainSchool.HostelAllocation>
{
    Task<IReadOnlyList<DomainSchool.HostelAllocation>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.HostelAllocation>> GetByRoomIdAsync(string roomId, CancellationToken cancellationToken = default);
}

public interface IMealPlanRepository : IRepository<DomainSchool.MealPlan>
{
    Task<IReadOnlyList<DomainSchool.MealPlan>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.MealPlan>> GetActivePlansAsync(CancellationToken cancellationToken = default);
}

public interface IClubRepository : IRepository<DomainSchool.Club>
{
    Task<IReadOnlyList<DomainSchool.Club>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ISportsTeamRepository : IRepository<DomainSchool.SportsTeam>
{
    Task<IReadOnlyList<DomainSchool.SportsTeam>> GetBySportAsync(string sport, CancellationToken cancellationToken = default);
}

public interface IFixtureRepository : IRepository<DomainSchool.Fixture>
{
    Task<IReadOnlyList<DomainSchool.Fixture>> GetByTeamIdAsync(string teamId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Fixture>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
}

public interface IHouseRepository : IRepository<DomainSchool.House>
{
    Task<DomainSchool.House?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

public interface IHousePointRepository : IRepository<DomainSchool.HousePoint>
{
    Task<IReadOnlyList<DomainSchool.HousePoint>> GetByHouseIdAsync(string houseId, CancellationToken cancellationToken = default);
}

public interface IDisciplineRepository : IRepository<DomainSchool.DisciplineRecord>
{
    Task<IReadOnlyList<DomainSchool.DisciplineRecord>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.DisciplineRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ICounsellingRepository : IRepository<DomainSchool.CounsellingSession>
{
    Task<IReadOnlyList<DomainSchool.CounsellingSession>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.CounsellingSession>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface INoticeRepository : IRepository<DomainSchool.Notice>
{
    Task<IReadOnlyList<DomainSchool.Notice>> GetByAudienceAsync(string audience, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Notice>> GetPublishedNoticesAsync(CancellationToken cancellationToken = default);
}

public interface ICalendarEventRepository : IRepository<DomainSchool.CalendarEvent>
{
    Task<IReadOnlyList<DomainSchool.CalendarEvent>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.CalendarEvent>> GetByAudienceAsync(string audience, CancellationToken cancellationToken = default);
}

public interface IMessageRepository : IRepository<DomainSchool.Message>
{
    Task<IReadOnlyList<DomainSchool.Message>> GetByChannelAsync(string channel, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Message>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface INotificationRepository : IRepository<DomainSchool.Notification>
{
    Task<IReadOnlyList<DomainSchool.Notification>> GetByAudienceAsync(string audience, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Notification>> GetUnreadNotificationsAsync(string audience, CancellationToken cancellationToken = default);
}

public interface IPTMSessionRepository : IRepository<DomainSchool.PTMSession>
{
    Task<IReadOnlyList<DomainSchool.PTMSession>> GetByTeacherIdAsync(string teacherId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.PTMSession>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ISurveyRepository : IRepository<DomainSchool.Survey>
{
    Task<IReadOnlyList<DomainSchool.Survey>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IDocumentRepository : IRepository<DomainSchool.DocumentRecord>
{
    Task<IReadOnlyList<DomainSchool.DocumentRecord>> GetByOwnerAsync(string ownerType, string ownerName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.DocumentRecord>> GetExpiringDocumentsAsync(CancellationToken cancellationToken = default);
}

public interface ICertificateTemplateRepository : IRepository<DomainSchool.CertificateTemplate>
{
    Task<IReadOnlyList<DomainSchool.CertificateTemplate>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
}

public interface ICertificateRepository : IRepository<DomainSchool.Certificate>
{
    Task<IReadOnlyList<DomainSchool.Certificate>> GetByTemplateIdAsync(string templateId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Certificate>> GetByStudentIdAsync(string studentId, CancellationToken cancellationToken = default);
}

public interface ITicketRepository : IRepository<DomainSchool.Ticket>
{
    Task<IReadOnlyList<DomainSchool.Ticket>> GetByAssigneeAsync(string assignee, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Ticket>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.Ticket>> GetByPriorityAsync(string priority, CancellationToken cancellationToken = default);
}

public interface IGrievanceRepository : IRepository<DomainSchool.Grievance>
{
    Task<IReadOnlyList<DomainSchool.Grievance>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IIncidentRepository : IRepository<DomainSchool.IncidentRecord>
{
    Task<IReadOnlyList<DomainSchool.IncidentRecord>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.IncidentRecord>> GetBySeverityAsync(string severity, CancellationToken cancellationToken = default);
}

public interface ITaskRepository : IRepository<DomainSchool.TaskItem>
{
    Task<IReadOnlyList<DomainSchool.TaskItem>> GetByAssigneeAsync(string assignee, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.TaskItem>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IUserAccountRepository : IRepository<DomainSchool.UserAccount>
{
    Task<DomainSchool.UserAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.UserAccount>> GetByRoleAsync(string role, CancellationToken cancellationToken = default);
}

public interface IAuditLogRepository : IRepository<DomainSchool.AuditLog>
{
    Task<IReadOnlyList<DomainSchool.AuditLog>> GetByUserAsync(string user, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AuditLog>> GetByEntityAsync(string entity, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainSchool.AuditLog>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface ISchoolSettingRepository : IRepository<DomainSchool.SchoolSetting>
{
    Task<DomainSchool.SchoolSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
}