using AutoMapper;
using GroupChat.Models;
using GroupChat.Models.Dto;

namespace GroupChat.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<MessageReturnDto, Message>();
            CreateMap<Message, MessageReturnDto>();
        }
    }
}
