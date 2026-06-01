namespace SecretChat.Api.Profiles
{
	public class PhotoProfile : Profile
	{
		public PhotoProfile()
		{
			CreateMap<Photo, PhotoDto>().ReverseMap();
		}
	}
}
