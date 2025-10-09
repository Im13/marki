using Core.Entities.Recommendation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class SessionInteractionConfiguration : IEntityTypeConfiguration<SessionInteraction>
    {
        public void Configure(EntityTypeBuilder<SessionInteraction> builder)
        {
            builder.ToTable("SessionInteractions");
            
            builder.HasKey(si => si.Id);
            
            builder.Property(si => si.SessionId)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(si => si.InteractionType)
                .IsRequired();
            
            builder.Property(si => si.InteractionDate)
                .IsRequired();
            
            // Indexes
            builder.HasIndex(si => new { si.SessionId, si.InteractionDate })
                .HasDatabaseName("IX_SessionInteractions_SessionId_Date");
            
            builder.HasIndex(si => new { si.ProductId, si.InteractionType })
                .HasDatabaseName("IX_SessionInteractions_ProductId_Type");
            
            // Relationships
            builder.HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(si => si.Session)
                .WithMany(s => s.Interactions)
                .HasForeignKey(si => si.SessionId)
                .HasPrincipalKey(s => s.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}