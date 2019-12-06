using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using System.Collections.Generic;

namespace Domain.Users.Interfaces
{
    public interface IPostActions
    {
        long CreatePost(CreatePostModel createPostModel);

        Post GetPost(long postId);

        IEnumerable<Post> GetPosts(string publisherEmail, int page, int itemsPerPage);

        void DeletePost(long postId);

        IEnumerable<Domain.Users.SocialActivities.Post> GetPostsByPublisherId(int publisherId, int page, int itemsPerPage);

        IEnumerable<Domain.Users.SocialActivities.Post> GetUserFeeds(string userEmail, int page, int itemsOnPage);
    }
}
