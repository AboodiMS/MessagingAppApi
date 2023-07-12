using System.Net;

namespace MessagingAppApi.Shared.Exceptions
{
    public sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);
}
