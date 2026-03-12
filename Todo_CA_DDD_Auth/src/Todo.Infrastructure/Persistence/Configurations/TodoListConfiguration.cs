using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Domain.TodoLists;

namespace Todo.Infrastructure.Persistence.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Colour).HasMaxLength(50);

        builder.HasMany(t => t.Items)
               .WithOne()
               .HasForeignKey("TodoListId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.Items).HasField("_items");
    }
}
