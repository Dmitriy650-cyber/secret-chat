namespace SecretChat.Api.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, LoginUserDto>().ReverseMap();
			CreateMap<User, RegisterUserDto>().ReverseMap();
			CreateMap<User, LoggedInUserDto>().ReverseMap();
			CreateMap<User, UpdatableUserDto>().ReverseMap();
		}
	}
}
