using VSRSystemsBackend.Core.Interfaces;
using VSRSystemsBackend.Domain.Bank;

namespace VSRSystemsBackend.Application.Bank.Interfaces;

public interface IBankAccountRepository : IRepository<BankAccount>
{
    Task<BankAccount?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankAccount>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankAccount>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankAccount>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface ITransactionRepository : IRepository<Transaction>
{
    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaction>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
}

public interface IBeneficiaryRepository : IRepository<Beneficiary>
{
    Task<IReadOnlyList<Beneficiary>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<Beneficiary?> GetByAccountAndNicknameAsync(string accountId, string nickname, CancellationToken cancellationToken = default);
}

public interface ICardRepository : IRepository<Card>
{
    Task<IReadOnlyList<Card>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Card>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<Card?> GetByCardNumberAsync(string cardNumber, CancellationToken cancellationToken = default);
}

public interface ILoanRepository : IRepository<Loan>
{
    Task<IReadOnlyList<Loan>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Loan>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Loan>> GetOverdueLoansAsync(CancellationToken cancellationToken = default);
}

public interface IDepositRepository : IRepository<Deposit>
{
    Task<IReadOnlyList<Deposit>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Deposit>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Deposit>> GetMaturingDepositsAsync(DateTime beforeDate, CancellationToken cancellationToken = default);
}

public interface IBillRepository : IRepository<Bill>
{
    Task<IReadOnlyList<Bill>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bill>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Bill>> GetDueBillsAsync(CancellationToken cancellationToken = default);
}

public interface IDocumentRepository : IRepository<BankDocument>
{
    Task<IReadOnlyList<BankDocument>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankDocument>> GetByTypeAsync(string type, CancellationToken cancellationToken = default);
}

public interface INotificationRepository : IRepository<BankNotification>
{
    Task<IReadOnlyList<BankNotification>> GetByAccountIdAsync(string accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<BankNotification>> GetUnreadAsync(string accountId, CancellationToken cancellationToken = default);
}