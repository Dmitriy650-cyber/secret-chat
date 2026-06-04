namespace SecretChat.Mobile.Models
{
    public class CollectionViewItemChat 
    {
        public ChatDto Item { get; set; } = null!;
        public int ItemIndex { get; set; }
        public int ItemCount { get; set; }

		public bool IsLastItem => ItemIndex != 0 && ItemIndex == ItemCount - 1;
        public string LastMessageTime => Item.LastMessage is { }
            ? Item.LastMessage.SendOn.ToString("dd.MM.yyyy HH:mm")
            : string.Empty;
	}
}
