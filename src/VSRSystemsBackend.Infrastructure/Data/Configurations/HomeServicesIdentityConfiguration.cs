using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VSRSystemsBackend.Domain.HomeServices;

namespace VSRSystemsBackend.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(u => u.Email).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Phone).HasMaxLength(20);
        builder.Property(u => u.PasswordHash).HasMaxLength(500);
        builder.Property(u => u.FullName).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Status).HasMaxLength(20).HasDefaultValue("active");
        builder.Property(u => u.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(u => u.UpdatedAt);
        builder.Property(u => u.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(u => u.Email).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(u => u.Phone).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(u => u.Status);

        builder.HasMany(u => u.UserRoles)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(r => r.Name).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(r => r.UpdatedAt);
        builder.Property(r => r.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(r => r.Name).IsUnique().HasFilter("\"IsDeleted\" = false");

        builder.HasMany(r => r.UserRoles)
            .WithOne(ur => ur.Role)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(ur => ur.Id);
        builder.Property(ur => ur.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(ur => ur.UserId).HasMaxLength(50).IsRequired();
        builder.Property(ur => ur.RoleId).HasMaxLength(50).IsRequired();
        builder.Property(ur => ur.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(ur => ur.UpdatedAt);
        builder.Property(ur => ur.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Code).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Area).HasMaxLength(50);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Code).IsUnique().HasFilter("\"IsDeleted\" = false");

        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");
        builder.HasKey(rp => rp.Id);
        builder.Property(rp => rp.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(rp => rp.RoleId).HasMaxLength(50).IsRequired();
        builder.Property(rp => rp.PermissionId).HasMaxLength(50).IsRequired();
        builder.Property(rp => rp.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(rp => rp.UpdatedAt);
        builder.Property(rp => rp.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique().HasFilter("\"IsDeleted\" = false");
    }
}

public class HomeServiceCustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("home_service_customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.UserId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.DisplayName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.DefaultAddressId).HasMaxLength(50);
        builder.Property(c => c.WalletBalance).HasColumnType("decimal(18,2)").HasDefaultValue(0);
        builder.Property(c => c.MembershipPlanId).HasMaxLength(50);
        builder.Property(c => c.ReferralCode).HasMaxLength(50);
        builder.Property(c => c.ReferredByCustomerId).HasMaxLength(50);
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => c.UserId).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.ReferralCode).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.Phone).HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(c => c.Email).HasFilter("\"IsDeleted\" = false");

        builder.HasMany(c => c.Addresses)
            .WithOne(a => a.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("customer_addresses");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Label).HasMaxLength(50).HasDefaultValue("home");
        builder.Property(a => a.Line1).HasMaxLength(200);
        builder.Property(a => a.Line2).HasMaxLength(200);
        builder.Property(a => a.CityId).HasMaxLength(50);
        builder.Property(a => a.ZoneId).HasMaxLength(50);
        builder.Property(a => a.LocalityId).HasMaxLength(50);
        builder.Property(a => a.Pincode).HasMaxLength(20);
        builder.Property(a => a.Lat).HasColumnType("double precision");
        builder.Property(a => a.Lng).HasColumnType("double precision");
        builder.Property(a => a.ContactPerson).HasMaxLength(100);
        builder.Property(a => a.ContactPhone).HasMaxLength(20);
        builder.Property(a => a.AccessInstructions).HasMaxLength(500);
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.CustomerId);
        builder.HasIndex(a => a.Pincode);
    }
}

public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> builder)
    {
        builder.ToTable("support_tickets");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(t => t.TicketNumber).HasMaxLength(50).IsRequired();
        builder.Property(t => t.RaisedBy).HasMaxLength(50).IsRequired();
        builder.Property(t => t.Role).HasMaxLength(20).HasDefaultValue("customer");
        builder.Property(t => t.BookingId).HasMaxLength(50);
        builder.Property(t => t.Category).HasMaxLength(50);
        builder.Property(t => t.Subject).HasMaxLength(300);
        builder.Property(t => t.Description).HasMaxLength(3000);
        builder.Property(t => t.Status).HasMaxLength(20).HasDefaultValue("open");
        builder.Property(t => t.Priority).HasMaxLength(10).HasDefaultValue("medium");
        builder.Property(t => t.AssignedTo).HasMaxLength(50);
        builder.Property(t => t.Resolution).HasMaxLength(1000);
        builder.Property(t => t.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(t => t.UpdatedAt);
        builder.Property(t => t.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(t => t.TicketNumber).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(t => t.RaisedBy);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.BookingId);
    }
}

