using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Commands;

public class DeleteToDoList : IRequest<Result>
{
    public int Id { get; set; }
}

public class DeleteToDoListHandler : IRequestHandler<DeleteToDoList, Result>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<DeleteToDoListHandler> _logger;

    public DeleteToDoListHandler(ApplicationDbContext dbContext, ILogger<DeleteToDoListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoLists.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }

            _dbContext.ToDoLists.Remove(entity);

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
