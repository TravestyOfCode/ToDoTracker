using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoTask.Queries;

public class GetTaskById : IRequest<Result<ToDoTaskModel>>
{
    public int Id { get; set; }
}

public class GetTaskByIdHandler : IRequestHandler<GetTaskById, Result<ToDoTaskModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<GetTaskByIdHandler> _logger;

    public GetTaskByIdHandler(ApplicationDbContext dbContext, ILogger<GetTaskByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoTaskModel>> Handle(GetTaskById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoTasks.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }

            return entity.ToModel();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexepected error handling request: {request}", request);

            return new ServerError();
        }
    }
}
