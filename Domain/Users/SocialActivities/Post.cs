using System;
using System.Collections.Generic;

namespace Domain.Users.SocialActivities
{
    public class Post
    {
        public Uri PostUrl { get; set; }

        public string Title { get; set; }

        public string OwnerName { get; set; }

        public Uri OwnerProfileLink { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public IEnumerable<Comment> Comments {get; set;}

        public string AttachmentUrl { get; set; }

        public int TotalPositiveRate { get; set; }

        public int TotalNegativeRate { get; set; }
    }
}
