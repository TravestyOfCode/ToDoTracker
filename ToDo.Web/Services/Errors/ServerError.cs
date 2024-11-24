namespace ToDo.Web.Services.Errors;

public class ServerError : HttpStatusError
{
    public ServerError() : base(500)
    {

    }
    public ServerError(string errorMessage) : base(500, errorMessage)
    {

    }
}
