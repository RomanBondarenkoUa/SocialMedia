using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Repository.Dapper.DataModels;
using SocialMedia.Domain.Users;
using System.Collections.Generic;

namespace Repository.Dapper.Mappers.Interfaces
{
    public interface IPostMapper
    {
        DataModels.Post Map(CreatePostModel createPostModel);

        Domain.Users.SocialActivities.Post Map(Repository.Dapper.DataModels.Post post, string publisherName, string publisherEmail, IEnumerable<Comment> comments);

        Comment Map(Reaction reaction, string reactionAuthorName);
    }
}
