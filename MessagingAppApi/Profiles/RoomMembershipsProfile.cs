using AutoMapper;
using MessagingAppApi.Dtos.Accounts;
using MessagingAppApi.Dtos.RoomMemberships;
using MessagingAppApi.Model.Entities;

namespace MessagingAppApi.Profiles
{
    public class RoomMembershipsProfile:Profile
    {
        public RoomMembershipsProfile() 
        {
            CreateMap<GetRoomMembershipDto, RoomMembership> ()
                .ForPath(dest => dest.User.Username, opt => opt.MapFrom(src => src.Username)).ReverseMap();

            CreateMap<GetUserInvitationsDto, RoomMembership>()
            .ForPath(dest => dest.Room.Name, opt => opt.MapFrom(src => src.RoomName)).ReverseMap();

            CreateMap<RoomMembership, InvitationRoomMembershipDto>().ReverseMap();
        }
    }
}
