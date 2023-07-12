using MessagingAppApi.Shared.Filters;

namespace MessagingAppApi.Filters
{
    public class UsersFilter:BaseFilter
    {
        public string? Username { get; set; }
    }
}
