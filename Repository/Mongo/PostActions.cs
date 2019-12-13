using Domain.Users.Interfaces;
using Domain.Users.Posts;
using Global.Environment.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Mongo.Infrastructure.Interfaces;
using Repository.Mongo.Mappers.Interfaces;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Mongo
{
    public class PostActions : IPostActions
    {
        private readonly IUserActions userActions;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IPostMapper postMapper;
        private readonly IMongoCollection<Mongo.DataModels.Post> posts;
        private readonly IMongoCollection<Mongo.DataModels.User> users;
        private readonly IPostIdCounter postIdCounter;

        public PostActions(
            IDbParams dbParams, 
            IDateTimeProvider dateTimeProvider,
            IUserActions userActions,
            IPostMapper postMapper,
            IPostIdCounter postIdCounter)
        {
            this.userActions = userActions;
            this.dateTimeProvider = dateTimeProvider;
            this.userActions = userActions;
            this.postMapper = postMapper;
            this.postIdCounter = postIdCounter;

            IMongoDatabase database = new MongoClient(dbParams.ConnectionString).GetDatabase("SocialMedia");


            this.users = database.GetCollection<Mongo.DataModels.User>("users");
            this.posts = database.GetCollection<Mongo.DataModels.Post>("posts");
        }

        public long CreatePost(CreatePostModel createPostModel)
        {
            var user = this.users.Find(u => u.Email == createPostModel.PublisherEmail).FirstOrDefault();
            var post = this.postMapper.Map(createPostModel, this.dateTimeProvider.Now(), user._id, this.postIdCounter.GetNewPostId());
            posts.InsertOne(post);

            return post._id;
        }

        public void DeletePost(long postId)
        {
            this.posts.DeleteOne(Builders<Mongo.DataModels.Post>.Filter.Eq("_id", postId));
        }

        public Domain.Users.SocialActivities.Post GetPost(long postId)
        {
            return this.postMapper.Map(this.posts.Find(new BsonDocument("_id", postId)).First(), this.users);
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPosts(string publisherEmail, int page, int itemsPerPage)
        {
            long fromId = page * itemsPerPage;
            long toId = fromId + itemsPerPage;
            return this.posts.Find(p => p._id >= fromId && p._id <= toId).ToList().Select(x => this.postMapper.Map(x, this.users));
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPostsByPublisherId(int publisherId, int page, int itemsPerPage)
        {
            long fromId = page * itemsPerPage;
            long toId = fromId + itemsPerPage;

            return this.posts.Find(p => p.PublisherId == publisherId && p._id >= fromId && p._id <= toId)
                .ToList().Select(x => this.postMapper.Map(x, this.users));
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetUserFeeds(string userEmail, int page, int itemsOnPage)
        {
            var fromId = page * itemsOnPage;
            
            var user = this.users.Find(Builders<Mongo.DataModels.User>.Filter.Eq(u => u.Email, userEmail)).FirstOrDefault();
            if (user == null)
            {
                throw new ArgumentException($"User with email {userEmail} does not exist.");
            }

            var posts = this.posts.Find(p => user.SubscribedOn.Contains(p.PublisherId))
                .SortByDescending(p => p.CreatedAt)
                .Skip(fromId)
                .Limit(itemsOnPage)
                .ToList()
                .Select(p => this.postMapper.Map(p, this.users));

            return posts;
        }
    }
}
