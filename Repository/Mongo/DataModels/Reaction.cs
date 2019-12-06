using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Mongo.DataModels
{
    public class Reaction
    {
        public ObjectId UserId { get; set; }

        public string Text { get; set; }

        public bool? IsPositive { get; set; }
    }
}
