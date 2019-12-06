namespace SocialMedia.Domain.Users.Conversations
{
    public class Conversation
    {
        public long ConversationId { get; set; }

        public int FirstSpeakerId { get; set; }

        public int SecondSpeakerId { get; set; }
    }
}
