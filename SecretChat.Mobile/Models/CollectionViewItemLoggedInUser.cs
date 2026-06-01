namespace SecretChat.Mobile.Models
{
	public class CollectionViewItemLoggedInUser
	{
		public LoggedInUserDto Item { get; set; } = null!;
		public int ItemIndex { get; set; }
		public int ItemCount { get; set; }

		public bool IsLastItem => ItemIndex != 0 && ItemIndex == ItemCount - 1;
	}
}
