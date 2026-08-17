using VSRSystemsBackend.Application.School.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.School.Interfaces;

public interface IStudentService
{
    Task<Result<StudentDto>> CreateAsync(CreateStudentDto dto, CancellationToken cancellationToken = default);
    Task<Result<StudentDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<StudentDto>> GetByAdmissionNoAsync(string admissionNo, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StudentDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StudentDto>>> GetByClassIdAsync(string classId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StudentDto>> UpdateAsync(string id, UpdateStudentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ISchoolClassService
{
    Task<Result<SchoolClassDto>> CreateAsync(CreateSchoolClassDto dto, CancellationToken cancellationToken = default);
    Task<Result<SchoolClassDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SchoolClassDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<SchoolClassDto>> UpdateAsync(string id, UpdateSchoolClassDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IStaffService
{
    Task<Result<StaffDto>> CreateAsync(CreateStaffDto dto, CancellationToken cancellationToken = default);
    Task<Result<StaffDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StaffDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StaffDto>>> GetByDepartmentAsync(string department, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StaffDto>> UpdateAsync(string id, UpdateStaffDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IParentService
{
    Task<Result<ParentDto>> CreateAsync(CreateParentDto dto, CancellationToken cancellationToken = default);
    Task<Result<ParentDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ParentDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<ParentDto>> UpdateAsync(string id, UpdateParentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ISchoolProjectService
{
    Task<Result<SchoolProjectDto>> CreateAsync(CreateSchoolProjectDto dto, CancellationToken cancellationToken = default);
    Task<Result<SchoolProjectDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SchoolProjectDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<SchoolProjectDto>> UpdateAsync(string id, UpdateSchoolProjectDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IStockItemService
{
    Task<Result<StockItemDto>> CreateAsync(CreateStockItemDto dto, CancellationToken cancellationToken = default);
    Task<Result<StockItemDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockItemDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<StockItemDto>>> GetLowStockAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<StockItemDto>> UpdateAsync(string id, UpdateStockItemDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IAdmissionLeadService
{
    Task<Result<AdmissionLeadDto>> CreateAsync(CreateAdmissionLeadDto dto, CancellationToken cancellationToken = default);
    Task<Result<AdmissionLeadDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AdmissionLeadDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AdmissionLeadDto>>> GetByStageAsync(string stage, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<AdmissionLeadDto>> UpdateAsync(string id, UpdateAdmissionLeadDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

// Academic services
public interface IAcademicSessionService
{
    Task<Result<AcademicSessionDto>> CreateAsync(CreateAcademicSessionDto dto, CancellationToken cancellationToken = default);
    Task<Result<AcademicSessionDto>> GetCurrentAsync(CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AcademicSessionDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
}

public interface ISubjectService
{
    Task<Result<SubjectDto>> CreateAsync(CreateSubjectDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SubjectDto>>> GetByClassIdAsync(string classId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<SubjectDto>>> GetByTeacherIdAsync(string teacherId, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface ITimetableService
{
    Task<Result<TimetableSlotDto>> CreateAsync(CreateTimetableSlotDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TimetableSlotDto>>> GetByClassIdAsync(string classId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TimetableSlotDto>>> GetByTeacherAsync(string teacher, PagedRequest request, CancellationToken cancellationToken = default);
}

// Finance services
public interface IFeeService
{
    Task<Result<FeeRecordDto>> CreateAsync(CreateFeeRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<FeeRecordDto>>> GetByStudentIdAsync(string studentId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<FeeRecordDto>>> GetOverdueAsync(PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IReceiptService
{
    Task<Result<ReceiptDto>> CreateAsync(CreateReceiptDto dto, CancellationToken cancellationToken = default);
    Task<Result<ReceiptDto>> GetByReceiptNoAsync(string receiptNo, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<ReceiptDto>>> GetByStudentIdAsync(string studentId, PagedRequest request, CancellationToken cancellationToken = default);
}

// HR services
public interface IPayrollService
{
    Task<Result<PayrollRecordDto>> CreateAsync(CreatePayrollRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PayrollRecordDto>>> GetByStaffIdAsync(string staffId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PayrollRecordDto>>> GetByMonthAsync(string month, PagedRequest request, CancellationToken cancellationToken = default);
}