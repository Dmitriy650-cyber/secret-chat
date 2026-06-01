namespace SecretChat.Mobile.Models
{
	public class CollectionViewItemMessage
	{
		public MessageDto Item { get; set; } = null!;
		public int ItemIndex { get; set; }
		public int ItemCount { get; set; }
		public bool IsCurrentUser { get; set; }

		public bool IsLastItem => ItemIndex != 0 && ItemIndex == ItemCount - 1;
	}
}
