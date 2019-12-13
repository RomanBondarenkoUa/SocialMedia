using Domain.Users.Interfaces;
using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Global.Environment.Interfaces;
using Neo4jClient;
using Repository.Helpers;
using Repository.Neo4j.Infrastructure;
using Repository.Neo4j.Infrastructure.Models;
using Repository.Neo4j.Mappers.Interfaces;
using Repository.Neo4j.Requests.Interfaces;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Neo4j
{
    public class PostActions : IPostActions, IDisposable
    {
        private GraphClient client;
        private IPostMapper postMapper;
        private INeo4jRequests neo4jRequests;
        private IUriFormatter uriFormatter;
        private IUserActions userActions;

        public PostActions(
            INeo4jDbParams neo4JDbParams,
            IPostMapper postMapper,
            INeo4jRequests neo4jRequests,
            IUriFormatter uriFormatter,
            IUserActions userActions)
        {
            var neo4jCreds = neo4JDbParams.GetNeo4jConnection();

            this.client = new GraphClient(neo4jCreds.ConnectionUri, neo4jCreds.UserName, neo4jCreds.UserPassword);
            this.postMapper = postMapper;
            this.neo4jRequests = neo4jRequests;
            this.uriFormatter = uriFormatter;
            this.userActions = userActions;
        }

        public long CreatePost(CreatePostModel createPostModel)
        {
            this.client.Connect();

            var newPostId = this.GetNewPostId();

            var post = this.postMapper.Map(createPostModel, newPostId);
            post.Id = newPostId;

            this.client.Cypher
                .Match("(publisher:User)")
                .Where((Neo4j.DataModels.User publisher) => publisher.Email == createPostModel.PublisherEmail)
                .Create("(publisher)-[:CREATE_POST]->(post:Post {newPost})")
                .WithParam("newPost", post)
                .ExecuteWithoutResults();

            return newPostId;
        }

        public void DeletePost(long postId)
        {
            this.client.Connect();

            this.client.Cypher
                .Match("(post:Post)")
                .Where((Neo4j.DataModels.Post post) => post.Id == postId)
                .DetachDelete("post")
                .ExecuteWithoutResults();
        }

        public Domain.Users.SocialActivities.Post GetPost(long postId)
        {
            this.client.Connect();

            var postModel = this.client.Cypher
                .Match("(publisher:User)-[:CREATE_POST]->(post:Post)")
                .Where((Neo4j.DataModels.Post post) => post.Id == postId)
                .OptionalMatch("(post)<-[:COMMENT_FOR]-(reaction:Reaction)<-[:LEAVE_REACTION]-(commentator:User)")
                .Return((publisher, post, reaction, commentator) =>
                    new
                    {
                        publisher = publisher.As<Neo4j.DataModels.User>(),
                        post = post.As<Neo4j.DataModels.Post>(),
                        reaction = reaction.CollectAs<Neo4j.DataModels.Reaction>(),
                        commentator = commentator.CollectAs<Neo4j.DataModels.User>(),
                    })
                .Results
                .FirstOrDefault();

            if (postModel == null)
            {
                throw new ArgumentException($"User with id: {postId} is not found");
            }

            return this.ComposePost(
                postModel.publisher,
                postModel.post,
                postModel.reaction,
                postModel.commentator);
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPosts(string publisherEmail, int page, int itemsPerPage)
        {
            var postIds = this.userActions.GetUserPostIds(publisherEmail);

            int fromItem = page * itemsPerPage;
            int toItem = fromItem + itemsPerPage;

            this.client.Connect();

            var posts = this.client.Cypher
                .Match("(publisher:User)-[:CREATE_POST]->(post:Post)")
                .Where((Neo4j.DataModels.User publisher) => publisher.Email == publisherEmail)
                .AndWhere((Neo4j.DataModels.Post post) => post.Id >= fromItem && post.Id <= toItem)
                .OptionalMatch("(post)<-[:COMMENT_FOR]-(reaction:Reaction)<-[:LEAVE_REACTION]-(commentator:User)")
                .Return((publisher, post, reaction, commentator) => 
                    new
                    {
                        publisher = publisher.As<Neo4j.DataModels.User>(),
                        post = post.As<Neo4j.DataModels.Post>(),
                        reaction = reaction.CollectAs<Neo4j.DataModels.Reaction>(),
                        commentator = commentator.CollectAs<Neo4j.DataModels.User>(),
                    })
                .OrderByDescending("post.CreatedAt")
                .Results;

            return posts.Select(p => this.ComposePost(p.publisher, p.post, p.reaction, p.commentator));
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPostsByPublisherId(int publisherId, int page, int itemsPerPage)
        {
            this.client.Connect();

            var publisherEmail = this.client.Cypher
                .Match("(user:User)")
                .Where((Neo4j.DataModels.User user) => user.Id == publisherId)
                .Return(user => user.As<Neo4j.DataModels.User>().Email)
                .Results
                .FirstOrDefault();

            if(publisherEmail == null)
            {
                throw new ArgumentNullException($"User with id : {publisherId} does not exist.");
            }

            return this.GetPosts(publisherEmail, page, itemsPerPage);
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetUserFeeds(string userEmail, int page, int itemsOnPage)
        {
            var itemsToSkip = page * itemsOnPage;

            this.client.Connect();
            var feedPosts = this.client.Cypher
                .Match("(subscriber:User)-[:SUBSCRIBED]->(publisher:User)-[:CREATE_POST]->(post:Post)")
                .Where((Neo4j.DataModels.User subscriber) => subscriber.Email == userEmail)
                .OptionalMatch("(post)<-[:COMMENT_FOR]-(reaction:Reaction)<-[:LEAVE_REACTION]-(commentator:User)")
                .Return((publisher, post, reaction, commentator) =>
                    new 
                    {
                        Publisher = publisher.As<Neo4j.DataModels.User>(),
                        Post = post.As<Neo4j.DataModels.Post>(),
                        Reactions = reaction.CollectAs<Neo4j.DataModels.Reaction>(),
                        Commentators = commentator.CollectAs<Neo4j.DataModels.User>(),
                    })
                .OrderByDescending("post.CreatedAt")
                .Skip(itemsToSkip)
                .Limit(itemsOnPage)
                .Results;

            return feedPosts.Select(p => this.ComposePost(p.Publisher, p.Post, p.Reactions, p.Commentators));
        }

        private long GetNewPostId()
        {
            client.Connect();

            return client.Cypher
                .Merge(this.neo4jRequests.AutoIncrementor_MergeRequest)
                .Set(this.neo4jRequests.AutoIncrementor_GetNewPostId_SetRequest)
                .Return(increment => increment.As<Incrementor>().LatestPostId)
                .Results
                .First()
                .Value;
        }


        public Domain.Users.SocialActivities.Post ComposePost(
            Neo4j.DataModels.User publisher,
            Neo4j.DataModels.Post postModel, 
            IEnumerable<Neo4j.DataModels.Reaction> reactions,
            IEnumerable<Neo4j.DataModels.User> commentators)
        {
            if(postModel == null)
            {
                throw new ArgumentNullException("Post does not exist.");
            }

            var comments = new List<Comment>();
            reactions.Zip(commentators, (r, c) =>
            {
                comments.Add(new Comment { CreatorName = c.Name, IsPositiveComment = r.IsPositive, Text = r.Text });
                return r;
            });
            
            return new Domain.Users.SocialActivities.Post
            {
                Text = postModel.Text,
                Title = postModel.Title,
                AttachmentUrl = postModel.AttachmentUrl,
                PostUrl = this.uriFormatter.GetPostUri(postModel.Id),
                CreatedAt = postModel.CreatedAt,
                Comments = comments,
                OwnerName = publisher?.Name,
                OwnerProfileLink = this.uriFormatter.GetUserUri(publisher.Id),
                TotalNegativeRate = CommentHelper.AggregateNegativeCommentsRate(comments),
                TotalPositiveRate = CommentHelper.AggregatePositiveCommentsRate(comments),
            };
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
