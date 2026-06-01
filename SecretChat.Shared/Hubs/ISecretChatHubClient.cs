namespace SecretChat.Shared.Hubs
{
	public interface ISecretChatHubClient
	{
		Task ChatCreatedAsync(ChatDto dto);
		Task ChatDeletedAsync(int id);

		Task UserCreatedOrUpdatedAsync(LoggedInUserDto dto);
		Task UserDeletedAsync(int id);

		Task ContactAddedAsync(int id);
		Task ContactDeletedAsync(int id);

		Task MessageSentAsync(MessageDto dto);
		Task MessageDeletedAsync(int id);
	}
}
