using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net;

namespace MessagingAppApi.Shared.Exceptions
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DbUpdateConcurrencyException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), "data may have been modified or deleted since entities were loaded.")), HttpStatusCode.BadRequest),
                BaseException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.BadRequest),
                UnauthorizedAccessException ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message)), HttpStatusCode.Unauthorized),
                _ => new ExceptionResponse(new ErrorsResponse(new Error("error", "There was an error.")), HttpStatusCode.InternalServerError),
            };

        private record Error(string Code, string Message);

        private record ErrorsResponse(params Error[] Errors);

        private static string GetErrorCode(object exception)
        {
            var type = exception.GetType();
            return Codes.GetOrAdd(type, type.Name.Replace("_exception", string.Empty));
        }
    }
}
