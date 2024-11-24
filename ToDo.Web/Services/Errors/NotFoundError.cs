namespace ToDo.Web.Services.Errors;

public class NotFoundError : HttpStatusError
{
    public NotFoundError() : base(404)
    {

    }
    public NotFoundError(string propertyName, object value) : base(404, $"The value {value} for {propertyName} was not found.")
    {

    }
    public NotFoundError(string errorMessage) : base(404, errorMessage)
    {

    }
}
