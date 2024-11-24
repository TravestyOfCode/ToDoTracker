namespace ToDo.Web.Models;

public class ToDoTaskModel
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public int Ordinal { get; set; }

    public bool IsCompleted { get; set; }

    public required string Description { get; set; }

    public DateTime? DueBy { get; set; }
}

public static class ToDoTaskModelExtensions
{
    public static ToDoTaskModel ToModel(this Data.ToDoTask entity)
    {
        return new ToDoTaskModel()
        {
            Id = entity.Id,
            ToDoListId = entity.ToDoListId,
            Ordinal = entity.Ordinal,
            IsCompleted = entity.IsCompleted,
            Description = entity.Description,
            DueBy = entity.DueBy
        };
    }
}