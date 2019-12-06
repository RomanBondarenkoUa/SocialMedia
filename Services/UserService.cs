using Domain.Users.Posts;
using Services.Interfaces;
using SocialMedia.Domain.Users;
using SocialMedia.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Authentication;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserActions userActions;

        public UserService(IUserActions userOperations)
        {
            this.userActions = userOperations;
        }

        public void CreateUser(Domain.Users.Conversations.Users.CreateUserModel createUserModel)
        {
            this.userActions.CreateUser(createUserModel.Name, createUserModel.Email, createUserModel.AboutMe, createUserModel.PasswordHash);
        }

        public void DeleteUser(string email, string passwordhash)
        {
            var checkResult = this.CheckUserCredentials(email, passwordhash);

            if(checkResult == false)
            {
                throw new InvalidCredentialException();
            }

            this.userActions.DeleteUser(email);
        }

        public User GetUser(string email, bool includeUserSubscriptions)
        {
            return this.userActions.GetUser(email, includeUserSubscriptions);
        }

        public void UpdateUser(string email, string newEmail, string newPasswordHash)
        {
            var user = this.userActions.GetUser(email, false);

            if (user == null)
            {
                throw new InvalidOperationException("Cant`t update nonexistent user.");
            }

            this.userActions.UpdateUser(email, newEmail, newPasswordHash);
        }

        public bool CheckUserCredentials(string email, string passwordHash)
        {
            return this.userActions.CheckUser(email, passwordHash);
        }

        public void Subscribe(string subscriberEmail, int publisherId)
        {
            this.userActions.Subscribe(subscriberEmail, publisherId);
        }
    }
}
