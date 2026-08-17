using System.ComponentModel.DataAnnotations;

namespace VSRSystemsBackend.Application.Bank.DTOs;

public class BankAccountDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string IfscCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBankAccountDto
{
    [Required]
    [MaxLength(30)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public decimal Balance { get; set; } = 0;

    [MaxLength(3)]
    public string Currency { get; set; } = "INR";

    [MaxLength(100)]
    public string Branch { get; set; } = string.Empty;

    [MaxLength(11)]
    public string IfscCode { get; set; } = string.Empty;
}

public class UpdateBankAccountDto
{
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    [MaxLength(3)]
    public string Currency { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Branch { get; set; } = string.Empty;

    [MaxLength(11)]
    public string IfscCode { get; set; } = string.Empty;
}

public class TransactionDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal BalanceAfter { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTransactionDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
}

public class UpdateTransactionDto
{
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class BeneficiaryDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string IfscCode { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBeneficiaryDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    public string IfscCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string BankName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Branch { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "IMPS";
}

public class UpdateBeneficiaryDto
{
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [MaxLength(30)]
    public string AccountNumber { get; set; } = string.Empty;

    [MaxLength(11)]
    public string IfscCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string BankName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Branch { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    public bool IsVerified { get; set; }
}

public class CardDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public string Cvv { get; set; } = string.Empty;
    public string CardHolderName { get; set; } = string.Empty;
    public decimal Limit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCardDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(19)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = "active";

    public DateTime ExpiryDate { get; set; }

    [MaxLength(4)]
    public string Cvv { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CardHolderName { get; set; } = string.Empty;

    public decimal Limit { get; set; }
}

public class UpdateCardDto
{
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    [MaxLength(4)]
    public string Cvv { get; set; } = string.Empty;

    [MaxLength(200)]
    public string CardHolderName { get; set; } = string.Empty;

    public decimal Limit { get; set; }
}

public class LoanDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal EmiAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateLoanDto
{
    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
}

public class UpdateLoanDto
{
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class DepositDto
{
    public string Id { get; set; } = string.Empty;
    public string CustomerId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public decimal MaturityAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime MaturityDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDepositDto
{
    [Required]
    [MaxLength(50)]
    public string CustomerId { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public DateTime StartDate { get; set; }
}

public class UpdateDepositDto
{
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public decimal InterestRate { get; set; }
    public int TenureMonths { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class BillDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string BillerName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AutoPay { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBillDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string BillerName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "pending";
    public bool AutoPay { get; set; }
}

public class UpdateBillDto
{
    [MaxLength(200)]
    public string BillerName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AutoPay { get; set; }
}

public class BankDocumentDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBankDocumentDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string FileUrl { get; set; } = string.Empty;

    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = "valid";
}

public class UpdateBankDocumentDto
{
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string FileUrl { get; set; } = string.Empty;

    public DateTime? ExpiryDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class BankNotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string AccountId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateBankNotificationDto
{
    [Required]
    [MaxLength(50)]
    public string AccountId { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Type { get; set; } = string.Empty;
}