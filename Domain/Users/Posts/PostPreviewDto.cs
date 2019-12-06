using System;

namespace Domain.Users.Posts
{
    public class PostPreviewDto
    {
        public string publisherName { get; set; }

        public Uri publisheProfileLink { get; set; }

        public string Title { get; set; }

        public string PreviewText { get; set; }

        public Uri FullPostUrl { get; set; }

        public int TotalRate { get; set; }
    }
}
