namespace SecretChat.Mobile.ViewModels
{
	public partial class ProfileViewModel(IConnectivity connectivity, AuthService authService, IPhotoApi photoApi) : BaseViewModel(connectivity, authService)
	{
		[ObservableProperty]
		private LoggedInUserDto _user = authService.User!;

		public ObservableCollection<PhotoDto> Photos { get; set; } = [];

		public override async Task InitializeViewModel()
		{
			await MakeFirstApiRequestAsync(async () =>
			{
				var response = await photoApi.GetUserPhotosAsync();

				if (!response.IsSuccess)
				{
					await ShowErrorAlertAsync(response.Message);
					return;
				}

				Photos.AddRange(response.Data);

				if (Photos.Count == 0)
				{
					Photos.Add(new PhotoDto(0, "user.png", true));
				}
			});
		}

		[RelayCommand]
		private async Task GoToEditPageAsync()
		{
			await NavigateAsync(nameof(EditPage));
		}
	}
}
