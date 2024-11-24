using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoTask.Queries;

public class GetTasksByToDoListId : IRequest<Result<ICollection<ToDoTaskModel>>>
{
    public int ToDoListId { get; set; }
}

public class GetTasksByToDoListIdHandler : IRequestHandler<GetTasksByToDoListId, Result<ICollection<ToDoTaskModel>>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<GetTasksByToDoListIdHandler> _logger;

    public GetTasksByToDoListIdHandler(ApplicationDbContext dbContext, ILogger<GetTasksByToDoListIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ICollection<ToDoTaskModel>>> Handle(GetTasksByToDoListId request, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.ToDoTasks
                .Where(p => p.ToDoListId.Equals(request.ToDoListId))
                .Select(p => p.ToModel())
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error handling request: {request}", request);

            return new ServerError();
        }
    }
}
