using API.Infrastructure.Helpers;
using API.Infrastructure.Models;
using Domain.Users.Conversations.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using SocialMedia.Domain.Users;

namespace API.Auth.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public Result<User> GetUser(bool includeUserSubscriptions)
        {
            var email = HttpContext.User.ExtractEmail();

            return new Result<User>(this.userService.GetUser(email, includeUserSubscriptions));
        }

        [HttpPatch]
        public void UpdateUser(
            [FromHeader] string newEmail,
            [FromHeader] string newPasswordHash)
        {
            var email = HttpContext.User.ExtractEmail();

            this.userService.UpdateUser(
                email,
                newEmail,
                newPasswordHash);
        }

        [HttpPost]
        [AllowAnonymous]
        public void CreateUser([FromBody] CreateUserModel createUserModel)   // TODO: Pack this params to the input model and make modelState.Valid verification
        {
            this.userService.CreateUser(createUserModel);
        }

        [HttpDelete]
        public void DeleteUser(
            [FromHeader] string passwordHash)
        {
            var email = HttpContext.User.ExtractEmail();
            this.userService.DeleteUser(email, passwordHash);
        }
    }
}
