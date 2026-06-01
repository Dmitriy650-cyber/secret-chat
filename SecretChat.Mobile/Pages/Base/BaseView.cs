namespace SecretChat.Mobile.Pages.Base
{
	public class BaseView<TViewModel>() : BasePage where TViewModel : BaseViewModel
	{
		protected bool _isPageLoaded = false;
		protected TViewModel ViewModel { get; set; } = null!;

		protected event EventHandler ViewModelInitializedAction = null!;

		protected override async void OnAppearing()
		{
			if (!_isPageLoaded)
			{
				base.OnAppearing();

				BindingContext = ViewModel = ServiceHelper.Services!.GetRequiredService<TViewModel>();

				await ViewModel.InitializeViewModel();

				ViewModelInitializedAction?.Invoke(this, EventArgs.Empty);

				_isPageLoaded = true;
			}
		}
	}
}
