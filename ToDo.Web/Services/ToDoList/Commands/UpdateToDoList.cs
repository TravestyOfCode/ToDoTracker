using Microsoft.EntityFrameworkCore;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Commands;

public class UpdateToDoList : IRequest<Result<ToDoListModel>>
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public required string Title { get; set; }

    public int Ordinal { get; set; }

    public string? BackgroundColor { get; set; }

    public string? ForegroundColor { get; set; }
}

public class UpdateToDoListHandler : IRequestHandler<UpdateToDoList, Result<ToDoListModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<UpdateToDoListHandler> _logger;

    public UpdateToDoListHandler(ApplicationDbContext dbContext, ILogger<UpdateToDoListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoListModel>> Handle(UpdateToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _dbContext.ToDoLists.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (entity == null)
            {
                return new NotFoundError(nameof(request.Id), request.Id);
            }

            entity.UserId = request.UserId;
            entity.Title = request.Title;
            entity.Ordinal = request.Ordinal;
            entity.BackgroundColor = request.BackgroundColor;
            entity.ForegroundColor = request.ForegroundColor;

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

