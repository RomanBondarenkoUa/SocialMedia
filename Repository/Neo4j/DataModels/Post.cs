using Domain.Users.SocialActivities;
using System;
using System.Collections.Generic;

namespace Repository.Neo4j.DataModels
{
    public class Post
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AttachmentUrl { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
