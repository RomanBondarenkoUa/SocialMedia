using System;

namespace Repository.Dapper.DataModels
{
    public class Post
    {
        public long Id { get; set; }

        public string PublisherEmail { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string AttachmentUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
