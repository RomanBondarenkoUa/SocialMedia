using Domain.Users.Interfaces;
using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PostService : IPostService
    {
        IPostActions postActions;
        Mappers.Interfaces.IPostMapper postMapper;

        public PostService(IPostActions postActions, Mappers.Interfaces.IPostMapper postMapper)
        {
            this.postActions = postActions;
            this.postMapper = postMapper;
        }

        public Uri CreatePost(CreatePostByUserModel postModel, string publisherEmail)
        {
            var createPostModel = this.postMapper.Map(postModel, publisherEmail);
            var postId = this.postActions.CreatePost(createPostModel);

            return new Uri($"/post/{postId}", UriKind.Relative);
        }

        public IEnumerable<PostPreviewDto> GetMyPosts(string publisherEmail, int page, int itemsPerPage)
        {
            return this.postMapper.Map(postActions.GetPosts(publisherEmail, page, itemsPerPage));
        }

        public Post GetPost(long postId)
        {
            return postActions.GetPost(postId);
        }

        public IEnumerable<PostPreviewDto> GetUserPosts(int userId, int page, int itemsPerPage)
        {
            return this.postMapper.Map(postActions.GetPostsByPublisherId(userId, page, itemsPerPage));
        }

        public IEnumerable<PostPreviewDto> GetUserFeeds(string userEmail, int page, int itemsOnPage)
        {
            return this.postMapper.Map(this.postActions.GetUserFeeds(userEmail, page, itemsOnPage));
        }
    }
}
