using Domain.Users.Posts;
using Global.Environment.Interfaces;
using Repository.Neo4j.DataModels;
using Repository.Neo4j.Mappers.Interfaces;

namespace Repository.Neo4j.Mappers
{
    public class PostMapper : IPostMapper
    {
        private IDateTimeProvider dateTimeProvider;
        private IUriFormatter uriFormatter;

        public PostMapper(IDateTimeProvider dateTimeProvider, IUriFormatter uriFormatter)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.uriFormatter = uriFormatter;
        }

        public Post Map(CreatePostModel createPostModel, long postId)
        {
            return new Neo4j.DataModels.Post
            {
                Id = postId,
                AttachmentUrl = createPostModel.AttachmentUrl,
                CreatedAt = dateTimeProvider.Now(),
                Text = createPostModel.Text,
                Title = createPostModel.Title,
            };
        }
    }
}
