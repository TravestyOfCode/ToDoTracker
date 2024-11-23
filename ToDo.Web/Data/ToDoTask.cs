using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDo.Web.Data;

public class ToDoTask
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public ToDoList? ToDoList { get; set; }

    public int Ordinal { get; set; }

    public bool IsCompleted { get; set; }

    public required string Description { get; set; }

    public DateTime? DueBy { get; set; }
}

public class ToDoTaskConfiguration : IEntityTypeConfiguration<ToDoTask>
{
    public void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder.ToTable(nameof(ToDoTask));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
            .HasMaxLength(1028)
            .IsRequired(true);

        builder.HasOne(p => p.ToDoList)
            .WithMany(p => p.ToDoTasks)
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.ToDoListId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}
