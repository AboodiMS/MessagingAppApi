using AutoMapper;
using MessagingAppApi.Dtos.Accounts;
using MessagingAppApi.Dtos.Messages;
using MessagingAppApi.Model.Entities;

namespace MessagingAppApi.Profiles
{
    public class MessagesProfile : Profile
    {
        public MessagesProfile()
        {
            CreateMap<GetMessageDto, Message>().ReverseMap();
            CreateMap<Message, CreateMessageDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text.Trim())).ReverseMap();
            CreateMap<Message, UpdateMessageDto>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text.Trim())).ReverseMap();
        }
    }
}
