using Dapper;
using Domain.Users.Interfaces;
using Global.Environment;
using Global.Environment.Interfaces;
using Repository.Dapper.Infrastructure;
using Repository.Dapper.Mappers.Interfaces;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Repository.Dapper
{
    public class UserActions : IUserActions
    {
        private IDbParams dbParams;
        private IDateTimeProvider dateTimeProvider;
        private IUserMapper userMapper;

        public UserActions(IDbParams dbParams, IDateTimeProvider dateTimeProvider, IUserMapper userMapper)
        {
            this.dbParams = dbParams;
            this.dateTimeProvider = dateTimeProvider;
            this.userMapper = userMapper;
        }

        public void CreateUser(string name, string email, string aboutMe, string passwordHash) // TODO: create user model here
        {
            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                db.Query<User>(
                    $"INSERT INTO [Users] VALUES ( @name, @email, @aboutMe, @registredAt, @passwordHash );",
                    new { @name = name, @email = email, @aboutMe = aboutMe, @registredAt = dateTimeProvider.Now().ToString("yyyy-MM-dd HH:mm:ss.fff"), @passwordHash = passwordHash});
            }
        }

        public void DeleteUser(string email)
        {
            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                db.Query<User>($"DELETE FROM Users WHERE Users.Email = '{email}'");
            }
        }

        public User GetUser(string email, bool includeUserSubscriptions)
        {
            if(!includeUserSubscriptions)
            {
                return this.userMapper.Map(GetUserData(email));
            }

            User user = null;
            List<User> userSubs = new List<User>();

            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                var users = db.Query<User, User, User>(
                    "SELECT u.*, p.*  FROM [dbo].[Users] u JOIN [dbo].[Subscriptions] s ON u.Email = s.SubscriberEmail JOIN Users p On s.PublisherEmail = p.Email WHERE u.Email = @userEmail",
                    (u, sub) =>
                    {
                        if (user == null)
                        {
                            user = u;
                        }

                        userSubs.Add(sub);
                        return null;
                    },
                    new { @userEmail = email },
                    null,
                    true,
                    "PasswordHash, Name");

                if(user == null)
                {
                    return this.GetUser(email, false);
                }

                user.Subscriptions = userSubs;
                return user;
            }
        }


        public void UpdateUser(string email, string newEmail, string passwordHash)
        {
            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                db.Query<User>($"UPDATE dbo.Users SET Email = '{newEmail}', PasswordHash = '{passwordHash}' WHERE Email = {email}");
            }
        }

        public IEnumerable<long> GetUserPostIds(string userEmail)
        {
            throw new NotImplementedException();
        }

        public bool CheckUser(string email, string passwordHash)
        {
            DataModels.User dataUser = null;

            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                dataUser = db.Query<DataModels.User>($"SELECT * FROM Users WHERE Users.Email = '{email}' AND Users.PasswordHash = '{passwordHash}'")
                    .FirstOrDefault();
            }

            if (dataUser == null)
            {
                return false;
            }

            return true;
        }

        internal DataModels.User GetUserData(string email)
        {
            using (IDbConnection db = new SqlConnection(this.dbParams.ConnectionString))
            {
                return db.Query<DataModels.User>($"SELECT * FROM Users WHERE Users.Email = '{email}'")
                    .FirstOrDefault();
            }
        }

        public void Subscribe(string subscriberEmail, int publisherId)
        {
            throw new NotImplementedException();
        }
    }
}
