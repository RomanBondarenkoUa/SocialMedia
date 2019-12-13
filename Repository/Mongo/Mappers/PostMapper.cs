using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Global.Environment.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Repository.Mongo.DataModels;
using Repository.Mongo.Mappers.Interfaces;
using System;
using System.Linq;
using Repository.Mongo.Helpers;

namespace Repository.Mongo.Mappers
{
    public class PostMapper : IPostMapper
    {
        IUriFormatter uriFormatter;

        public PostMapper(IUriFormatter uriFormatter)
        {
            this.uriFormatter = uriFormatter;
        }

        public Mongo.DataModels.Post Map(CreatePostModel createPostModel, DateTime createdAt, int publisherId, long postId)
        {
            return new DataModels.Post()
            {
                _id = postId,
                Title = createPostModel.Title,
                Text = createPostModel.Text,
                AttachmentUrl = createPostModel.AttachmentUrl,
                PublisherId = publisherId,
                CreatedAt = createdAt,
                Reactions = new System.Collections.Generic.List<Reaction>(),
            };
        }

        public Domain.Users.SocialActivities.Post Map(Mongo.DataModels.Post post, IMongoCollection<Mongo.DataModels.User> users)
        {
            var publisher = users.Find(new BsonDocument("_id", post.PublisherId)).First();

            return new Domain.Users.SocialActivities.Post
            {
                Title = post.Title,
                Text = post.Text,
                CreatedAt = post.CreatedAt,
                AttachmentUrl = post.AttachmentUrl,
                Comments = post.Reactions.Select(r => new Comment()
                {
                    Text = r.Text,
                    CreatorName = users.Find(new BsonDocument("_id", r.UserId)).First().Name,
                    IsPositiveComment = r.IsPositive
                }),
                OwnerName = publisher.Name,
                OwnerProfileLink = this.uriFormatter.GetUserUri(publisher._id),
                PostUrl = this.uriFormatter.GetPostUri(post._id),
                TotalPositiveRate = post.Reactions.AggregatePositiveReactionsRate(),
                TotalNegativeRate = post.Reactions.AggregateNegativeReactionsRate()
            };
        }
    }
}