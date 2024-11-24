using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ToDo.Web.Data;
using ToDo.Web.Models;
using ToDo.Web.Services.Errors;

namespace ToDo.Web.Services.ToDoList.Queries;

public class GetAllByUserId : IRequest<Result<ICollection<ToDoListModel>>>
{
    public required string UserId { get; set; }
}

public class GetAllByUserIdHandler : IRequestHandler<GetAllByUserId, Result<ICollection<ToDoListModel>>>
{
    private readonly ApplicationDbContext _dbContext;

    private readonly ILogger<GetAllByUserIdHandler> _logger;

    public GetAllByUserIdHandler(ApplicationDbContext dbContext, ILogger<GetAllByUserIdHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<ICollection<ToDoListModel>>> Handle(GetAllByUserId request, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.ToDoLists
                .Where(p => p.UserId.Equals(request.UserId))
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
