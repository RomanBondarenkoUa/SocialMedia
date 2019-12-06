using System.Collections.Generic;
using Domain.Users.Posts;

namespace SocialMedia.Domain.Users.Interfaces
{
    public interface IUserActions
    {
        User GetUser(string email, bool includeUserSubscriptions);

        bool CheckUser(string email, string passwordHash);

        void CreateUser(string name, string email, string aboutMe, string passwordHash);

        void UpdateUser(string email, string newEmail, string newPasswordHash);

        void DeleteUser(string email);

        IEnumerable<long> GetUserPostIds(string userEmail);

        void Subscribe(string subscriberEmail, int publisherId);
    }
}
