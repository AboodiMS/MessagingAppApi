using MessagingAppApi.Model.Entities;
using MessagingAppApi.Shared.Exceptions;

namespace MessagingAppApi.Profiles
{
    public static class Extensions
    {
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
                        => services
                          .AddAutoMapper(typeof(AccountsProfile))
                          .AddAutoMapper(typeof(UsersProfile))
                          .AddAutoMapper(typeof(RoomsProfile))
                          .AddAutoMapper(typeof(RoomMembershipsProfile))
                          .AddAutoMapper(typeof(MessagesProfile));
    }
}
