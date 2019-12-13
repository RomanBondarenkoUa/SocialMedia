using API.Infrastructure.Helpers;
using API.Infrastructure.Models;
using Domain.Users.Posts;
using Domain.Users.SocialActivities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace API.Auth.Controllers
{
    [Route("api/post")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost]
        public Result<Uri> CreatePost(CreatePostByUserModel createPostModel)
        {
            var email = HttpContext.User.ExtractEmail();

            return new Result<Uri>(this.postService.CreatePost(createPostModel, email));
        }

        [HttpGet]
        public Result<IEnumerable<PostPreviewDto>> GetPosts(
            [FromQuery] int page = -1,
            [FromQuery] int itemsPerPage = 30)
        {
            var email = HttpContext.User.ExtractEmail();

            return new Result<IEnumerable<PostPreviewDto>>(this.postService.GetMyPosts(email, page, itemsPerPage));
        }

        [HttpGet]
        [Route("/post/{postId}")]
        public Result<Post> GetPost([FromRoute] long postId)
        {
            return new Result<Post>(this.postService.GetPost(postId));
        }

        [HttpGet]
        [Route("/user/posts/{userId}")]
        public Result<IEnumerable<PostPreviewDto>> GetPosts([FromRoute] int userId, [FromQuery] int page, [FromQuery] int itemsPerPage)
        {
            return new Result<IEnumerable<PostPreviewDto>>(this.postService.GetUserPosts(userId, page, itemsPerPage));
        }
    }
}