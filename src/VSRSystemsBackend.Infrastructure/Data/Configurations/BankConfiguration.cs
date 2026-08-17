using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.Bank;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
{
    public void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.ToTable("bank_accounts");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.AccountNumber).HasMaxLength(30).IsRequired();
        builder.Property(b => b.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.Type).HasMaxLength(20).IsRequired();
        builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(b => b.Balance).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(b => b.Currency).HasMaxLength(3).HasDefaultValue("INR");
        builder.Property(b => b.Branch).HasMaxLength(100);
        builder.Property(b => b.IfscCode).HasMaxLength(11);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.AccountNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.Status);
    }
}

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(t => t.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Type).HasMaxLength(20).IsRequired();
        builder.Property(t => t.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(t => t.BalanceAfter).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.Reference).HasMaxLength(50);
        builder.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("completed");
        builder.Property(t => t.Date).HasDefaultValueSql("NOW()");
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(t => t.UpdatedAt);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(t => t.AccountId);
        builder.HasIndex(t => t.Date);
        builder.HasIndex(t => t.Type);
        builder.HasIndex(t => t.Status);
    }
}

public class BeneficiaryConfiguration : IEntityTypeConfiguration<Beneficiary>
{
    public void Configure(EntityTypeBuilder<Beneficiary> builder)
    {
        builder.ToTable("beneficiaries");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.Name).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Nickname).HasMaxLength(50).IsRequired();
        builder.Property(b => b.AccountNumber).HasMaxLength(30).IsRequired();
        builder.Property(b => b.IfscCode).HasMaxLength(11).IsRequired();
        builder.Property(b => b.BankName).HasMaxLength(100).IsRequired();
        builder.Property(b => b.Branch).HasMaxLength(100);
        builder.Property(b => b.Type).HasMaxLength(20).HasDefaultValue("IMPS");
        builder.Property(b => b.IsVerified).HasDefaultValue(false);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.AccountId);
        builder.HasIndex(b => new { b.AccountId, b.Nickname }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CardNumber).HasMaxLength(19).IsRequired();
        builder.Property(c => c.Type).HasMaxLength(20).IsRequired();
        builder.Property(c => c.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(c => c.ExpiryDate).IsRequired();
        builder.Property(c => c.Cvv).HasMaxLength(4);
        builder.Property(c => c.CardHolderName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Limit).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.CardNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.AccountId);
        builder.HasIndex(c => c.Status);
    }
}

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("loans");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(l => l.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(l => l.Type).HasMaxLength(30).IsRequired();
        builder.Property(l => l.PrincipalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(l => l.InterestRate).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(l => l.TenureMonths).IsRequired();
        builder.Property(l => l.EmiAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(l => l.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(l => l.StartDate);
        builder.Property(l => l.EndDate);
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt);
        builder.Property(l => l.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(l => l.CustomerId);
        builder.HasIndex(l => l.Status);
    }
}

public class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder.ToTable("deposits");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Type).HasMaxLength(30).IsRequired();
        builder.Property(d => d.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(d => d.InterestRate).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(d => d.TenureMonths).IsRequired();
        builder.Property(d => d.MaturityAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(d => d.StartDate).IsRequired();
        builder.Property(d => d.MaturityDate).IsRequired();
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.CustomerId);
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.MaturityDate);
    }
}

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("bills");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(b => b.BillerName).HasMaxLength(200).IsRequired();
        builder.Property(b => b.Category).HasMaxLength(50).IsRequired();
        builder.Property(b => b.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(b => b.DueDate).IsRequired();
        builder.Property(b => b.Status).HasMaxLength(20).HasDefaultValue("pending");
        builder.Property(b => b.AutoPay).HasDefaultValue(false);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.AccountId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.DueDate);
    }
}

public class BankDocumentConfiguration : IEntityTypeConfiguration<BankDocument>
{
    public void Configure(EntityTypeBuilder<BankDocument> builder)
    {
        builder.ToTable("bank_documents");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Type).HasMaxLength(50).IsRequired();
        builder.Property(d => d.FileName).HasMaxLength(255).IsRequired();
        builder.Property(d => d.FileUrl).HasMaxLength(500).IsRequired();
        builder.Property(d => d.ExpiryDate);
        builder.Property(d => d.Status).HasMaxLength(20).HasDefaultValue("valid");
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.AccountId);
        builder.HasIndex(d => d.Type);
    }
}

public class BankNotificationConfiguration : IEntityTypeConfiguration<BankNotification>
{
    public void Configure(EntityTypeBuilder<BankNotification> builder)
    {
        builder.ToTable("bank_notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(n => n.AccountId).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Message).HasMaxLength(1000).IsRequired();
        builder.Property(n => n.Type).HasMaxLength(30).IsRequired();
        builder.Property(n => n.IsRead).HasDefaultValue(false);
        builder.Property(n => n.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(n => n.UpdatedAt);
        builder.Property(n => n.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(n => n.AccountId);
        builder.HasIndex(n => n.IsRead);
    }
}