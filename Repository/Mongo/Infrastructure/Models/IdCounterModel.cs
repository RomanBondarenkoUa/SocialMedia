using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Mongo.Infrastructure.Models
{
    internal class IdCounterModel
    {
        [BsonId]
        public int _id;

        public long LatestPostId;
    }
}
