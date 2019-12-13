using System;
namespace Repository.Mongo.Mappers.Interfaces
{
    public interface IUserMapper
    {
        SocialMedia.Domain.Users.User Map(Mongo.DataModels.User user);
    }
}
