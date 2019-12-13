using System.Collections.Generic;
using System.Linq;
using API.Infrastructure.Helpers;
using Domain.Users.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Auth.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SocialController : ControllerBase
    {
        public IUserService userService;
        public IPostService postService;

        public SocialController(IUserService userService, IPostService postService)
        {
            this.userService = userService;
            this.postService = postService;
        }

        [HttpPost]
        [Route("/[controller]/me/subscribe/{userId}")]
        public void Subscribe([FromRoute] int userId)
        {
            var email = HttpContext.User.ExtractEmail();

            this.userService.Subscribe(email, userId);
        }

        [HttpGet]
        [Route("/[controller]/me/feeds")]
        public IEnumerable<PostPreviewDto> GetFeeds([FromQuery]int page, [FromQuery]int itemsPerPage)
        {
            var email = HttpContext.User.ExtractEmail();

            return this.postService.GetUserFeeds(email, page, itemsPerPage);
        }
    }
}