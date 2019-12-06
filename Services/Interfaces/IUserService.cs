using Domain.Users.Posts;
using SocialMedia.Domain.Users;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IUserService
    {
        void CreateUser(Domain.Users.Conversations.Users.CreateUserModel createUserModel);

        void DeleteUser(string email, string passwordHash);

        User GetUser(string email, bool includeUserSubscriptions);

        void UpdateUser(string email, string newEmail, string newPasswordHash);

        bool CheckUserCredentials(string email, string passwordHash);

        void Subscribe(string subscriberEmail, int publisherId);
    }
}
