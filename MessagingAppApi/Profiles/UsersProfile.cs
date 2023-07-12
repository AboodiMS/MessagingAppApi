using AutoMapper;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Model.Entities;

namespace MessagingAppApi.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<GetUserDto, User>().ReverseMap();
        }
    }
}
