using Domain.Users.Posts;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Repository.Mongo.Mappers.Interfaces
{
    public interface IPostMapper
    {
        Mongo.DataModels.Post Map(CreatePostModel createPostModel, DateTime createdAt, int publisherId, long postId);

        Domain.Users.SocialActivities.Post Map(Mongo.DataModels.Post post, IMongoCollection<Mongo.DataModels.User> users);
    }
}
