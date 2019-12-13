using Repository.Neo4j.Mappers.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Neo4j.Mappers
{
    public class UserMapper : IUserMapper
    {
        public IEnumerable<SocialMedia.Domain.Users.User> Map(IEnumerable<Neo4j.DataModels.User> users)
        {
            if (users == null)
            {
                return null;
            }

            return users.Select(u =>
                new SocialMedia.Domain.Users.User
                {
                    AboutMe = u.AboutMe,
                    Name = u.Name,
                    RegistredAt = u.RegistredAt,
                });
        }
    }
}
