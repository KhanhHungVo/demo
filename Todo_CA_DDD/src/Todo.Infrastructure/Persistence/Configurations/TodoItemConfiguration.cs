using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.Entities;

namespace Todo.Infrastructure.Persistence.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Notes)
            .HasMaxLength(1000);

        builder.Property(i => i.IsDone).IsRequired();
        builder.Property(i => i.CreatedAt).IsRequired();
        builder.Property(i => i.CompletedAt);

        builder.OwnsOne(i => i.Priority, p =>
            p.Property(x => x.Value).HasColumnName("Priority").IsRequired());

        builder.OwnsOne(i => i.DueDate, d =>
            d.Property(x => x.Value).HasColumnName("DueDate"));
    }
}
