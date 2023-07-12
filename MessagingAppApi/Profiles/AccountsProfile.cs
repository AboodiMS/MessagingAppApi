using AutoMapper;
using MessagingAppApi.Dtos.Accounts;
using MessagingAppApi.Dtos.Users;
using MessagingAppApi.Model.Entities;

namespace MessagingAppApi.Profiles
{
    public class AccountsProfile:Profile
    {
        public AccountsProfile() 
        {
            CreateMap<GetAccountDto,User>().ReverseMap();
            CreateMap<User, RegisterAccountDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username.Trim()))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.Trim())).ReverseMap();
        }
    }
}
