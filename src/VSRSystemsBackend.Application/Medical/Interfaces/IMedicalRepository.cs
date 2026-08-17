using VSRSystemsBackend.Core.Interfaces;
using DomainMedical = VSRSystemsBackend.Domain.Medical;

namespace VSRSystemsBackend.Application.Medical.Interfaces;

public interface IPatientRepository : IRepository<DomainMedical.Patient>
{
    Task<DomainMedical.Patient?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default);
    Task<DomainMedical.Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<DomainMedical.Patient?> GetByPatientNumberAsync(string patientNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

public interface IDoctorRepository : IRepository<DomainMedical.Doctor>
{
    Task<IReadOnlyList<DomainMedical.Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Doctor>> GetAvailableDoctorsAsync(CancellationToken cancellationToken = default);
    Task<DomainMedical.Doctor?> GetByLicenseNumberAsync(string licenseNumber, CancellationToken cancellationToken = default);
}

public interface IAppointmentRepository : IRepository<DomainMedical.Appointment>
{
    Task<IReadOnlyList<DomainMedical.Appointment>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Appointment>> GetByDoctorIdAsync(string doctorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Appointment>> GetByDateAsync(DateTime date, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Appointment>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Appointment>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
}

public interface IMedicalRecordRepository : IRepository<DomainMedical.MedicalRecord>
{
    Task<IReadOnlyList<DomainMedical.MedicalRecord>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.MedicalRecord>> GetByDoctorIdAsync(string doctorId, CancellationToken cancellationToken = default);
}

public interface IPrescriptionRepository : IRepository<DomainMedical.Prescription>
{
    Task<IReadOnlyList<DomainMedical.Prescription>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Prescription>> GetByDoctorIdAsync(string doctorId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.Prescription>> GetByAppointmentIdAsync(string appointmentId, CancellationToken cancellationToken = default);
}

public interface IPharmacyItemRepository : IRepository<DomainMedical.PharmacyItem>
{
    Task<IReadOnlyList<DomainMedical.PharmacyItem>> GetLowStockAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.PharmacyItem>> GetByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<DomainMedical.PharmacyItem?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
}

public interface ILabTestRepository : IRepository<DomainMedical.LabTest>
{
    Task<IReadOnlyList<DomainMedical.LabTest>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.LabTest>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IBillingRepository : IRepository<DomainMedical.MedicalBilling>
{
    Task<IReadOnlyList<DomainMedical.MedicalBilling>> GetByPatientIdAsync(string patientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.MedicalBilling>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DomainMedical.MedicalBilling>> GetPendingPaymentsAsync(CancellationToken cancellationToken = default);
}