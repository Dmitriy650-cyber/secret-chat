namespace SecretChat.Mobile.Models.Messages
{
    public class FromContactToDetailsMessage
    {
        public LoggedInUserDto User = null!;
        public bool IsUserContact;
        public bool IsHaveChat;
        public bool IsFromChat = false;
    }
}
