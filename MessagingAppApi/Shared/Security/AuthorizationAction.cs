using MessagingAppApi.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MessagingAppApi.Shared.Security
{
    [AttributeUsage(AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAction : Attribute, IAuthorizationFilter, IFilterMetadata
    {
        private List<Claim> Claims { get; set; } = new List<Claim>();
        private bool IsAuthorizedByRule { get; set; }

        public AuthorizationAction(bool isAuthorizedByRule = false)
        {
            IsAuthorizedByRule= isAuthorizedByRule;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            string userId = context.HttpContext.User.Claims.FirstOrDefault(a => a.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException();
            Guid myUserId;
            if(Guid.TryParse(userId, out myUserId) == false)
                throw new UnauthorizedException();

            Claims = context.HttpContext.User.Claims.ToList();
            if(validateRule(context.ActionDescriptor.AttributeRouteInfo.Template) == false && IsAuthorizedByRule)
                throw new UnauthorizedException(true);
        }
        private bool validateRule(string ruleName)
        {
             return Claims.Any(a => a.Type == ClaimTypes.Role && (a.Value == ruleName || a.Value == "admin"));
        }
    }
}
