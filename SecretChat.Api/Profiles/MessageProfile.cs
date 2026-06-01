namespace SecretChat.Api.Profiles
{
	public class MessageProfile : Profile
	{
		public MessageProfile()
		{
			CreateMap<Message, CreatableUpdatableMessageDto>().ReverseMap();
			CreateMap<Message, MessageDto>().ReverseMap();
		}
	}
}
