using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using System;
using System.Collections.Generic;
using Domain.Users.Posts.Comments;

namespace Services.Interfaces
{
    public interface IPostService
    {
        Uri CreatePost(CreatePostByUserModel postModel, string publisherEmail);

        Post GetPost(long postId);

        IEnumerable<PostPreviewDto> GetMyPosts(string publisherEmail, int page, int itemsPerPage);

        IEnumerable<PostPreviewDto> GetUserPosts(int userId, int page, int itemsPerPage);

        IEnumerable<PostPreviewDto> GetUserFeeds(string userEmail, int page, int itemsOnPage);

        void LeaveComment(CreateCommentModel createCommentModel);
    }
}
