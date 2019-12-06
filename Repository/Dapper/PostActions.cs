using Dapper;
using Domain.Users.Interfaces;
using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Global.Environment;
using Global.Environment.Interfaces;
using Repository.Dapper.DataModels;
using Repository.Dapper.Infrastructure;
using Repository.Dapper.Mappers.Interfaces;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository.Dapper
{
    public class PostActions : IPostActions
    {
        private IDbParams dbParams;
        private IDateTimeProvider dateTimeProvider;
        private IPostMapper postMapper;
        private IUserActions userActions;

        public PostActions(IDbParams dbParams, IDateTimeProvider dateTimeProvider, IPostMapper postMapper, IUserActions userActions)
        {
            this.dbParams = dbParams;
            this.dateTimeProvider = dateTimeProvider;
            this.postMapper = postMapper;
            this.userActions = userActions;
        }

        public long CreatePost(CreatePostModel createPostModel)
        {
            var post = this.postMapper.Map(createPostModel);
            long postId;

            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                postId = db.Query<long>(
                    $"INSERT INTO [Posts] VALUES ( @publisherEmail, @title, @text, @attachment, @createdAt); SELECT CAST(SCOPE_IDENTITY() as BIGINT);",
                    new { @publisherEmail = post.PublisherEmail, @title = post.Title, @text = post.Text, @attachment = post.AttachmentUrl, @createdAt = this.dateTimeProvider.Now().ToString("yyyy-MM-dd HH:mm:ss.fff") })
                    .First();
            }
            return postId;
        }

        public Domain.Users.SocialActivities.Post GetPost(long postId)
        {
            DataModels.Post post = null;
            string publisherName = null;
            IEnumerable<Reaction> reactions = null;
            IEnumerable<Comment> comments = null;

            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                Dictionary<Reaction, string> commentPairs = new Dictionary<Reaction, string>();

                reactions = db.Query<DataModels.Post, User, Reaction, Reaction>(
                    "SELECT * FROM [Posts] AS p JOIN [Users] AS u ON p.PublisherEmail = u.Email FULL OUTER JOIN [Reactions] AS r ON r.PostId = p.Id WHERE [p].[Id] = @postId",
                    (p, u, r) =>
                    {
                        if (post == null)
                        {
                            post = new DataModels.Post()
                            {
                                Id = p.Id,
                                Title = p.Title,
                                Text = p.Text,
                                AttachmentUrl = p.AttachmentUrl,
                                CreatedAt = p.CreatedAt,
                                PublisherEmail = p.PublisherEmail
                            };
                        }
                        if (r != null)
                        {
                            commentPairs.Add(r, this.userActions.GetUser(r.UserEmail, false).Name);
                        }

                        return null;
                    },
                    new { @postId = postId },
                    null,
                    true,
                    "Name, UserEmail");

                comments = commentPairs.Select(c => this.postMapper.Map(c.Key, c.Value));

                publisherName = db.QueryFirst<string>("SELECT u.Name FROM [Users] AS u where u.Email = @publisherEmail", new { @publisherEmail = post.PublisherEmail });
            }

            return this.postMapper.Map(post, publisherName, post.PublisherEmail, comments);
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPosts(string publisherEmail, int page, int itemsPerPage)
        {
            long itemsToSkip = page * itemsPerPage;

            IEnumerable<DataModels.Post> posts;

            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                posts = db.Query<DataModels.Post>("SELECT * FROM [Posts] ORDER BY Posts.CreatedAt DESC OFFSET @offset ROWS FETCH NEXT @itemsOnPage ROWS ONLY;",
                    new { @offset = itemsToSkip, @itemsOnPage = itemsPerPage });

            }

            return posts.Select(p => this.postMapper.Map(p, this.userActions.GetUser(p.PublisherEmail, false).Name, p.PublisherEmail, new List<Comment>())); // TODO: implement valid comments
        }

        public void DeletePost(long postId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetPostsByPublisherId(int publisherId, int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.Users.SocialActivities.Post> GetUserFeeds(string userEmail, int page, int itemsOnPage)
        {
            throw new NotImplementedException();
        }
    }
}
