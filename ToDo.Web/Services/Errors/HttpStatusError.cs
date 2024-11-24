using FluentResults;

namespace ToDo.Web.Services.Errors;

public class HttpStatusError : Error
{
    public int StatusCode { get; set; }

    public HttpStatusError(int statusCode) : this(statusCode, string.Empty)
    {
    }

    public HttpStatusError(int statusCode, string errorMessage) : base(errorMessage)
    {
        StatusCode = statusCode;
    }
}
