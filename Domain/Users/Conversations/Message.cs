using System;

namespace SocialMedia.Domain.Users.Conversations
{
    public class Message
    {
        public string Text { get; set; }
        
        public long ConversationId { get; set; }

        public DateTime SentAt { get; set; }

        public bool WasRead { get; set; }
    }
}
