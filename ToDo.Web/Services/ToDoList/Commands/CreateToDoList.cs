using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Commands;

public class CreateToDoList : IRequest<Result<ToDoListModel>>
{
    public required string UserId { get; set; }

    public required string Title { get; set; }

    public int Ordinal { get; set; }

    public string? BackgroundColor { get; set; }

    public string? ForegroundColor { get; set; }
}

public class CreateToDoListHandler : IRequestHandler<CreateToDoList, Result<ToDoListModel>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<CreateToDoListHandler> _logger;

    public CreateToDoListHandler(ApplicationDbContext dbContext, ILogger<CreateToDoListHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ToDoListModel>> Handle(CreateToDoList request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = _dbContext.ToDoLists.Add(new Data.ToDoList()
            {
                UserId = request.UserId,
                Title = request.Title,
                Ordinal = request.Ordinal,
                BackgroundColor = request.BackgroundColor,
                ForegroundColor = request.ForegroundColor
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
