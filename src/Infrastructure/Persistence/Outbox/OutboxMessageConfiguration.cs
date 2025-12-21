
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Outbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> b)
    {
        b.ToTable("outbox_messages");

        b.HasKey(x => x.Id);

        b.Property(x => x.Type).HasMaxLength(300).IsRequired();
        b.Property(x => x.PayloadJson).IsRequired();

        b.HasIndex(x => x.PublishedAtUtc);
        b.HasIndex(x => x.NextAttemptAtUtc);
        b.HasIndex(x => new { x.PublishedAtUtc, x.NextAttemptAtUtc, x.OccurredAtUtc });
    }
}
