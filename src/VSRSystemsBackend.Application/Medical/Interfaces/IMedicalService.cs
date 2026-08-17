using VSRSystemsBackend.Application.Medical.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Medical.Interfaces;

public interface IPatientService
{
    Task<Result<PatientDto>> CreateAsync(CreatePatientDto dto, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> GetByPatientNumberAsync(string patientNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PatientDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PatientDto>>> SearchAsync(string searchTerm, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PatientDto>> UpdateAsync(string id, UpdatePatientDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IDoctorService
{
    Task<Result<DoctorDto>> CreateAsync(CreateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<Result<DoctorDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DoctorDto>> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DoctorDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DoctorDto>>> GetBySpecializationAsync(string specialization, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DoctorDto>>> GetAvailableDoctorsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<DoctorDto>> UpdateAsync(string id, UpdateDoctorDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IAppointmentService
{
    Task<Result<AppointmentDto>> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task<Result<AppointmentDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AppointmentDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AppointmentDto>>> GetByPatientIdAsync(string patientId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AppointmentDto>>> GetByDoctorIdAsync(string doctorId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AppointmentDto>>> GetByDateAsync(DateTime date, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<AppointmentDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<AppointmentDto>> UpdateAsync(string id, UpdateAppointmentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<AppointmentDto>> ConfirmAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<AppointmentDto>> CancelAsync(string id, string reason, CancellationToken cancellationToken = default);
    Task<Result<AppointmentDto>> CompleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IMedicalRecordService
{
    Task<Result<MedicalRecordDto>> CreateAsync(CreateMedicalRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result<MedicalRecordDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<MedicalRecordDto>>> GetByPatientIdAsync(string patientId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<MedicalRecordDto>>> GetByDoctorIdAsync(string doctorId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<MedicalRecordDto>> UpdateAsync(string id, UpdateMedicalRecordDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IPrescriptionService
{
    Task<Result<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto, CancellationToken cancellationToken = default);
    Task<Result<PrescriptionDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PrescriptionDto>>> GetByPatientIdAsync(string patientId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PrescriptionDto>>> GetByDoctorIdAsync(string doctorId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PrescriptionDto>>> GetByAppointmentIdAsync(string appointmentId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PrescriptionDto>> UpdateAsync(string id, UpdatePrescriptionDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IPharmacyItemService
{
    Task<Result<PharmacyItemDto>> CreateAsync(CreatePharmacyItemDto dto, CancellationToken cancellationToken = default);
    Task<Result<PharmacyItemDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PharmacyItemDto>> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PharmacyItemDto>>> GetAllAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PharmacyItemDto>>> GetByCategoryAsync(string category, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<PharmacyItemDto>>> GetLowStockAsync(PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PharmacyItemDto>> UpdateAsync(string id, UpdatePharmacyItemDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ILabTestService
{
    Task<Result<LabTestDto>> CreateAsync(CreateLabTestDto dto, CancellationToken cancellationToken = default);
    Task<Result<LabTestDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LabTestDto>>> GetByPatientIdAsync(string patientId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LabTestDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<LabTestDto>> UpdateAsync(string id, UpdateLabTestDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<LabTestDto>> CompleteAsync(string id, string result, CancellationToken cancellationToken = default);
}

public interface IBillingService
{
    Task<Result<MedicalBillingDto>> CreateAsync(CreateMedicalBillingDto dto, CancellationToken cancellationToken = default);
    Task<Result<MedicalBillingDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<MedicalBillingDto>>> GetByPatientIdAsync(string patientId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<MedicalBillingDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<MedicalBillingDto>> UpdateAsync(string id, UpdateMedicalBillingDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<MedicalBillingDto>> PayAsync(string id, decimal amount, string method, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<MedicalBillingDto>>> GetPendingPaymentsAsync(PagedRequest request, CancellationToken cancellationToken = default);
}