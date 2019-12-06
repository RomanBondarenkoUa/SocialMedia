using Domain.Users.Posts;
using System.Collections.Generic;

namespace Services.Mappers.Interfaces
{
    public interface IPostMapper
    {
        CreatePostModel Map(CreatePostByUserModel createPostByUserModel, string email);

        IEnumerable<PostPreviewDto> Map(IEnumerable<Domain.Users.SocialActivities.Post> posts);
    }
}
