using Core.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
    {
        public void Configure(EntityTypeBuilder<UserSession> builder)
        {
            builder.ToTable("UserSessions");

            builder.HasKey(us => us.Id);

            builder.HasIndex(us => us.SessionId)
                .IsUnique();

            builder.Property(us => us.SessionId)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(us => us.CreatedAt)
                .IsRequired();

            builder.Property(us => us.LastActivityAt)
                .IsRequired();
        }
    }
}