public class DisputeConfiguration : IEntityTypeConfiguration<Dispute>
{
    public void Configure(EntityTypeBuilder<Dispute> builder)
    {
        builder.ToTable("disputes");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(d => d.TicketId).HasMaxLength(50);
        builder.Property(d => d.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(d => d.RaisedBy).HasMaxLength(50).IsRequired();
        builder.Property(d => d.Reason).HasMaxLength(50);
        builder.Property(d => d.Details).HasMaxLength(2000);
        builder.Property(d => d.Status).HasMaxLength(30).HasDefaultValue("open");
        builder.Property(d => d.Resolution).HasMaxLength(1000);
        builder.Property(d => d.ResolvedBy).HasMaxLength(50);
        builder.Property(d => d.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(d => d.UpdatedAt);
        builder.Property(d => d.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(d => d.BookingId);
        builder.HasIndex(d => d.Status);
        builder.HasIndex(d => d.RaisedBy);
    }
}

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(c => c.BookingId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.ProfessionalId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(c => new { c.BookingId, c.CustomerId, c.ProfessionalId }).IsUnique().HasFilter("\"IsDeleted\" = false");

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ConversationMessageConfiguration : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.ToTable("conversation_messages");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(m => m.ConversationId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.SenderId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.Body).HasMaxLength(2000);
        builder.Property(m => m.ImageUrl).HasMaxLength(1000);
        builder.Property(m => m.SentAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.ReadAt);
        builder.Property(m => m.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(m => m.UpdatedAt);
        builder.Property(m => m.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(m => m.ConversationId);
        builder.HasIndex(m => m.SenderId);
        builder.HasIndex(m => m.SentAt);
    }
}

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(n => n.UserId).HasMaxLength(50).IsRequired();
        builder.Property(n => n.Channel).HasMaxLength(10).HasDefaultValue("in_app");
        builder.Property(n => n.Template).HasMaxLength(100);
        builder.Property(n => n.PayloadJson).HasMaxLength(3000).HasDefaultValue("{}");
        builder.Property(n => n.SentAt).HasDefaultValueSql("NOW()");
        builder.Property(n => n.ReadAt);
        builder.Property(n => n.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(n => n.UpdatedAt);
        builder.Property(n => n.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(n => n.UserId);
        builder.HasIndex(n => n.SentAt);
        builder.HasIndex(n => n.ReadAt);
    }
}

public class CmsPageConfiguration : IEntityTypeConfiguration<CmsPage>
{
    public void Configure(EntityTypeBuilder<CmsPage> builder)
    {
        builder.ToTable("cms_pages");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(p => p.Slug).HasMaxLength(220).IsRequired();
        builder.Property(p => p.Title).HasMaxLength(300).IsRequired();
        builder.Property(p => p.Body).HasMaxLength(10000);
        builder.Property(p => p.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(p => p.UpdatedAt);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(p => p.Slug).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(p => p.IsPublished);
    }
}

public class BannerConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable("banners");
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(b => b.Title).HasMaxLength(200);
        builder.Property(b => b.ImageUrl).HasMaxLength(1000);
        builder.Property(b => b.LinkUrl).HasMaxLength(1000);
        builder.Property(b => b.SortOrder).HasDefaultValue(0);
        builder.Property(b => b.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(b => b.UpdatedAt);
        builder.Property(b => b.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(b => b.IsActive);
        builder.HasIndex(b => b.SortOrder);
    }
}

public class FaqConfiguration : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        builder.ToTable("faqs");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(f => f.Category).HasMaxLength(100);
        builder.Property(f => f.Question).HasMaxLength(500);
        builder.Property(f => f.Answer).HasMaxLength(2000);
        builder.Property(f => f.SortOrder).HasDefaultValue(0);
        builder.Property(f => f.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(f => f.UpdatedAt);
        builder.Property(f => f.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(f => f.Category);
    }
}

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasMaxLength(50).ValueGeneratedNever();
        builder.Property(a => a.ActorId).HasMaxLength(50);
        builder.Property(a => a.Action).HasMaxLength(200).IsRequired();
        builder.Property(a => a.EntityType).HasMaxLength(100);
        builder.Property(a => a.EntityId).HasMaxLength(50);
        builder.Property(a => a.BeforeJson).HasMaxLength(5000).HasDefaultValue("{}");
        builder.Property(a => a.AfterJson).HasMaxLength(5000).HasDefaultValue("{}");
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(a => a.UpdatedAt);
        builder.Property(a => a.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(a => a.EntityType);
        builder.HasIndex(a => a.EntityId);
        builder.HasIndex(a => a.Action);
        builder.HasIndex(a => a.CreatedAt);
    }
}
