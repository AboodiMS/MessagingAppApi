using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MessagingAppApi.Shared.Exceptions
{

    public class BaseException : Exception
    {

        public BaseException(string message) : base(message)
        {
        }

    }
}
