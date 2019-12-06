using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Repository.Mongo.DataModels
{
    public class Post
    {
        [BsonId]
        public long _id { get; set; }

        public int PublisherId { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AttachmentUrl { get; set; }

        public List<Reaction> Reactions { get; set; }
    }
}
