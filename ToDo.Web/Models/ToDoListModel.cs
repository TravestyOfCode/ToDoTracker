using System.Collections.Generic;
using System.Linq;

namespace ToDo.Web.Models;

public class ToDoListModel
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public required string Title { get; set; }

    public int Ordinal { get; set; }

    public string? BackgroundColor { get; set; }

    public string? ForegroundColor { get; set; }

    public ICollection<ToDoTaskModel>? ToDoTasks { get; set; }
}

public static class ToDoListModelExtensions
{
    public static ToDoListModel ToModel(this Data.ToDoList entity)
    {
        return new ToDoListModel()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Title = entity.Title,
            Ordinal = entity.Ordinal,
            BackgroundColor = entity.BackgroundColor,
            ForegroundColor = entity.ForegroundColor,
            ToDoTasks = entity.ToDoTasks?.Select(p => p.ToModel()).ToList()
        };
    }
}
