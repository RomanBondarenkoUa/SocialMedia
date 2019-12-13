using Domain.Users.Posts;

namespace Repository.Neo4j.Mappers.Interfaces
{
    public interface IPostMapper
    {
        Neo4j.DataModels.Post Map(CreatePostModel createPostModel, long postId);
    }
}
