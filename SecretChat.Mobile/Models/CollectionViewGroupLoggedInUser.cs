namespace SecretChat.Mobile.Models
{
	public class CollectionViewGroupLoggedInUser : List<LoggedInUserDto>
	{
		public string? GroupTitle { get; set; }
		public int ItemIndex { get; set; }
		public int ItemCount { get; set; }

		public bool IsLastItem => ItemIndex != 0 && ItemIndex == ItemCount - 1;

		public CollectionViewGroupLoggedInUser(string groupTitle, int index, int count, List<LoggedInUserDto> items) : base(items)
		{
			GroupTitle = groupTitle;
			ItemIndex = index;
			ItemCount = count;
		}
	}
}
