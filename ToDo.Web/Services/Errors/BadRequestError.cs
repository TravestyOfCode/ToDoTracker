using System.Collections.Generic;

namespace ToDo.Web.Services.Errors;

public class BadRequestError : HttpStatusError
{
    public BadRequestError() : base(400)
    {

    }
    public BadRequestError(string propertyName, string errorMessage) : base(400)
    {
        Metadata.Add(propertyName, errorMessage);
    }
    public BadRequestError(IEnumerable<(string, string)> errors) : base(400)
    {
        foreach (var error in errors)
        {
            Metadata.Add(error.Item1, error.Item2);
        }
    }
}
