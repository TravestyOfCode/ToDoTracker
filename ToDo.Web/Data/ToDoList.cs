using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace ToDo.Web.Data;

public class ToDoList
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public required string Title { get; set; }

    public int Ordinal { get; set; }

    public string? BackgroundColor { get; set; }

    public string? ForegroundColor { get; set; }

    public ICollection<ToDoTask>? ToDoTasks { get; set; }
}

public class ToDoListConfiguration : IEntityTypeConfiguration<ToDoList>
{
    public void Configure(EntityTypeBuilder<ToDoList> builder)
    {
        builder.ToTable(nameof(ToDoList));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasMaxLength(64)
            .IsRequired(true);

        builder.Property(p => p.BackgroundColor)
            .HasMaxLength(8)
            .IsRequired(false);

        builder.Property(p => p.ForegroundColor)
            .HasMaxLength(8)
            .IsRequired(false);

        builder.HasOne(p => p.User)
            .WithMany()
            .HasPrincipalKey(p => p.Id)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(true);
    }
}
