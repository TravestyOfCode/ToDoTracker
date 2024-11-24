using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoTask.Commands;

public class UpdateTask : IRequest<Result<ToDoTaskModel>>
{
    public int Id { get; set; }

    public int ToDoListId { get; set; }

    public int Ordinal { get; set; }

    public bool IsCompleted { get; set; }

    public required string Description { get; set; }

    public DateTime? DueBy { get; set; }
}

public class UpdateTaskHandler : IRequestHandler<UpdateTask, Result<ToDoTaskModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<UpdateTaskHandler> _logger;

    public UpdateTaskHandler(ApplicationDbContext dbContext, ILogger<UpdateTaskHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoTaskModel>> Handle(UpdateTask request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoTasks.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }

            entity.ToDoListId = request.ToDoListId;
            entity.Ordinal = request.Ordinal;
            entity.IsCompleted = request.IsCompleted;
            entity.Description = request.Description;
            entity.DueBy = request.DueBy;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return entity.ToModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return new ServerError();
        }
    }
}
