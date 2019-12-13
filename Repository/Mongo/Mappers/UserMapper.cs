using Repository.Mongo.DataModels;
using Repository.Mongo.Mappers.Interfaces;
using SocialMedia.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Mongo.Mappers
{
    public class UserMapper : IUserMapper
    {
        public SocialMedia.Domain.Users.User Map(DataModels.User user)
        {
            return new SocialMedia.Domain.Users.User()
            {
                Name = user.Name,
                AboutMe = user.AboutMe,
                RegistredAt = user.RegistredAt.ToUniversalTime()
            };
        }
    }
}
