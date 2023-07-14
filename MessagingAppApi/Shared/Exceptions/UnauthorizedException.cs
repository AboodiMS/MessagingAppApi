namespace MessagingAppApi.Shared.Exceptions
{
    public class UnauthorizedException: Exception
    {
        private static readonly string UnauthorizedMessage = "Unauthorized";
        private static readonly string UnauthorizedByRuleMessage = "UnauthorizedByRule";   
        public UnauthorizedException(bool unauthorizedByRule = false) 
            : base(unauthorizedByRule? UnauthorizedByRuleMessage: UnauthorizedMessage)
        {
        }
    }
}
