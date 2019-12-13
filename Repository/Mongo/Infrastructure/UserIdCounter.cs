using Domain.Users.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Mongo.Infrastructure.Interfaces;
using Repository.Mongo.Infrastructure.Models;

namespace Repository.Mongo.Infrastructure
{
    public class UserIdCounter : IUserIdCounter
    {
        private readonly IMongoCollection<IdCounterModel> counter;

        public UserIdCounter(IDbParams dbParams)
        {
            IMongoDatabase database = new MongoClient(dbParams.ConnectionString).GetDatabase("SocialMedia");
            this.counter = database.GetCollection<IdCounterModel>("userIdCouner");
        }

        public int GetNewUserId()
        {
            var currentCounter = this.counter.Find(new BsonDocument()).FirstOrDefault();

            var nextCounter = currentCounter == null ?
                    new IdCounterModel { LatestPostId = 1 }
                    : new IdCounterModel() { LatestPostId = currentCounter.LatestPostId + 1, _id = currentCounter._id };

            counter.ReplaceOne(
                filter: new BsonDocument(),
                options: new UpdateOptions { IsUpsert = true },
                replacement: nextCounter);

            return checked((int)nextCounter.LatestPostId);
        }
    }
}
