using AutoMapper;
using Repository.Dapper.Mappers.Interfaces;
using SocialMedia.Domain.Users;

namespace Repository.Dapper.Mappers
{
    public class UserMapper : IUserMapper
    {
        IMapper mapper;

        public UserMapper()
        {
            this.mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DataModels.User, User>();
            })
            .CreateMapper();
        }

        public User Map(DataModels.User user)
        {
            return this.mapper.Map<User>(user);
        }
    }
}
