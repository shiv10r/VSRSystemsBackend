using VSRSystemsBackend.Application.Bank.DTOs;
using VSRSystemsBackend.Core.Common;

namespace VSRSystemsBackend.Application.Bank.Interfaces;

public interface IBankAccountService
{
    Task<Result<BankAccountDto>> CreateAsync(CreateBankAccountDto dto, CancellationToken cancellationToken = default);
    Task<Result<BankAccountDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BankAccountDto>> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BankAccountDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BankAccountDto>>> GetByTypeAsync(string type, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BankAccountDto>> UpdateAsync(string id, UpdateBankAccountDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BankAccountDto>> DepositAsync(string id, decimal amount, string description, CancellationToken cancellationToken = default);
    Task<Result<BankAccountDto>> WithdrawAsync(string id, decimal amount, string description, CancellationToken cancellationToken = default);
}

public interface ITransactionService
{
    Task<Result<TransactionDto>> CreateAsync(CreateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TransactionDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TransactionDto>>> GetByDateRangeAsync(DateTime from, DateTime to, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<TransactionDto>>> GetByTypeAsync(string type, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<TransactionDto>> UpdateAsync(string id, UpdateTransactionDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface IBeneficiaryService
{
    Task<Result<BeneficiaryDto>> CreateAsync(CreateBeneficiaryDto dto, CancellationToken cancellationToken = default);
    Task<Result<BeneficiaryDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BeneficiaryDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BeneficiaryDto>> UpdateAsync(string id, UpdateBeneficiaryDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface ICardService
{
    Task<Result<CardDto>> CreateAsync(CreateCardDto dto, CancellationToken cancellationToken = default);
    Task<Result<CardDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CardDto>> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<CardDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<CardDto>> UpdateAsync(string id, UpdateCardDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CardDto>> BlockAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<CardDto>> UnblockAsync(string id, CancellationToken cancellationToken = default);
}

public interface ILoanService
{
    Task<Result<LoanDto>> CreateAsync(CreateLoanDto dto, CancellationToken cancellationToken = default);
    Task<Result<LoanDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LoanDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LoanDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<LoanDto>> UpdateAsync(string id, UpdateLoanDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<LoanDto>> ApproveAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<LoanDto>> DisburseAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<LoanDto>>> GetOverdueLoansAsync(PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IDepositService
{
    Task<Result<DepositDto>> CreateAsync(CreateDepositDto dto, CancellationToken cancellationToken = default);
    Task<Result<DepositDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DepositDto>>> GetByCustomerIdAsync(string customerId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<DepositDto>> UpdateAsync(string id, UpdateDepositDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<DepositDto>> MatureAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<DepositDto>>> GetMaturingDepositsAsync(DateTime beforeDate, PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IBillService
{
    Task<Result<BillDto>> CreateAsync(CreateBillDto dto, CancellationToken cancellationToken = default);
    Task<Result<BillDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BillDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BillDto>>> GetByStatusAsync(string status, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BillDto>> UpdateAsync(string id, UpdateBillDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<BillDto>> PayAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BillDto>>> GetDueBillsAsync(PagedRequest request, CancellationToken cancellationToken = default);
}

public interface IDocumentService
{
    Task<Result<BankDocumentDto>> CreateAsync(CreateBankDocumentDto dto, CancellationToken cancellationToken = default);
    Task<Result<BankDocumentDto>> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BankDocumentDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BankDocumentDto>> UpdateAsync(string id, UpdateBankDocumentDto dto, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}

public interface INotificationService
{
    Task<Result<BankNotificationDto>> CreateAsync(CreateBankNotificationDto dto, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BankNotificationDto>>> GetByAccountIdAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<PagedResult<BankNotificationDto>>> GetUnreadAsync(string accountId, PagedRequest request, CancellationToken cancellationToken = default);
    Task<Result<BankNotificationDto>> MarkAsReadAsync(string id, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string id, CancellationToken cancellationToken = default);
}