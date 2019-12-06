
namespace Domain.Users.Posts
{
    public class CreatePostModel
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string PublisherEmail { get; set; }

        public string AttachmentUrl { get; set; }
    }
}
