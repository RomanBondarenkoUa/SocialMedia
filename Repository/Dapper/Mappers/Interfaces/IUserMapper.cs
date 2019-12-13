using SocialMedia.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Dapper.Mappers.Interfaces
{
    public interface IUserMapper
    {
        User Map(DataModels.User user);
    }
}
