using AutoMapper;
using MessagingAppApi.Dtos.Accounts;
using MessagingAppApi.Dtos.Rooms;
using MessagingAppApi.Model.Entities;

namespace MessagingAppApi.Profiles
{
    public class RoomsProfile:Profile
    {
        public RoomsProfile() 
        {
            CreateMap<GetRoomDto, Room>().ReverseMap();
            CreateMap<Room, CreateRoomDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ReverseMap();
            CreateMap<Room, UpdateRoomDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()))
            .ReverseMap();
        }
    }
}
