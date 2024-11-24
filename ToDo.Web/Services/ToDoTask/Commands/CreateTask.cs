using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoTask.Commands;

public class CreateTask : IRequest<Result<ToDoTaskModel>>
{
    public int ToDoListId { get; set; }

    public int Ordinal { get; set; }

    public bool IsCompleted { get; set; }

    public required string Description { get; set; }

    public DateTime? DueBy { get; set; }
}

public class CreateTaskHandler : IRequestHandler<CreateTask, Result<ToDoTaskModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CreateTaskHandler> _logger;

    public CreateTaskHandler(ApplicationDbContext dbContext, ILogger<CreateTaskHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoTaskModel>> Handle(CreateTask request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.ToDoTasks.Add(new Data.ToDoTask()
            {
                ToDoListId = request.ToDoListId,
                Ordinal = request.Ordinal,
                IsCompleted = request.IsCompleted,
                Description = request.Description,
                DueBy = request.DueBy
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.Entity.ToModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return new ServerError();
        }
    }
}
