using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Repository.Mongo.DataModels
{
    public class User
    {
        [BsonId]
        public int _id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string AboutMe { get; set; }

        public BsonDateTime RegistredAt { get; set; }

        public string PasswordHash { get; set; }

        public List<int> SubscribedOn { get; set; }
    }
}
