using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Queries;

public class GetById : IRequest<Result<ToDoListModel>>
{
    public int Id { get; set; }
}

public class GetByIdHandler : IRequestHandler<GetById, Result<ToDoListModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<GetByIdHandler> _logger;

    public GetByIdHandler(ApplicationDbContext dbContext, ILogger<GetByIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoListModel>> Handle(GetById request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoLists.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
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
