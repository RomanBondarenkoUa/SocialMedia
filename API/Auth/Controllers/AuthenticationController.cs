using API.Auth.Jwt.Interfaces;
using API.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Auth.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IJwtSigningEncodingKey jwtSigningEncodingKey;
        private IUserService userService;

        public AuthenticationController(
            IJwtSigningEncodingKey signingEncodingKey,
            IUserService userService)
        {
            this.jwtSigningEncodingKey = signingEncodingKey;
            this.userService = userService;
        }


        [AllowAnonymous]
        [HttpGet]
        public string ImATeapot()
        {
            return "I`m a teapot";
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Authenticate(AuthenticationRequest request)
        {
            var userEmail = request.Email;
            var passwordHash = request.PasswordHash;
            
            if(!this.userService.CheckUserCredentials(userEmail, passwordHash))
            {
                throw new UnauthorizedAccessException();
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, userEmail),
            };

            var token = new JwtSecurityToken(
            issuer: "SocialMedia",
            audience: "SocialMediaClient",
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(
                    jwtSigningEncodingKey.GetKey(),
                    jwtSigningEncodingKey.SigningAlgorithm));

            return $"{JwtBearerDefaults.AuthenticationScheme} {new JwtSecurityTokenHandler().WriteToken(token)}";
        }
    }
}
