using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoTask.Commands;

public class DeleteTask : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteTaskHandler : IRequestHandler<DeleteTask, Result>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<DeleteTaskHandler> _logger;

    public DeleteTaskHandler(ApplicationDbContext dbContext, ILogger<DeleteTaskHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteTask request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoTasks.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }

            _dbContext.ToDoTasks.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return new ServerError();
        }
    }
}
