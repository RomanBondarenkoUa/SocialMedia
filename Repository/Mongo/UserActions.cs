using Domain.Users.Interfaces;
using Domain.Users.Posts;
using Global.Environment;
using Global.Environment.Interfaces;
using MongoDB.Driver;
using Repository.Mongo.Infrastructure.Interfaces;
using Repository.Mongo.Mappers.Interfaces;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Mongo
{
    public class UserActions : IUserActions
    {
        private IDateTimeProvider dateTimeProvider;

        private IMongoCollection<Mongo.DataModels.User> users;
        private IUserMapper userMapper;
        private IUserIdCounter userIdCounter;

        public UserActions(IDbParams dbParams, IDateTimeProvider dateTimeProvider, IDbParams mongoDbParams, IUserMapper userMapper, IUserIdCounter userIdCounter)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.userMapper = userMapper;
            this.userIdCounter = userIdCounter;

            IMongoDatabase database = new MongoClient(dbParams.ConnectionString).GetDatabase("SocialMedia");

            this.users = database.GetCollection<Mongo.DataModels.User>("users");

            var indexModel = new CreateIndexModel<Mongo.DataModels.User>(
                Builders<Mongo.DataModels.User>.IndexKeys.Ascending(u => u.Email),
                new CreateIndexOptions { Unique = true });
            this.users.Indexes.CreateOne(indexModel);
        }

        public bool CheckUser(string email, string passwordHash)
        {
            Mongo.DataModels.User user;

            user = users.Find(u => u.Email == email).FirstOrDefault();

            if (user == null)
            {
                return false;
            }
            
            return true;
        }

        public void CreateUser(string name, string email, string aboutMe, string passwordHash)
        {
            var user = new Mongo.DataModels.User
            {
                _id = this.userIdCounter.GetNewUserId(),
                Name = name,
                Email = email,
                AboutMe = aboutMe,
                PasswordHash = passwordHash,
                RegistredAt = this.dateTimeProvider.Now()
            };

            this.users.InsertOne(user);
        }

        public void DeleteUser(string email)
        {
            this.users.FindOneAndDelete(u => u.Email == email);
        }

        public IEnumerable<long> GetUserPostIds(string userEmail)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string subscriberEmail, int publisherId)
        {
            var publisher = this.users.Find(Builders<Mongo.DataModels.User>.Filter.Eq(u => u._id, publisherId)).FirstOrDefault();
            var subscriber = this.users.Find(Builders<Mongo.DataModels.User>.Filter.Eq(u => u.Email, subscriberEmail)).FirstOrDefault();

            if (publisher == null || subscriber == null)
            {
                throw new ArgumentException($"User with id {publisherId} does not exist.");
            }

            var filter = Builders<Mongo.DataModels.User>.Filter.Eq(u => u.Email, subscriberEmail);

            if (subscriber.SubscribedOn != null)
            {
                var update = Builders<Mongo.DataModels.User>.Update.Push(e => e.SubscribedOn, publisherId);
                this.users.FindOneAndUpdate(filter, update);
            }
            else
            {
                var set = Builders<Mongo.DataModels.User>.Update.Set(e => e.SubscribedOn, new List<int>{ publisherId });
                this.users.FindOneAndUpdate(filter, set);
            }
        }

        public void UpdateUser(string email, string newEmail, string newPasswordHash)
        {
            var updateUserDef = Builders<Mongo.DataModels.User>.Update.Set(u => u.Email, newEmail).Set(u => u.PasswordHash, newPasswordHash);

            this.users.UpdateOne(u => u.Email == email, updateUserDef);
        }

        SocialMedia.Domain.Users.User IUserActions.GetUser(string email, bool includeUserSubscriptions)
        {
            var user = this.users.Find(u => u.Email == email).First();

            var resultUser = this.userMapper.Map(user);

            if (includeUserSubscriptions && user.SubscribedOn != null)
            {
                resultUser.Subscriptions = this.users.Find(u => user.SubscribedOn.Contains(u._id)).ToList().Select(u => this.userMapper.Map(u));
            }

            return resultUser;
        }
    }
}
