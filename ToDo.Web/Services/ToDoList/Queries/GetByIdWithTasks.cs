using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Queries;

public class GetByIdWithTasks : IRequest<Result<ToDoListModel>>
{
    public int Id { get; set; }
}

public class GetByIdWithTasksHandler : IRequestHandler<GetByIdWithTasks, Result<ToDoListModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<GetByIdWithTasksHandler> _logger;

    public GetByIdWithTasksHandler(ApplicationDbContext dbContext, ILogger<GetByIdWithTasksHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoListModel>> Handle(GetByIdWithTasks request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoLists
                .Include(p => p.ToDoTasks)
                .SingleOrDefaultAsync(p => p.Id.Equals(request.Id));
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }
            return entity.ToModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);
            return new ServerError();
        }
    }
}
