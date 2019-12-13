
using System.Collections.Generic;

namespace Repository.Neo4j.Mappers.Interfaces
{
    public interface IUserMapper
    {
        IEnumerable<SocialMedia.Domain.Users.User> Map(IEnumerable<Neo4j.DataModels.User> users);
    }
}
