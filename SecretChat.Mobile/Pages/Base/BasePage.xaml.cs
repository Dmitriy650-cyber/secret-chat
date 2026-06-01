namespace SecretChat.Mobile.Pages.Base;

public partial class BasePage : ContentPage
{
	public IList<Microsoft.Maui.IView> PageContent => PageContentGrid.Children;

	public BasePage()
	{
		InitializeComponent();
	}
}